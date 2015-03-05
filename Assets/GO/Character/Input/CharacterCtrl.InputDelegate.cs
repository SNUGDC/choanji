using Gem;

namespace Choanji
{

	public partial class CharacterCtrl : ICharacterInputDelegate
	{
		private void UpdateInput()
		{}

		public void ProcessInputYes()
		{
			if (!isInspecting)
				Inspect();
		}

		public void ProcessInput(Direction _dir)
		{
			if (!ch.CanMove(_dir))
				return;

			if (!TryMove(_dir, Character.MoveType.WALK))
				TryTurn(_dir);
		}
	}
}