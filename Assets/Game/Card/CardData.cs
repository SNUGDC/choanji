using Gem;

namespace Choanji
{
	public enum CardMode
	{
		PASSIVE,
		ACTIVE,
	}

	public class CardData
	{
		CardData(string _name, string _detail, StatSet _stat, PassiveData _passive, ActiveData _active)
		{
			id = (CardID) HashEnsure.Do(_name);
			name = _name;
			detail = _detail;
			stat = _stat;
			passive = _passive;
			active = _active;
		}

		public readonly CardID id;
		public readonly string name;
		public readonly string detail;
		public readonly StatSet stat;
		public readonly PassiveData passive;
		public readonly ActiveData active;
	}

}