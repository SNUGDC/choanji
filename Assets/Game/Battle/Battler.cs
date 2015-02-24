using System;
using System.Collections.Generic;
using System.Linq;
using Gem;
using Random = UnityEngine.Random;

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

		public string name { get { return data.name; } }

		public StatSet baseStat { get { return data.stat; } }
		public readonly StatSet partyStat;
		public StatSet dynamicStat = new StatSet();
		public Party party { get { return data.party; } }

		private int mCurTurn;
		public Dictionary<int, StatSet> mTimeoutStat = new Dictionary<int, StatSet>();

		public readonly HP hpMax;
		public HP mHP;
		public HP hp { 
			get { return mHP; }
			private set
			{
				if (mHP == value) return;
				if (value < 0) 
					mHP = 0;
				else if (value > hpMax)
					mHP = hpMax;
				else
					mHP = value;
			} 
		}

		public bool alive { get { return hp > 0; } }

		public readonly AP apMax;

		private AP mAP;
		public AP ap
		{
			get { return mAP; }
			private set
			{
				D.Assert(value >= 0);
				if (mAP == value) return;
				mAP = value < apMax ? value : apMax;
			}
		}

		public SC? sc { get; private set; }
		private readonly HashSet<SC> mSCImmune = new HashSet<SC>();

		public readonly Dictionary<ElementID, int> attackModifier = new Dictionary<ElementID, int>();
		public readonly DamageBuilder attackBuilder = new DamageBuilder();

		public readonly Dictionary<ElementID, int> hitModifier = new Dictionary<ElementID, int>();
		public readonly DamageBuilder hitBuilder = new DamageBuilder();

		public bool blockHitOneTime;
		public Action<Damage> beforeHit;
		public Action<Damage> afterHit;

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

		public void ApplyForDuration(StatSet _stat, int _dur)
		{
			dynamicStat += _stat;

			var _until = mCurTurn + _dur;
			if (mTimeoutStat.ContainsKey(_until))
				mTimeoutStat[_until] += _stat;
			else
				mTimeoutStat[_until] = _stat;
		}

		public HP Hit(Damage _dmg)
		{
			var _def = CalStat(StatType.DEF);
			var _rst = CalRst(_dmg.ele);
			var _factor = (100 / (100 + (float)_def)) 
				* (100 / (100 + (float)_rst));
			var _trueDmg = (HP) ((float) _dmg.val*_factor);
			hp -= _trueDmg;
			return _trueDmg;
		}

		public void Heal(HP _val)
		{
			hp += (int)_val;
		}

		public void Heal(Percent _val)
		{
			hp += (int)(((int) hpMax) * ((int)_val / 100f));
		}

		public AP CalConsumption(AP _val)
		{
			var _mod = CalStat(StatType.AP_CONSUME);
			var _consumption = _val + _mod;
			return _consumption > 0 ? _consumption : 0;
		}

		public AP ChargeAP(AP _val)
		{
			ap += (int)_val;
			return ap;
		}

		public void ConsumeAP(AP _val)
		{
			ap -= CalConsumption(_val);
		}

		public APChangeDigest RegenAP()
		{
			ap += CalStat(StatType.AP_REGEN);
			return new APChangeDigest(new Invoker(this), ap);
		}

		public bool IsImmuned(SC _sc)
		{
			return mSCImmune.Contains(_sc);
		}

		public void AddImmune(SC _sc)
		{
			mSCImmune.Add(_sc);
		}

		public void RemoveImmune(SC _sc)
		{
			mSCImmune.Remove(_sc);
		}

		public bool TryImposeSC(SC _sc)
		{
			if (IsImmuned(_sc))
				return false;
			sc = _sc;
			return true;
		}

		public bool TryImposeSC(SC _sc, Percent _per)
		{
			var _rst = CalStat(StatType.RST_SC);
			var _factor = 100f / (100 + _rst);
			var _factored = (int)_per * _factor;

			if (_factored > Random.Range(0, 100))
				return TryImposeSC(_sc);
			else
				return false;
		}

		public void HealSC()
		{
			sc = null;
		}

		public void AfterTurnEnd()
		{
			while (!mTimeoutStat.Empty())
			{
				var _kv = mTimeoutStat.First();
				if (_kv.Key > mCurTurn) break;
				dynamicStat -= _kv.Value;
				mTimeoutStat.Remove(_kv.Key);
			}

			++mCurTurn;
		}


	}
}