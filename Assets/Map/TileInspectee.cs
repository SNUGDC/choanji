using Gem;

namespace Choanji
{

	public class TileInspectee : TileOwnership
	{
		public IInspectee inspectee { get; private set; }

		private Grid<TileState> mStates;
		public Grid<TileState> states
		{
			get { return mStates; }

			set
			{
				if (states == value)
					return;
				mStates = value;
				Release();
			}
		}

		public TileInspectee(IInspectee _inspectee)
		{
			inspectee = _inspectee;
		}

		protected override bool DoRetain(Point _p)
		{
			if (states == null)
			{
				L.W(L.M.SHOULD_NOT_NULL("states"));
				return false;
			}

			var _state = states[_p];
			if (_state.inspectee == null)
				_state.inspectee = inspectee;
			else
				L.E(L.M.SHOULD_NULL("inspectee"));
			return true;
		}

		protected override void DoRelease()
		{
			var _state = states[position.Value];
			var _org = _state.inspectee;
			if (_org == inspectee)
				_state.inspectee = null;
			else
				L.E(L.M.STATE_INVALID);
		}
	}

}