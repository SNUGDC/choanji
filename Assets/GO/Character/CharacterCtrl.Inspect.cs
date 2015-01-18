
using Gem;

namespace Choanji
{

	public partial class CharacterCtrl
	{
		private bool mIsInspecting;

		public bool isInspecting
		{
			get { return mIsInspecting; }
			private set
			{
				if (isInspecting == value)
				{
					L.W(L.M.CALL_RETRY("set inspecting"));
					return;
				}

				mIsInspecting = value;

				if (isInspecting)
					blockMove.Add(CharacterMoveBlock.INSPECT);
				else
					blockMove.Remove(CharacterMoveBlock.INSPECT);
			}
		}

		private Connection<InspectResponse, IInspectee> mOnInspectDone;

		public void Inspect()
		{
			if (isInspecting)
			{
				L.E(L.M.CALL_RETRY("inspect"));
				return;
			}

			var _state = TryGetTileState(ch.front);

			if ((_state != null) && (_state.inspectee != null)
				&& _state.inspectee.CanStart())
			{
				isInspecting = true;

				if (mOnInspectDone == null)
					mOnInspectDone = new Connection<InspectResponse, IInspectee>(
					delegate { isInspecting = false; });

				_state.inspectee.Start(new InspectRequest(), mOnInspectDone);
			}
		}
	}


}