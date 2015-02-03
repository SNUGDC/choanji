using Gem;

namespace Choanji
{
	public class ActiveData
	{
		public ActiveData(string _name, string _detail, int _cost)
		{
			id = (ActiveID) HashEnsure.Do(_name);
			name = _name;
			detail = _detail;
			cost = _cost;
		}

		public readonly ActiveID id;
		public readonly string name;
		public readonly string detail;

		public readonly int cost;
	}
}