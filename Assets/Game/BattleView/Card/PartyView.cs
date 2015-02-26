using System;
using System.Collections.Generic;
using System.Linq;
using Gem;
using UnityEngine;
using CardView = Choanji.UI.CardView;

namespace Choanji.Battle
{
	public class PartyView : MonoBehaviour
	{
		private const float X_MARGIN = 0.2f;
		private const float Y_MARGIN = 0.02f;
		private const float CARD_HEIGHT = 0.9f;
		private const float CARD_WIDTH = 1 / (6 + X_MARGIN * 7);

		private AP mCost;
		private readonly Dictionary<CardView, Card> mMap = new Dictionary<CardView, Card>();
		private readonly HashSet<CardView> mSelection = new HashSet<CardView>();

		public Action<Card> onSelect;
		public Action<Card> onCancel;
		public Action<bool, Card> onPointerOver;

		public void Setup(Party _party)
		{
			var i = 0;
			var _cardPrf = UI.DB.g.cardViewPrf;

			foreach (var _card in _party.actives)
			{
				var _view = _cardPrf.Instantiate();
				Position((RectTransform)_view.transform, i);
				_view.SetCard(_card, CardMode.ACTIVE);
				mMap.Add(_view, _card);

				_view.onSelect += () => OnSelect(_view);
				_view.onCancel += () => Cancel(_card);
				_view.onPointerOver += _enter => OnPointerOver(_enter, _view);

				++i;
			}

			foreach (var _card in _party.passives)
			{
				var _view = _cardPrf.Instantiate();
				Position((RectTransform)_view.transform, i);
				_view.SetCard(_card, CardMode.PASSIVE);
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

		private void OnSelect(CardView _view)
		{
			if (mSelection.Contains(_view))
				return;

			var _card = mMap[_view];
			var _cost = _card.data.active.cost;

			var _battler = TheBattle.state.battlerA;
			if (mCost + (int)_battler.CalConsumption(_cost) > _battler.ap)
				return;

			mCost += (int)_cost;
			mSelection.Add(_view);
			onSelect.CheckAndCall(mMap[_view]);
		}

		private CardView mPointerOver;
		private void OnPointerOver(bool _enter, CardView _card)
		{
			if (mPointerOver != null && mPointerOver != _card)
				onPointerOver.CheckAndCall(mPointerOver, mPointerOver.card);
			mPointerOver = _card;
			onPointerOver.CheckAndCall(_enter, mPointerOver.card);
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

		private void Cancel(CardView _view)
		{
			if (!mSelection.Contains(_view))
				return;
			var _card = mMap[_view];
			mCost -= _card.data.active.cost;
			mSelection.Remove(_view);
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