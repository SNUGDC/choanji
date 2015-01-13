using Gem;
using Gem.In;
using UnityEngine;

namespace Choanji
{
	public class CharacterInputAgent : MonoBehaviour
	{
		private InputGroup mInput;
		private ICharacterInputDelegate mDelegate;

		void Start()
		{
			mInput = new InputGroup(InputManager.g);
			foreach (var _dir in EnumHelper.GetValues<Direction>())
				mInput.Add(_dir.ToInputCode(), DirHandler(_dir));
			mInput.Reg();

			mDelegate = GetComponent<CharacterCtrl>();
		}

		public void Process(Direction _dir)
		{
			if (!enabled) return;
			mDelegate.ProcessInput(_dir);
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