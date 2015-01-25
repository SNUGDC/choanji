using Gem;

namespace Choanji
{

	public sealed class TileOccupy : TileOwnership
	{
		protected override bool DoRetain(LocalCoor p)
		{
			p.FindState().Occupy();
			return true;
		}

		protected override void DoRelease()
		{
			position.Value.FindState().Unoccupy();
		}
	}

}