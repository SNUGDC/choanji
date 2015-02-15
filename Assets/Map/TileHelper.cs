using UnityEngine;

namespace Choanji
{
	public static class TileHelper
	{
		public static LocalCoor GetLocalCoor(Transform _trans)
		{
			var _coor = new Coor(((Vector2)_trans.position) + Coor.OFFSET);
			var _roomComp = _trans.parent.parent.gameObject.GetComponent<WorldRoomComp>();
			return _roomComp.map.Convert(new WorldCoor(_coor));
		}
	}

}