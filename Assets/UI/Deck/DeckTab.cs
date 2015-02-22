using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji.UI
{
	public class DeckTab : MonoBehaviour
	{
		private const int CELL_ROW = 4;
		private const float CELL_MARGIN = 0.01f;
		private const float CELL_SIZE = 1f/CELL_ROW - CELL_MARGIN;
		private const float CELL_PIXEL_SIZE = 75f;

		public RectTransform cardDetailParent;
		[HideInInspector]
		public CardDetail cardDetail;

		public RectTransform deckTable;

		private readonly Dictionary<CardID, DeckCell> mCells = new Dictionary<CardID, DeckCell>();

		private Deck mDeck;
		public Deck deck
		{
			set
			{
				mDeck = value;
				Refresh();
			}
		}

		private Party mParty;
		public Party party
		{
			get { return mParty; }
			set
			{
				mParty = value;
				Refresh();
			}
		}

		void Start()
		{
			{
				cardDetail = DB.g.cardDetailPrf.Instantiate();
				var _rect = cardDetail.gameObject.RectTransform();
				_rect.SetParent(cardDetailParent, false);
				_rect.Fill();	
			}

			{
				var _count = CardDB.count;

				if (_count / CELL_ROW > 6)
					deckTable.SetMinY(-(CELL_PIXEL_SIZE + 5) * (_count / CELL_ROW - 6));

				var i = 0;
				foreach (var _data in CardDB.GetEnum())
				{
					var _cell = DB.g.deckCellPrf.Instantiate();

					_cell.canSelectPassive = () => !party.isFull;
					_cell.canSelectActive = () => !party.isFull;

					_cell.equipRequest = (_card, _mode, _confirm) => _confirm(party.Add(_card, _mode));
					_cell.unequipRequest = (_card, _confirm) => _confirm(party.Remove(_card));

					_cell.onPointerHover = (_card, _mode, _enter) =>
					{
						if (_enter)
						{
							cardDetail.SetCard(_card, _mode);
						}
						else 
						{
							if (DeckCell.lastFocus)
							{
								var _lastCard = DeckCell.lastFocus.card;
								var _lastMode = DeckCell.lastFocus.mode;
								cardDetail.SetCard(_lastCard, _lastMode);
							}
						}
					};

					_cell.Hide();

					var _rect = _cell.gameObject.RectTransform();
					_rect.SetParent(deckTable, false);

					var x = i % CELL_ROW;
					var y = (i / CELL_ROW) / (float)(_count / CELL_ROW) / 1.3f;

					_rect.SetPivotY(1.3f);

					_rect.SetAnchor(new Vector2(
						(x + 0.5f) * CELL_SIZE + x * CELL_MARGIN,
						1 - y));

					mCells.Add(_data, _cell);

					++i;
				}
			}

			Refresh();
		}

		public void Refresh()
		{
			if (mCells.Empty()
			    || mDeck == null
			    || mParty == null)
			{
				return;
			}

			foreach (var _cell in mCells.Values)
				_cell.Hide();

			foreach (var _card in mDeck.cards)
				mCells[_card.Value.data].Show(_card.Value);

			foreach (var _card in mParty.passives)
				mCells[_card.data].Equip(CardMode.PASSIVE);

			foreach (var _card in mParty.actives)
				mCells[_card.data].Equip(CardMode.ACTIVE);
		}
	}
}