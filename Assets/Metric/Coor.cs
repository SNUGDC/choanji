using System;
using Gem;
using UnityEngine;

namespace Choanji
{
	[Serializable]
	public struct Coor
	{
		public static readonly Coor ZERO = new Coor(0, 0);
		public static readonly Coor ONE = new Coor(1, 1);
		public static readonly Coor NULL = new Coor(-94578348, -89348975);
		public static readonly Vector2 OFFSET = new Vector2(0.5f, 0.5f);

		public int x;
		public int y;

		public Coor(int _x, int _y)
		{
			x = _x;
			y = _y;
		}

		public Coor(Point _p)
			: this(_p.x, _p.y)
		{}

		public Coor(Vector2 _v)
		{
			_v = _v - OFFSET;

			x = (int) _v.x;
			y = (int) _v.y;

#if UNITY_EDITOR
			if ((Math.Abs(_v.x - x) > float.Epsilon)
				|| (Math.Abs(_v.y - y) > float.Epsilon))
			{
				L.W(L.M.CONV_NARROW);
			}
#endif
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ")";
		}

		public static Coor Ceiling(Vector2 v)
		{
			return new Coor((v - OFFSET).Ceiling());
		}

		public static Coor Floor(Vector2 v)
		{
			return new Coor((v - OFFSET).Floor());
		}

		#region equality operator
		public static bool operator ==(Coor a, Coor b)
		{
			return (a.x == b.x) && (a.y == b.y);
		}

		public static bool operator !=(Coor a, Coor b)
		{
			return !(a == b);
		}

		public bool Equals(Coor _other)
		{
			return x == _other.x && y == _other.y;
		}

		public override bool Equals(object _obj)
		{
			if (ReferenceEquals(null, _obj)) return false;
			return _obj is Coor && Equals((Coor)_obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (x * 397) ^ y;
			}
		}
		#endregion

		#region arithmetic operator
		public static Coor operator +(Coor a, Coor b)
		{
			return new Coor(a.x + b.x, a.y + b.y);
		}

		public static Coor operator -(Coor _this)
		{
			return new Coor(-_this.x, -_this.y);
		}

		public static Coor operator -(Coor a, Coor b)
		{
			return a + (-b);
		}

		public static Coor operator +(Coor _this, Direction _dir)
		{
			return _this + (Coor) new Point(_dir);
		}
		#endregion

		#region conversion operator
		public static explicit operator Direction(Coor _c)
		{
			return (Direction) (Point) _c;
		}

		public static implicit operator Coor(Vector2 _v)
		{
			return new Coor(_v);
		}

		public static implicit operator Vector2(Coor _c)
		{
			return new Vector2(_c.x + OFFSET.x, _c.y + OFFSET.y);
		}

		public static implicit operator Coor(Point _p)
		{
			return new Coor(_p);
		}

		public static implicit operator Point(Coor _c)
		{
			return new Point(_c.x, _c.y);
		}
		#endregion
	}
}
