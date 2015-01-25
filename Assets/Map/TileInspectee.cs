using Gem;

namespace Choanji
{

	public class TileInspectee : TileOwnership
	{
		public IInspectee inspectee { get; private set; }

		public TileInspectee(IInspectee _inspectee)
		{
			inspectee = _inspectee;
		}

		protected override bool DoRetain(LocalCoor p)
		{
			var _state = p.FindState();
			if (_state.inspectee != null)
				return false;
			_state.inspectee = inspectee;
			return true;
		}

		protected override void DoRelease()
		{
			var _state = position.Value.FindState();

			var _org = _state.inspectee;
			if (_org == inspectee)
				_state.inspectee = null;
			else
				L.E(L.M.STATE_INVALID);
		}
	}

}