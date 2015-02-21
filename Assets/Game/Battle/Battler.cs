using System;
using System.Collections.Generic;
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

			foreach (var _ele in ElementDB.GetEnum())
			{
				attackModifier.Add(_ele, 0);
				hitModifier.Add(_ele, 0);
			}

			attackBuilder.Add(_org =>
			{
				int _mod;
				if (!attackModifier.TryGetValue(_org.ele, out _mod))
					return _org;
				_org.val = (HP)((float)_org.val * ((100 + _mod) / 100f));
				return _org;
			});

			hitBuilder.Add(_org =>
			{
				int _mod;
				if (!hitModifier.TryGetValue(_org.ele, out _mod))
					return _org;
				_org.val = (HP)((float)_org.val * (100f / (100 + _mod)));
				return _org;
			});
		}

		public readonly BattlerData data;

		public StatSet baseStat { get { return data.stat; } }
		public readonly StatSet partyStat;
		public StatSet dynamicStat = new StatSet();
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

		public readonly Dictionary<ElementID, int> attackModifier = new Dictionary<ElementID, int>();
		public readonly DamageBuilder attackBuilder = new DamageBuilder();

		public readonly Dictionary<ElementID, int> hitModifier = new Dictionary<ElementID, int>();
		public readonly DamageBuilder hitBuilder = new DamageBuilder();

		public bool blockHitOneTime;
		public Action<Damage> beforeHit;

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

		public AP CalConsumption(AP _val)
		{
			var _mod = CalStat(StatType.AP_CONSUME);
			var _consumption = _val + _mod;
			return _consumption > 0 ? _consumption : 0;
		}

		public void ConsumeAP(AP _val)
		{
			ap -= CalConsumption(_val);
		}

		public void RegenAP()
		{
			ap += CalStat(StatType.AP_REGEN);
		}

	}
}