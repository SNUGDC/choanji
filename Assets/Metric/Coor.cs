using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Unit = PreciseFloat;

public struct Coor
{
	public Unit x;
	public Unit y;

	public static explicit operator Vector2(Coor _coor)
	{
		return new Vector2(UnitHelper.METER * _coor.x, UnitHelper.METER * _coor.y);
	}

	public static Vector2 up = new Vector2(0, UnitHelper.METER);
	public static Vector2 right = new Vector2(UnitHelper.METER, 0);
}
