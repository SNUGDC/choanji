namespace Choanji
{
	public enum StatType
	{
		STR,
		DEF,
		SPD,
		RST,
	}

	public enum StatRstID {}

	public struct Stat
	{
		public StatType type;
		public int sub1;
	}
}