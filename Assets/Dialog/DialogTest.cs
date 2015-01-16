using Gem;
using Gem.In;
using UnityEngine;

namespace Choanji
{
	public class DialogTest : MonoBehaviour
	{
		public Dialog dialog;
		public string orgText;

		private bool mRebasabe;
		private InputHandler mInputHandler;

		void Start()
		{
			RegInput();
			dialog.orgText = orgText;
			dialog.events.onStrollDone += OnStrollDone;
		}

		void OnDestroy()
		{
			UnregInput();
			if (dialog) dialog.events.onStrollDone -= OnStrollDone;
		}

		void RegInput()
		{
			if (mInputHandler != null)
			{
				L.W(L.M.CALL_RETRY("reg input"));
				return;
			}

			mInputHandler = new InputHandler
			{
				down = delegate
				{
					if (mRebasabe)
						Rebase();
					return true;
				}
			};

			InputManager.g.Reg(InputCode.Y, mInputHandler);

			InputManager.g.mask.On((InputMaskKey)GetHashCode(), new []
			{
				InputCode.U, InputCode.D, InputCode.L, InputCode.R
			});

			InputManager.g.DebugLock(true, GetHashCode());
		}

		void UnregInput()
		{
			if (mInputHandler == null)
			{
				L.W(L.M.CALL_RETRY("unreg input"));
				return;
			}

			InputManager.g.DebugLock(false, GetHashCode());

			InputManager.g.Unreg(InputCode.Y, mInputHandler);

			InputManager.g.mask.Off((InputMaskKey)GetHashCode(), new []
			{
				InputCode.U, InputCode.D, InputCode.L, InputCode.R
			});

			mInputHandler = null;
		}

		void Rebase()
		{
			dialog.Rebase();
			mRebasabe = false;
		}

		void OnStrollDone(bool _end)
		{
			if (_end)
			{
				UnregInput();
				return;
			}

			mRebasabe = true;
		}
	}
}