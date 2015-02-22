using LitJson;

namespace Choanji
{
	public static class Player
	{
		public static StatSet stat;
		public static Deck deck;
		public static Party party;

		public static void Load(JsonData _data)
		{
			stat = new StatSet(_data["stat"]);
			deck = new Deck(_data["deck"]);
			party = new Party(_data["party"]);
		}

		public static void Save(JsonData _data)
		{
			_data["stat"] = stat.Serialize();
			_data["deck"] = deck.Serialize();
			_data["party"] = party.Serialize();
		}
	}
}