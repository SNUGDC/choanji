using System;
using System.Collections.Generic;
using System.Linq;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class PartyView : MonoBehaviour
	{
		private const float X_MARGIN = 0.2f;
		private const float Y_MARGIN = 0.02f;
		private const float CARD_HEIGHT = 0.9f;
		private const float CARD_WIDTH = 1 / (6 + X_MARGIN * 7);

		private AP mCost;
		private readonly Dictionary<int, Card> mMap = new Dictionary<int, Card>();
		private readonly HashSet<int> mSelection = new HashSet<int>();

		public Action<Card> onSelect;
		public Action<Card> onCancel;

		public void Setup(Party _party)
		{
			var i = 0;

			foreach (var _card in _party.actives)
			{
				var _view = PrefabDB.g.activeCardView.Instantiate();
				Position((RectTransform)_view.transform, i);
				_view.card.Setup(_card.data);
				_view.Setup(_card.data.active);

				var _id = _view.GetInstanceID();
				mMap.Add(_id, _card);

				_view.card.onSelect += () => OnSelect(_id);
				_view.card.onCancel += () => Cancel(_card);

				++i;
			}

			foreach (var _card in _party.passives)
			{
				var _view = PrefabDB.g.passiveCardView.Instantiate();
				Position((RectTransform)_view.transform, i);
				_view.card.Setup(_card.data);
				_view.Setup(_card.data.passive);
				++i;
			}
		}

		private void Position(RectTransform _rect, int _idx)
		{
			var _size = new Vector2(CARD_WIDTH, CARD_HEIGHT);
			_rect.SetParent(transform);
			_rect.offsetMin = Vector2.zero;
			_rect.offsetMax = Vector2.zero;
			_rect.anchorMin = new Vector2(CARD_WIDTH * (X_MARGIN * (_idx + 1) + _idx), Y_MARGIN);
			_rect.anchorMax = _size + _rect.anchorMin;
		}

		private void OnSelect(int _id)
		{
			if (mSelection.Contains(_id))
				return;

			var _card = mMap[_id];
			var _cost = _card.data.active.cost;

			if (mCost + (int)_cost > TheBattle.state.battlerA.ap)
				return;

			mCost += (int)_cost;
			mSelection.Add(_id);
			onSelect.CheckAndCall(mMap[_id]);
		}

		public void Cancel(Card _card)
		{
			foreach (var _kv in mMap)
			{
				if (_kv.Value == _card)
				{
					Cancel(_kv.Key);
					return;
				}
			}
		}

		private void Cancel(int _id)
		{
			if (!mSelection.Contains(_id))
				return;
			var _card = mMap[_id];
			mCost -= _card.data.active.cost;
			mSelection.Remove(_id);
			onCancel.CheckAndCall(_card);
		}

		public List<Card> Submit()
		{
			var _ret = mSelection.ToList().ConvertAll(_viewID => mMap[_viewID]);
			mCost = 0;
			mSelection.Clear();
			return _ret;
		}
	}
}