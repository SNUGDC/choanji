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

		private IInspectable mInspectable;
		public IInspectable inspectable
		{
			get
			{
				if (mInspectable != null)
					return mInspectable;

				var _inspData = data.GetInspectableData();
				if (_inspData == null)
					return null;

				mInspectable = InspectorFactory.Make(_inspData);
				return mInspectable; 
			}

			set
			{
				if (inspectable == value)
					return;

				if ((mInspectable != null) && (value != null))
					L.E(L.DO.REPLACE("inspectable"), L.M.CALL_RETRY("set inspectable"));
				
				mInspectable = value;
			}
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