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

			if (Cheat.BoolOrDefault("card_all"))
			{
				deck = new Deck();
				foreach (var _card in CardDB.GetEnum())
					deck.cards.Add(_card, new Card(_card));
			}
			else
			{
				deck = new Deck(_data["deck"]);
			}

			
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