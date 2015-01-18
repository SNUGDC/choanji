
using Gem;

namespace Choanji
{
	public class CharacterInspectRequest : InspectRequest
	{
		public CharacterInspectRequest(Character _ch)
		{
			ch = _ch;
		}

		public readonly Character ch;
	}

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

		private ActionWrap<InspectResponse, IInspectee> mOnInspectDone;

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
					mOnInspectDone = new ActionWrap<InspectResponse, IInspectee>
					{ val = delegate { isInspecting = false; } };

				_state.inspectee.Start(new CharacterInspectRequest(ch), mOnInspectDone);
			}
		}
	}


}