using Gem;

namespace Choanji
{

	public sealed class TileOccupy : TileOwnership
	{
		private readonly Grid<TileState> mStates;

		public TileOccupy(Grid<TileState> _states)
		{
			mStates = _states;
		}

		protected override bool DoRetain(Point _p)
		{
			mStates[_p].Occupy();
			return true;
		}

		protected override void DoRelease()
		{
			mStates[position.Value].Unoccupy();
		}
	}

}