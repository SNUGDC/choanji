using System;
using Gem;

namespace Choanji
{
	[Serializable]
	public class MapMeta
	{
		public MapMeta(string _name, Point _size)
		{
			id = MapIDHelper.Make(_name);
			name = _name;
			size = _size;
		}

		public readonly MapID id;
		public readonly string name;
		public readonly Point size;

		public static implicit operator MapID(MapMeta _this) { return _this.id; }

		public override int GetHashCode()
		{
			return (int) id;
		}
	}

}