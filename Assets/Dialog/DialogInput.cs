using Gem;
using Gem.In;

namespace Choanji
{
	public class DialogInput
	{
		public bool isConn
		{
			get
			{
				// to avoid unity moan CompareBaseObjectsInternal
				return !ReferenceEquals(mDialog, null);
			}
		}

		private Dialog mDialog;
		private bool mRebasabe;
		private InputBind mInputBind;

		~DialogInput()
		{
			if (isConn)
				Dis();
		}

		public void Conn(Dialog _dialog)
		{
			if (isConn)
			{
				L.W(L.M.CALL_RETRY("connect"));
				return;
			}

			mDialog = _dialog;
			mDialog.events.onStrollDone += OnStrollDone;
			RegInput();
		}

		public void Dis()
		{
			if (!isConn)
			{
				L.W(L.M.CALL_RETRY("disconnect"));
				return;
			}

			UnregInput();
			mDialog.events.onStrollDone -= OnStrollDone;
			mDialog = null;
		}

		void RegInput()
		{
			if (mInputBind.handler == null)
			{
				mInputBind.code = InputCode.Y;

				mInputBind.handler = new InputHandler
				{
					down = delegate
					{
						if (mRebasabe)
							Rebase();
						return true;
					}
				};
			}

			InputManager.g.Reg(mInputBind);
		}

		void UnregInput()
		{
			InputManager.g.Unreg(mInputBind);
		}

		void Rebase()
		{
			mDialog.Rebase();
			mRebasabe = false;
		}

		void OnStrollDone(bool _end)
		{
			if (_end)
			{
				Dis();
				return;
			}

			mRebasabe = true;
		}
	}
}