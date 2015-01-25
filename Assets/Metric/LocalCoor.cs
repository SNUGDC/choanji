using Gem;

namespace Choanji
{
	public struct LocalCoor
	{
		public LocalCoor(Map _map, Coor _val)
		{
			D.Assert(_map != null);

#if UNITY_EDITOR
			if (!_map.size.Outer(_val))
				L.W(string.Format("Coor is out of bound. Coor: {0}, Size: {1}", 
					_val, _map.size));
#endif

			map = _map;
			val = _val;
		}

		public readonly Map map;
		public Coor val;

		public TileState FindState()
		{
			TileState _state;
			map.dynamic.grid.TryGet(val, out _state);
			return _state;
		}

		public static explicit operator WorldCoor(LocalCoor _this)
		{
			return new WorldCoor(new Coor(_this.map.go.transform.position) + _this.val);
		}

		#region equality operator

		public static bool operator ==(LocalCoor a, LocalCoor b)
		{
			return (a.map == b.map)
				&& (a.val == b.val);
		}

		public static bool operator !=(LocalCoor a, LocalCoor b)
		{
			return !(a == b);
		}

		public bool Equals(LocalCoor other)
		{
			return Equals(map, other.map) && val.Equals(other.val);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is LocalCoor && Equals((LocalCoor) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((map != null ? map.GetHashCode() : 0)*397) ^ val.GetHashCode();
			}
		}

		#endregion
	}
}