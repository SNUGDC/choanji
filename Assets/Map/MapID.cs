namespace Choanji
{
	public enum MapID {}

	public enum WorldID {}

	public static class MapIDHelper
	{
		public static MapID Make(string _name)
		{
			return (MapID) _name.GetHashCode();
		}

	}
}