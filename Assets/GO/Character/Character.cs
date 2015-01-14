using Gem;
using UnityEngine;

namespace Choanji
{
	public class Character : MonoBehaviour
	{
		public Coor position
		{
			get
			{
				return new Coor(transform.localPosition);
			}

			set
			{
				transform.localPosition = (Vector2) value;
			}
		}

		private Direction mDirection = Direction.D;
		public Direction direction {
			get { return mDirection; }
			private set { mDirection = value; } 
		}

		public Coor front
		{
			get { return position + direction; }
		}

		public GameObject renderer;

		public void LookAt(Direction _dir)
		{
			renderer.transform.SetEulerZ(_dir.ToDeg());
		}

		public bool CanMove(Direction _dir)
		{
			return true;
		}

		public void Move(Direction _dir)
		{
			D.Assert(CanMove(_dir));
			direction = _dir;
			position += _dir;
			LookAt(_dir);
		}
	}

}