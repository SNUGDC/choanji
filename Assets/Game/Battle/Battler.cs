using Gem;

namespace Choanji.Battle
{
	public enum BattlerID { }

	public class Battler
	{
		public Battler(StatSet _baseStat, Party _party)
		{
			baseStat = _baseStat;
			partyStat = _party.CalStat();

			hp = hpMax = (baseStat.hp + (int)partyStat.hp);
			ap = apMax = (baseStat.ap + (int)partyStat.ap);

			party = _party;
		}

		public readonly StatSet baseStat;
		public readonly StatSet partyStat;
		public readonly StatSet dynamicStat = new StatSet();

		public readonly HP hpMax;
		public HP mHP;
		public HP hp { 
			get { return mHP; }
			private set { mHP = value < hpMax ? value : hpMax; } 
		}

		public readonly AP apMax;

		private AP mAP;
		public AP ap
		{
			get { return mAP; }
			private set
			{
				D.Assert(value >= 0);
				mAP = value < apMax ? value : apMax;
			}
		}

		public readonly Party party;

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