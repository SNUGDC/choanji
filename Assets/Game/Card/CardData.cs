using System.Diagnostics;
using Gem;
using LitJson;

namespace Choanji
{
	public enum CardMode
	{
		PASSIVE,
		ACTIVE,
	}

	[DebuggerDisplay("{key}")]
	public class CardData
	{
		private CardData(string _key)
		{
			id = CardHelper.MakeID(_key);
			key = _key;	
		}

		public CardData(string _key, JsonData _data)
			: this(_key)
		{
			ele = ElementDB.Search(_data.StringOrDefault("ele", "NOR"));
			name = (string)_data["name"];
			detail = (string) _data["detail"];
			theme = _data.StringOrDefault("theme", "444444");
			stat = new StatSet(_data["stat"]);
			passive = MoveDB.Get(CardHelper.MakePassiveID((string)_data["passive"]));
			active = MoveDB.Get(CardHelper.MakeActiveID((string)_data["active"]));
		}
		
		public readonly CardID id;
		public readonly string key;
		public readonly ElementID ele;
		public readonly string name;
		public readonly string detail;
		public readonly string theme;
		public readonly StatSet stat;
		public readonly PassiveData passive;
		public readonly ActiveData active;

		public string richName { get { return UIHelper.RichAddColor(name, theme); } }

		public static implicit operator CardID(CardData _this)
		{
			return _this.id;
		}

		public string GetModeName(CardMode _mode)
		{
			switch (_mode)
			{
				case CardMode.PASSIVE:
					return passive.name;
				case CardMode.ACTIVE:
					return active.name;
				default:
					return "UNDEFINED_CARD_MODE";
			}
		}

		public CardUsage GetModeUsage(CardMode _mode)
		{
			switch (_mode)
			{
				case CardMode.PASSIVE:
					return passive.usage;
				case CardMode.ACTIVE:
					return active.usage;
				default:
					return CardUsage.NONE;
			}
		}
	}

}