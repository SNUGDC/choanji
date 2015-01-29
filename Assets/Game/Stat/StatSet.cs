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
		public StatSet(JsonData _json)
		{
			str = _json.IntOrDefault(StatType.STR.ToString());
			def = _json.IntOrDefault(StatType.DEF.ToString());
			spd = _json.IntOrDefault(StatType.SPD.ToString());

			JsonData _rstData;
			if (_json.TryGet(StatType.RST.ToString(), out _rstData))
			{
				mRst = new Rst();
				foreach (var _elemRst in _rstData.GetDictEnum())
				{
					var _elemData = ElementDB.Search(_elemRst.Key);
					mRst.Add(_elemData.id, (int)_elemRst.Value);
				}
			}
		}

		public int str;
		public int def;
		public int spd;

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