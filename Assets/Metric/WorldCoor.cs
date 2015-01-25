namespace Choanji
{
	public struct WorldCoor
	{
		public WorldCoor(Coor _val)
		{
			val = _val;
		}

		public Coor val;

		#region equality operator
		public static bool operator ==(WorldCoor a, WorldCoor b)
		{
			return a.val == b.val;
		}

		public static bool operator !=(WorldCoor a, WorldCoor b)
		{
			return !(a == b);
		}

		public bool Equals(WorldCoor _other)
		{
			return val.Equals(_other.val);
		}

		public override bool Equals(object _obj)
		{
			if (ReferenceEquals(null, _obj)) return false;
			return _obj is WorldCoor && Equals((WorldCoor)_obj);
		}

		public override int GetHashCode()
		{
			return val.GetHashCode();
		}
		#endregion

	}
}