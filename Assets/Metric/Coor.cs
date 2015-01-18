using System;
using Gem;
using UnityEngine;

namespace Choanji
{
	public struct Coor
	{
		private const float OFFSET = 0.5f;

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
			_v.x = _v.x - OFFSET;
			_v.y = _v.y - OFFSET;

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
			return new Vector2(_c.x + OFFSET, _c.y + OFFSET);
		}

		public static implicit operator Coor(Point _p)
		{
			return new Coor(_p);
		}

		public static implicit operator Point(Coor _c)
		{
			return new Point(_c.x, _c.y);
		}
	}
}
