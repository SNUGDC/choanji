using System.Collections.Generic;
using System.Linq;
using Gem;

namespace Choanji
{
	class WorldBluePrint
	{
		public class Room
		{
			public Point pos;
			public MapMeta meta;
		}

		private readonly List<Room> mRooms = new List<Room>();
		private readonly PRectGroup mRectGroup = new PRectGroup();

		public void Add(Room _room)
		{
			D.Assert(_room.meta != null);
			mRooms.Add(_room);
			mRectGroup.Add(new PRect { org = _room.pos, size = _room.meta.size });
		}

		public List<Room> Overlaps(PRect _rect)
		{
			return mRectGroup.Overlaps(_rect).Select(_idx => mRooms[_idx]).ToList();
		}

	}
}