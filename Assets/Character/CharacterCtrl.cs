using Gem;
using UnityEngine;

namespace Choanji
{
	public partial class CharacterCtrl : MonoBehaviour
	{
		public MapData curMap;

		public Coor position
		{
			get
			{
				return new Coor(transform.localPosition);
			}
		}

		private TileState TryGetTileState(Coor _pos)
		{
			D.Assert(curMap != null);
			TileState _state;
			curMap.states.TryGet(_pos, out _state);
			return _state;
		}

		public bool TrySetPosition(Coor _pos)
		{
			var _state = TryGetTileState(_pos);
			if ((_state == null) || _state.occupied) 
				return false;
			SetPosition(_pos, _state);
			return true;
		}

		private void SetPosition(Coor _pos, TileState _state)
		{
			D.Assert(_state != null);
			_state.occupied = true;
			transform.localPosition = (Vector2) _pos;
		}

		public bool TryMove(Direction _dir)
		{
			var _pos = position + new Point(_dir);
			var _state = TryGetTileState(_pos);
			if ((_state == null) || _state.occupied || !_state.IsPassable(_dir)) 
				return false;
			SetPosition(_pos, _state);
			return true;
		}

		void Update()
		{
			UpdateInput();
		}

	}
}