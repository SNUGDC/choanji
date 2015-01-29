namespace Choanji
{
	public enum CardMode
	{
		PASSIVE,
		ACTIVE,
	}

	public class CardData
	{
		public CardID id;
		public string name;
		public string detail;
		public StatSet stat;
		public PassiveData passive;
		public ActiveData active;
	}

}