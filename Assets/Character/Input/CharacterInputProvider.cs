using Gem;
using Gem.In;

public class CharacterInputProvider
{
	public CharacterInputProvider(InputManager _input)
	{
		mInput = new InputGroup(_input);

		var _map = new[]
		{
			new { code = InputCode.U, dir = Direction.U },
			new { code = InputCode.D, dir = Direction.D },
			new { code = InputCode.R, dir = Direction.R },
			new { code = InputCode.L, dir = Direction.L },
		};

		foreach (var _bind in _map)
			mInput.Add(_bind.code, DirHandler(_bind.dir));
		mInput.Reg();
	}

	public void Process(Direction _dir)
	{
		L.Log(0, "Process " + _dir);
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

	private readonly InputGroup mInput;
	public ICharacterInputDelegate delegate_;
}