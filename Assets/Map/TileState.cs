using Gem;

namespace Choanji
{
	public class TileState
	{
		public TileState(TileData _data)
		{
			data = _data;
		}

		public bool mOccupied = false;
		public bool occupied
		{
			get { return mOccupied && data.occupied; }
			set
			{
				if (occupied && value)
					L.W(L.M.CALL_RETRY("occupy"));
				mOccupied = value;
			}
		}

		public readonly TileData data;

		public bool IsPassable(Direction _dir)
		{
			return !occupied && data.wall.No(_dir);
		}
	}

}