namespace Choanji
{
	public enum StatType
	{
		STR,
		DEF,
		SPD,
		RST,
	}

	public struct StatKey
	{
		public StatKey(StatType _type, int _sub1 = 0)
		{
			type = _type;
			sub1 = _sub1;
		}

		public readonly StatType type;
		public readonly int sub1;

		public override int GetHashCode()
		{
			unchecked
			{
				return 347 * (int) type + 123 * sub1;
			}
		}
	}
}