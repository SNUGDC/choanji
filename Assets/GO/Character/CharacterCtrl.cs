using Gem;
using UnityEngine;

namespace Choanji
{
	public partial class CharacterCtrl : MonoBehaviour
	{
		public Character ch { get; private set; }

		public SetBool<CharacterMoveBlock> blockMove 
			= new SetBool<CharacterMoveBlock>();

		private readonly TileOccupy mOccupy = new TileOccupy();

		public ActionWrap<LocalCoor> doTileRetain
			= new ActionWrap<LocalCoor>();

		void Awake()
		{
			ch = GetComponent<Character>();
		}

		public bool TrySetPosition(LocalCoor p)
		{
			var _state = p.FindState();
			if ((_state == null) || _state.occupied) 
				return false;
			SetPosition(p);
			return true;
		}

		private void BeforeSetPosition(LocalCoor p)
		{
			mOccupy.Retain(p);
			if (doTileRetain.val != null)
				doTileRetain.val(p);
		}

		private void SetPosition(LocalCoor p)
		{
			BeforeSetPosition(p);
			ch.position = (WorldCoor) p;
		}

		public bool TryMove(Direction _dir)
		{
			if (!ch.CanMove(_dir))
				return false;

			if (blockMove)
				return false;

			var _world = TheWorld.g;

			var _hasCur = _world.SearchMapAndTile(new WorldCoor(ch.position));
			if (_hasCur == null)
				return false;
			var _cur = _hasCur.Value.FindState();
			if (_cur == null || !_cur.IsHole(_dir))
				return false;

			var _pos = new WorldCoor(ch.position + _dir);
			var _hasMove = _world.SearchMapAndTile(_pos);
			if (_hasMove == null)
				return false;
			var _move = _hasMove.Value.FindState();
			if (_move == null || _move.occupied || !_move.IsHole(_dir.Neg()))
				return false;

			var _map = _hasMove.Value.map;
			BeforeSetPosition(_map.Convert(_pos));
			ch.Move(_dir);

			if (_move.data.door != null)
			{
				if (onEnterDoor != null)
					onEnterDoor(_move.data.door);
			}

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