namespace Choanji
{
	public struct WorldCoor
	{
		public WorldCoor(Coor _val)
		{
			mVal = _val;
		}

		private readonly Coor mVal;

		public static implicit operator Coor(WorldCoor _this)
		{
			return _this.mVal;
		}

		#region equality operator
		public static bool operator ==(WorldCoor a, WorldCoor b)
		{
			return a.mVal == b.mVal;
		}

		public static bool operator !=(WorldCoor a, WorldCoor b)
		{
			return !(a == b);
		}

		public bool Equals(WorldCoor _other)
		{
			return mVal.Equals(_other.mVal);
		}

		public override bool Equals(object _obj)
		{
			if (ReferenceEquals(null, _obj)) return false;
			return _obj is WorldCoor && Equals((WorldCoor)_obj);
		}

		public override int GetHashCode()
		{
			return mVal.GetHashCode();
		}
		#endregion

	}
}