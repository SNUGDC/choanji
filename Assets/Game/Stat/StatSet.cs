using System.Collections.Generic;
using System.Linq;
using Gem;
using LitJson;

namespace Choanji
{
	using Rsts = Dictionary<ElementID, RST>;
	using Rst = KeyValuePair<ElementID, RST>;

	public class StatSet
	{
		public StatSet()
		{}

		public StatSet(JsonData _json)
		{
			hp = (HP)_json.IntOrDefault(StatType.HP.ToString());
			ap = (AP)_json.IntOrDefault(StatType.AP.ToString());
			apRegen = (AP)_json.IntOrDefault(StatType.AP_REGEN.ToString());
			apConsume = (AP)_json.IntOrDefault(StatType.AP_CONSUME.ToString());
			str = (STR)_json.IntOrDefault(StatType.STR.ToString());
			def = (DEF)_json.IntOrDefault(StatType.DEF.ToString());
			spd = (SPD)_json.IntOrDefault(StatType.SPD.ToString());

			JsonData _rstData;
			if (_json.TryGet("RST", out _rstData))
			{
				mRst = new Rsts();
				foreach (var _elemRst in _rstData.GetDictEnum())
				{
					var _elemData = ElementDB.Search(_elemRst.Key);
					mRst.Add(_elemData, (RST)(int)_elemRst.Value);
				}
			}
		}

		public HP hp;
		public AP ap;
		public AP apRegen;
		public AP apConsume;
		public STR str;
		public DEF def;
		public SPD spd;

		public int this[StatType _stat]
		{
			get
			{
				switch (_stat)
				{
					case StatType.HP:  return (int)hp;
					case StatType.AP:  return (int)ap;
					case StatType.AP_REGEN: return (int)apRegen;
					case StatType.AP_CONSUME: return (int)apConsume;
					case StatType.STR: return (int)str;
					case StatType.DEF: return (int)def;
					case StatType.SPD: return (int)spd;
					default:
						L.E(L.M.CASE_INVALID(_stat));
						return 0;
				}
			}

			set
			{
				switch (_stat)
				{
					case StatType.HP:  hp = (HP)value; return;
					case StatType.AP:  ap = (AP)value; return;
					case StatType.AP_REGEN: apRegen = (AP)value; return;
					case StatType.AP_CONSUME: apConsume = (AP)value; return;
					case StatType.STR: str = (STR)value; return;
					case StatType.DEF: def = (DEF)value; return;
					case StatType.SPD: spd = (SPD)value; return;
					default:
						L.E(L.M.CASE_INVALID(_stat));
						return;
				}
			}
		}

		private Rsts mRst;

		public RST GetRst(ElementID _elem)
		{
			return mRst != null ? mRst.GetOrDefault(_elem) : 0;
		}

		public void SetRst(ElementID _elem, RST _val)
		{
			if (mRst == null) 
				mRst = new Rsts { { _elem, _val} };
			else
				mRst[_elem] = _val;
		}

		public IEnumerable<Rst> GetRstEnum()
		{
			return mRst ?? Enumerable.Empty<Rst>();
		}

		public static StatSet operator -(StatSet _this)
		{
			var _ret = new StatSet();

			foreach (var _stat in EnumHelper.GetValues<StatType>())
				_ret[_stat] = -_this[_stat];

			foreach (var _ele in ElementDB.GetEnum())
			{
				var _rst = (RST)(-(int)_this.GetRst(_ele));
				if (_rst != 0) _ret.SetRst(_ele, _rst);
			}

			return _ret;
		}

		public static StatSet operator +(StatSet a, StatSet b)
		{
			var _ret = new StatSet();

			foreach (var _stat in EnumHelper.GetValues<StatType>())
				_ret[_stat] = a[_stat] + b[_stat];

			foreach (var _ele in ElementDB.GetEnum())
			{
				var _rst = (RST)((int)a.GetRst(_ele) + (int)b.GetRst(_ele));
				if (_rst != 0) _ret.SetRst(_ele, _rst);
			}

			return _ret;
		}

		public static StatSet operator -(StatSet a, StatSet b)
		{
			return a + (-b);
		}
	}
}