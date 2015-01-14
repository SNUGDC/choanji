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
			mInput.Add(InputCode.Y, MakeYesHandler());
			mInput.DecorateDirection(ProcessDir, _dir => ProcessDir(_dir));
			mDelegate = GetComponent<CharacterCtrl>();
		}

		private InputHandler MakeYesHandler()
		{
			return new InputHandler()
			{
				down = () =>
				{
					mDelegate.ProcessInputYes();
					return true;
				}
			};
		}

		private bool ProcessDir(Direction _dir)
		{
			if (enabled) 
				mDelegate.ProcessInput(_dir);
			return true;
		}

	}
}