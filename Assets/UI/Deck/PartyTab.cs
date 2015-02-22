﻿using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.UI
{
	public class PartyTab : MonoBehaviour
	{
		public const float CARD_MARGIN = 0.05f;
		public const float CARD_WIDTH = (1 - CARD_MARGIN * 2) / 3;

		public Text hp;
		public Text ap;
		public Text strAndDef;
		public Text spd;

		public RectTransform party1;
		public RectTransform party2;
		private readonly List<PartyCell> mCells = new List<PartyCell>(Const.PARTY_MAX);

		public Action<Card> onRemove;

		public StatSet stat 
		{
			set
			{
				hp.text = "HP: " + value.hp;
				ap.text = string.Format("AP: {0}<size=16>(+{1})</size>",value.ap, value.apRegen);
				strAndDef.text = string.Format("공/방: <color=#ff0000>{0}</color>/<color=#0000ff>{1}</color>", value.str, value.def);
				spd.text = "속: " + value.spd;
			}
		}

		public Party party
		{
			set
			{
				if (mCells.Empty())
				{
					for (var i = 0; i != Const.PARTY_MAX; ++i)
					{
						var _cell = DB.g.partyCellPrf.Instantiate();
						var _trans = _cell.gameObject.transform;
						_trans.SetParent(i < Const.PARTY_MAX / 2 ? party1 : party2, false);

						var _pos = i%3;

						var _rect = (RectTransform) _trans;
						_rect.anchorMin = new Vector2(_pos * (CARD_WIDTH + CARD_MARGIN), 0);
						_rect.anchorMax = new Vector2((_pos + 1) * CARD_WIDTH + _pos * CARD_MARGIN, 1);
						_rect.offsetMin = Vector2.zero;
						_rect.offsetMax = Vector2.zero;

						mCells.Add(_cell);
					}
				}

				var _idx = 0;

				foreach (var _card in value.actives)
				{
					var _cell = mCells[_idx];
					_cell.card = _card;
					_cell.onCancel = () => { onRemove.CheckAndCall(_card); };
					++_idx;
				}
				
				foreach (var _card in value.passives)
				{
					var _cell = mCells[_idx];
					_cell.card = _card;
					_cell.onCancel = () => { onRemove.CheckAndCall(_card); };
					++_idx;
				}

				for (; _idx < Const.PARTY_MAX; ++_idx)
				{
					mCells[_idx].card = null;
				}
			}
		}
	}
}