using Gem;

namespace Choanji.R.Game
{
	public static class Spr
	{
		public const string CARD_PASSIVE_ICON = "passive";
		public const string CARD_ACTIVE_ICON = "active";

		public static string CardIcon(CardMode _mode)
		{
			switch (_mode)
			{
				case CardMode.PASSIVE:
					return CARD_PASSIVE_ICON;
				case CardMode.ACTIVE:
					return CARD_ACTIVE_ICON;
				default:
					D.Assert(false);
					return string.Empty;
			}
		}
	}
}