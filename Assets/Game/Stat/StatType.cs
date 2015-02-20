using System.Collections.Generic;

namespace Choanji
{
	public enum StatType
	{
		HP,
		AP,
		AP_REGEN,
		STR,
		DEF,
		SPD,
	}

	public struct StatRst
	{
		public StatRst(ElementID _elem )
		{
			element = _elem;
		}

		public readonly ElementID element;

		public override int GetHashCode()
		{
			return (int) element;
		}

		public static IEnumerable<StatRst> GetEnum()
		{
			foreach (var _elem in ElementDB.GetEnum())
				yield return new StatRst(_elem);
		}
	}
}