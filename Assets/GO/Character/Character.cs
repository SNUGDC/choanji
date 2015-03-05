using Gem;
using UnityEngine;

namespace Choanji
{
	public class Character : MonoBehaviour
	{
		public enum MoveType 
		{
			WALK, RUN, 
		}

		public const float WALK_TIME = 0.15f;
		public const float RUN_TIME = 0.07f;

		public Coor lastPosition { get; private set; }
		private Coor mPosition;
		public Coor position
		{
			get { return mPosition; }
		}

		private Direction mDirection = Direction.D;
		public Direction direction {
			get { return mDirection; }
			set
			{
				if (direction == value)
					return;
				mDirection = value;
				renderer_.LookAt(value);
			} 
		}

		public Coor front
		{
			get { return position + direction; }
		}

		public CharacterRenderer renderer_;

		public void Update()
		{
			if (isMoving)
			{
				mMoveElapsed += Time.deltaTime;
				if (mMoveElapsed > mMoveTime)
				{
					var _pos = (Vector3)(Vector2)position;
					_pos.z = _pos.y * 0.01f;
					transform.position = _pos;
					StopMove();
				}
				else
				{
					var _delta = direction.ToVector2() * (mMoveElapsed / mMoveTime);
					var _pos = (Vector3)((Vector2)mMoveBase + _delta);
					_pos.z = _pos.y * 0.01f;
					transform.position = _pos;	
				}
			}
		}

		public void SetPosition(Coor _val, bool _transform)
		{
			lastPosition = position;
			if (lastPosition == _val) return;

			if (isMoving)
				StopMove();

			mPosition = _val;

			if (_transform)
			{
				var _pos = (Vector3)(Vector2)_val;
				_pos.z = _val.y * 0.01f;
				transform.position = _pos;
			}
		}

		public bool isMoving { get { return (mMoveTime > 0) && (mMoveElapsed < mMoveTime); } }
		private float mMoveTime;
		private float mMoveElapsed;
		private Coor mMoveBase;

		public bool CanMove(Direction _dir)
		{
			return !isMoving;
		}

		public void Move(Direction _dir, MoveType _move)
		{
			D.Assert(CanMove(_dir));

			var _posOld = position;
			direction = _dir;
			SetPosition(position + _dir, false);
			mMoveBase = _posOld;

			switch (_move)
			{
				case MoveType.WALK:
					mMoveTime = WALK_TIME;
					break;
				case MoveType.RUN:
					mMoveTime = RUN_TIME;
					break;
			}

			renderer_.Walk(_dir);
		}

		public void StopMove()
		{
			mMoveTime = 0;
			mMoveElapsed = 0;
			mMoveBase = default(Coor);
		}
	}
}