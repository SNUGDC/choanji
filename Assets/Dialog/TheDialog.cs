using System;
using System.Collections.Generic;
using Gem;
using Gem.In;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Choanji
{

	public class TheDialog
	{
		public static readonly TheDialog g = new TheDialog();

		public RectTransform dialogParent;
		public Dialog dialogPrefab;

		public int session { get; private set; }
		public bool isOpened { get { return dialog != null; }}

		private Dialog mDialog;
		public Dialog dialog
		{
			get { return mDialog; }
			private set
			{
				if (dialog == value)
				{
					return;
				}

				if (value == null)
				{
					Object.Destroy(dialog.gameObject);
					session = 0;
				}
				else
				{
					session = MathHelper.RandPosInt();
				}

				mDialog = value;
			}
		}

		private readonly DialogInput mInput = new DialogInput();

		private DialogProvider mProvider;
		private DialogHandler mHandler;
		private IEnumerator<string> mIndexer;

		public int Open(DialogProvider _provider, DialogHandler _handler)
		{
			if (isOpened)
			{
				L.E(L.M.CALL_RETRY("open"));
				return 0;
			}

			dialog = dialogPrefab.Instantiate();
			dialog.transform.SetParent(dialogParent);
			dialog.transform.CastRectAndAssignWith(dialogPrefab.transform);
			dialog.events.onStrollDone += OnStrollDone;

			mProvider = _provider;
			mIndexer = mProvider.GetEnumerator();

			mHandler = _handler;

			mInput.Conn(dialog);

			Next();

			return session;
		}

		public void Close(int _session)
		{
			if (!isOpened)
			{
				L.E(L.M.CALL_RETRY("close"));
				return;
			}

			if (session != _session)
			{
				L.E(L.M.STATE_INVALID);
				return;
			}

			Done(false);
		}

		private void OnStrollDone(bool _end)
		{
			if (!_end) return;

			var _bind = new InputBind(InputCode.Y, new InputHandler());

			_bind.handler.down = _bind.MakeOneShot(TheInput.world, delegate { 
				if (!Next()) 
					Done(true);
				return true;
			});

			TheInput.world.Reg(_bind);
		}

		private bool Next()
		{
			if (mIndexer.MoveNext())
			{
				dialog.orgText = mIndexer.Current;
				return true;
			}
			else
			{
				Done(true);
				return false;
			}
		}

		private void Done(bool _exitNorm)
		{
			Action<bool> _onDone = null;

			if (mHandler != null)
				_onDone = mHandler.onDone;

			if (mInput.isConn)
				mInput.Dis();

			mHandler = null;
			mProvider = null;
			mIndexer = null;
			dialog = null;

			// for reentrancy.
			Timer.g.Add(0, () => _onDone.CheckAndCall(_exitNorm));
		}
	}

}