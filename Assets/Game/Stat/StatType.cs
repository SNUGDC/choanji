using System.Collections.Generic;

namespace Choanji
{
	public enum StatType
	{
		HP,
		AP,
		AP_CONSUME,

		STR,
		DEF,
		SPD,

		RST_SC,
	}

	public struct StatRst
	{
		public StatRst(ElementID _ele )
		{
			element = _ele;
		}

		public readonly ElementID element;

		public override int GetHashCode()
		{
			return (int) element;
		}

		public static IEnumerable<StatRst> GetEnum()
		{
			foreach (var _ele in ElementDB.GetEnum())
				yield return new StatRst(_ele);
		}
	}

	public static class StatHelper
	{
		public static string Name(this StatType _stat)
		{
			switch (_stat)
			{
				case StatType.HP: return "HP";
				case StatType.AP: return "AP";
				case StatType.AP_CONSUME: return "AP 소모량";
				case StatType.STR: return "공격력";
				case StatType.DEF: return "방어력";
				case StatType.SPD: return "속";
				case StatType.RST_SC: return "상태이상 저항력";
				default: return "UNDEFINED_STAT_" + _stat;
			}
		}

		public static string GetRstName(ElementData _ele)
		{
			return _ele.name + " 저항력";
		}
	}
}