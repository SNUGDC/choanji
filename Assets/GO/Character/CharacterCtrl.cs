using System;
using Gem;
using UnityEngine;

namespace Choanji
{
	public partial class CharacterCtrl : MonoBehaviour
	{
		public Character ch { get; private set; }

		public SetBool<CharacterMoveBlock> blockMove 
			= new SetBool<CharacterMoveBlock>();

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
				TrySetPosition(ch.position);
			}
		}

		public ActionWrap<Coor, TileState> doTileRetain
			= new ActionWrap<Coor, TileState>();

		void Awake()
		{
			ch = GetComponent<Character>();
		}

		private TileState TryGetTileState(Coor _pos)
		{
			if (curMap == null)
			{
				L.E(L.M.SHOULD_NOT_NULL("curMap"));
				return null;
			}

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

		private void BeforeSetPosition(Coor _pos, TileState _state)
		{
			D.Assert(_state != null);
			mOccupy.Retain(_pos);
			if (doTileRetain.val != null) 
				doTileRetain.val(_pos, _state);
		}

		private void SetPosition(Coor _pos, TileState _state)
		{
			BeforeSetPosition(_pos, _state);
			ch.position = _pos;
		}

		public bool TryMove(Direction _dir)
		{
			if (!ch.CanMove(_dir))
				return false;

			if (blockMove)
				return false;

			var _curTile = TryGetTileState(ch.position);
			if ((_curTile != null) && !_curTile.IsHole(_dir))
				return false;

			var _pos = ch.position + _dir;
			var _state = TryGetTileState(_pos);
			if ((_state == null) || _state.occupied || !_state.IsHole(_dir.Neg())) 
				return false;

			BeforeSetPosition(_pos, _state);
			ch.Move(_dir);
			
			return true;
		}

		public bool TryTurn(Direction _dir)
		{
			if (blockMove)
				return false;
			ch.direction = _dir;
			return true;
		}

		void Update()
		{
			UpdateInput();
		}

	}
}