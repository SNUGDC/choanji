
using Gem;

namespace Choanji
{

	public partial class CharacterCtrl
	{
		public bool isInspecting { get; private set; }
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