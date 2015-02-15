using Gem;

namespace Choanji
{
	public enum CharacterAnimKey
	{
		UP, DOWN, LEFT, RIGHT,
	}

	public static class CharacterAnimHelper
	{
		public static CharacterAnimKey ToKey(Direction _dir)
		{
			switch (_dir)
			{
				case Direction.L: 
					return CharacterAnimKey.LEFT;
				case Direction.R:
					return CharacterAnimKey.RIGHT;
				case Direction.U:
					return CharacterAnimKey.UP;
				case Direction.D:
					return CharacterAnimKey.DOWN;
				default:
					D.Assert(false);
					return CharacterAnimKey.DOWN;
			}
		}
	}

}