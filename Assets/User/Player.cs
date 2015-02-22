using Choanji.Battle;
using LitJson;

namespace Choanji
{
	public static class Player
	{
		public static string name = "플레이어";
		public static StatSet stat = new StatSet();
		public static Deck deck = new Deck();
		public static Party party = new Party();

		public static void Load(JsonData _data)
		{
			stat = new StatSet(_data["stat"]);
			deck = new Deck(_data["deck"]);
			party = new Party(deck, _data["party"]);
		}

		public static void Save(JsonData _data)
		{
			_data["stat"] = stat.Serialize();
			_data["deck"] = deck.Serialize();
			_data["party"] = party.Serialize();
		}

		public static BattlerData MakeBattler()
		{
			return new BattlerData(name, stat, party);
		}
	}
}