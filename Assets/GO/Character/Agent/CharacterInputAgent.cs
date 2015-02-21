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
			mInput = new InputGroup(TheInput.world);
			mInput.Add(MakeYesBind());
			mInput.DecorateDirection(ProcessDir, _dir => ProcessDir(_dir));
			mDelegate = GetComponent<CharacterCtrl>();
		}

		private InputBind MakeYesBind()
		{
			return new InputBind(InputCode.Y, new InputHandler()
			{
				down = () =>
				{
					mDelegate.ProcessInputYes();
					return true;
				}
			});
		}

		private bool ProcessDir(Direction _dir)
		{
			if (enabled) 
				mDelegate.ProcessInput(_dir);
			return true;
		}

	}
}