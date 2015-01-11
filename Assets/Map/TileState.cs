using Gem;

namespace Choanji
{
	public class TileState
	{
		public TileState(TileData _data)
		{
			data = _data;
		}

		public int mOccupied = 0;
		public bool occupied
		{
			get { return (mOccupied > 0) || data.occupied; }
		}

		public void Occupy()
		{
			++mOccupied;
		}

		public void Unoccupy()
		{
			if (--mOccupied < 0)
				L.E(L.M.SHOULD_POS("occupied", mOccupied));
		}

		public bool IsHole(Direction _dir)
		{
			return data.wall.No(_dir);
		}

		public readonly TileData data;
	}

}