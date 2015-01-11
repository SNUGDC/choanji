using Gem;
using UnityEngine;

namespace Choanji
{
	public partial class CharacterCtrl : MonoBehaviour
	{
		private MapData mCurMap;
		private TileOccupy mOccupy;

		[HideInInspector]
		public MapData curMap
		{
			get { return mCurMap; }

			set
			{
				if (curMap == value)
					return;
				mCurMap = value;
				mOccupy = new TileOccupy(mCurMap.states);
			}
		}

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
			mOccupy.Occupy(_pos);
			transform.localPosition = (Vector2) _pos;
		}

		public bool TryMove(Direction _dir)
		{
			var _curTile = TryGetTileState(position);
			if ((_curTile != null) && !_curTile.IsHole(_dir))
				return false;

			var _pos = position + new Point(_dir);
			var _tile = TryGetTileState(_pos);
			if ((_tile == null) || _tile.occupied || !_tile.IsHole(_dir.Neg())) 
				return false;

			SetPosition(_pos, _tile);
			return true;
		}

		void Update()
		{
			UpdateInput();
		}

	}
}