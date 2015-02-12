using Gem;
using UnityEngine;

namespace Choanji
{
	public class Character : MonoBehaviour
	{
		public Coor lastPosition { get; private set; }
		public Coor position
		{
			get
			{
				return new Coor(transform.position);
			}

			set
			{
				var _orgPos = position;
				if (_orgPos == lastPosition) return;
				lastPosition = _orgPos;
				transform.position = (Vector2)value;
			}
		}

		private Direction mDirection = Direction.D;
		public Direction direction {
			get { return mDirection; }
			set
			{
				if (direction == value)
					return;
				mDirection = value;
				renderer_.transform.SetLEulerZ(mDirection.ToDeg());
			} 
		}

		public Coor front
		{
			get { return position + direction; }
		}

		public GameObject renderer_;

		public bool CanMove(Direction _dir)
		{
			return true;
		}

		public void Move(Direction _dir)
		{
			D.Assert(CanMove(_dir));
			direction = _dir;
			position += _dir;
		}
	}

}