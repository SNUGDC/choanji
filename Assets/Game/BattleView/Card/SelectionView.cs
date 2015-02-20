using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class SelectionView : MonoBehaviour
	{
		private const float WIDTH = 1 / 6.5f;
		private const float MARGIN = WIDTH * 0.1f;

		private readonly List<Pair<Card, SelectionCell>> mCards = new List<Pair<Card, SelectionCell>>();

		public Action<Card> onCancel;

		public void Add(Card _card)
		{
			var _cell = PrefabDB.g.selectedCardCell.Instantiate();
			_cell.transform.SetParent(transform);
			Position(_cell, mCards.Count);

			_cell.card = _card.data.key;
			_cell.onCancel += () =>
			{
				Remove(_card);
				onCancel.CheckAndCall(_card);
			};

			mCards.Add(new Pair<Card, SelectionCell>(_card, _cell));
		}

		public void Remove(Card _card)
		{
			var _idx = mCards.FindIndex(_kv => _kv.first == _card);
			if (_idx < 0) return;

			var _found = mCards[_idx];
			Destroy(_found.second.gameObject);
			mCards.RemoveAt(_idx);

			for (var i = _idx; i < mCards.Count; ++i)
				Position(mCards[i].second, i);
		}

		public void Clear()
		{
			foreach (var _card in mCards)
				Destroy(_card.second.gameObject);
			mCards.Clear();
		}

		private static void Position(Component _cell, int i)
		{
			var _rect = (RectTransform)_cell.transform;
			_rect.offsetMin = Vector2.zero;
			_rect.offsetMax = Vector2.zero;
			_rect.anchorMin = new Vector2(i * (WIDTH + MARGIN), 0);
			_rect.anchorMax = new Vector2((i + 1) * WIDTH + i * MARGIN, 1);
		}
	}

}