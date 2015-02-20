﻿using System;
using Gem;

namespace Choanji.Battle
{
	public enum BattlerID { }

	public class Battler
	{
		public Battler(BattlerData _data)
		{
			data = _data;
			partyStat = party.CalStat();
			hp = hpMax = (baseStat.hp + (int)partyStat.hp);
			ap = apMax = (baseStat.ap + (int)partyStat.ap);
		}

		public readonly BattlerData data;

		public StatSet baseStat { get { return data.stat; } }
		public readonly StatSet partyStat;
		public readonly StatSet dynamicStat = new StatSet();
		public Party party { get { return data.party; } }

		public readonly HP hpMax;
		public HP mHP;
		public HP hp { 
			get { return mHP; }
			private set
			{
				if (mHP == value) return;

				var _old = mHP;

				if (value < 0) 
					mHP = 0;
				else if (value > hpMax)
					mHP = hpMax;
				else
					mHP = value;

				onHPMod.CheckAndCall(mHP, _old);
			} 
		}

		public readonly AP apMax;

		private AP mAP;
		public AP ap
		{
			get { return mAP; }
			private set
			{
				D.Assert(value >= 0);
				if (mAP == value) return;
				var _old = mAP;
				mAP = value < apMax ? value : apMax;
				onAPMod.CheckAndCall(mAP, _old);
			}
		}

		public Action<HP, HP> onHPMod;
		public Action<AP, AP> onAPMod;

		public int CalStat(StatType _type)
		{
			return baseStat[_type] + partyStat[_type] + dynamicStat[_type];
		}

		public RST CalRst(ElementID _ele)
		{
			return baseStat.GetRst(_ele) 
				+ (int)partyStat.GetRst(_ele) 
				+ (int)dynamicStat.GetRst(_ele);
		}

		public StatSet CalStat()
		{
			return baseStat + partyStat + dynamicStat;
		}

		public HP Hit(Damage _dmg)
		{
			var _rst = CalRst(_dmg.ele);
			var _factor = (100/(100 + (float) _rst));
			var _trueDmg = (HP) ((float) _dmg.val*_factor);
			hp -= _trueDmg;
			return _trueDmg;
		}

		public void ConsumeAP(AP _val)
		{
			ap -= _val;
		}

		public void RegenAP()
		{
			ap += CalStat(StatType.AP_REGEN);
		}

	}
}