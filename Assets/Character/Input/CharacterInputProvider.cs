using Gem;
using Gem.In;

namespace Choanji
{
	public class CharacterInputProvider
	{
		private readonly InputGroup mInput;
		public ICharacterInputDelegate delegate_;

		public CharacterInputProvider(InputManager _input)
		{
			mInput = new InputGroup(_input);
			foreach (var _dir in EnumHelper.GetValues<Direction>())
				mInput.Add(_dir.ToInputCode(), DirHandler(_dir));
			mInput.Reg();
		}

		public void Process(Direction _dir)
		{
			L.D("process " + _dir);
			delegate_.ProcessInput(_dir);
		}

		public InputHandler DirHandler(Direction _dir)
		{
			return new InputHandler
			{
				down = delegate
				{
					Process(_dir);
					return true;
				},

				listen = true,

				update = () => Process(_dir)
			};
		}
	}
}