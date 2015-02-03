using System.Collections.Generic;
using System.Linq;
using Gem;
using LitJson;

namespace Choanji
{
	using Rst = Dictionary<ElementID, int>;
	using ElemAndValue = KeyValuePair<ElementID, int>;

	public class StatSet
	{
		public StatSet()
		{}

		public StatSet(JsonData _json)
		{
			str = _json.IntOrDefault(StatType.STR.ToString());
			def = _json.IntOrDefault(StatType.DEF.ToString());
			spd = _json.IntOrDefault(StatType.SPD.ToString());

			JsonData _rstData;
			if (_json.TryGet("RST", out _rstData))
			{
				mRst = new Rst();
				foreach (var _elemRst in _rstData.GetDictEnum())
				{
					var _elemData = ElementDB.Search(_elemRst.Key);
					mRst.Add(_elemData, (int)_elemRst.Value);
				}
			}
		}

		public int str;
		public int def;
		public int spd;

		public int this[StatType _stat]
		{
			get
			{
				switch (_stat)
				{
					case StatType.STR:
						return str;
					case StatType.DEF:
						return def;
					case StatType.SPD:
						return spd;
					default:
						L.E(L.M.CASE_INVALID(_stat));
						return 0;
				}
			}

			set
			{
				switch (_stat)
				{
					case StatType.STR:
						str = value;
						return;
					case StatType.DEF:
						def = value;
						return;
					case StatType.SPD:
						spd = value;
						return;
					default:
						L.E(L.M.CASE_INVALID(_stat));
						return;
				}
			}
		}

		private Rst mRst;

		public int GetRst(ElementID _elem)
		{
			return mRst != null ? mRst.GetOrDefault(_elem) : 0;
		}

		public void SetRst(ElementID _elem, int _val)
		{
			if (mRst == null) 
				mRst = new Rst { { _elem, _val} };
			else
				mRst[_elem] = _val;
		}

		public IEnumerable<ElemAndValue> GetRstEnum()
		{
			return mRst ?? Enumerable.Empty<ElemAndValue>();
		}

		public static StatSet operator +(StatSet a, StatSet b)
		{
			var _ret = new StatSet();

			foreach (var _stat in EnumHelper.GetValues<StatType>())
				_ret[_stat] = a[_stat] + b[_stat];

			foreach (var _elem in ElementDB.GetEnum())
			{
				var _rst = a.GetRst(_elem) + b.GetRst(_elem);
				if (_rst != 0) _ret.SetRst(_elem, _rst);
			}

			return _ret;
		}
	}

	public class ReadOnlyStatSet
	{
		public ReadOnlyStatSet(StatSet _statSet)
		{
			mStatSet = _statSet;
		}

		public int str { get { return mStatSet.str; } }
		public int def { get { return mStatSet.def; } }
		public int spd { get { return mStatSet.spd; } }

		public int GetRst(ElementID _elemID)
		{
			return mStatSet.GetRst(_elemID);
		}

		public IEnumerable<ElemAndValue> GetRstEnum()
		{
			return mStatSet.GetRstEnum();
		}

		private readonly StatSet mStatSet;
	}
}