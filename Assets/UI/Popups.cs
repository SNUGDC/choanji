using Gem;

namespace Choanji.UI
{
	public static class Popups
	{
		public static readonly PopupKey PARTY = PopupHelper.MakeKey("PARTY");
		public static readonly PopupKey BATTLE_WIN = PopupHelper.MakeKey("BATTLE_WIN");
		public static readonly PopupKey BATTLE_LOSE = PopupHelper.MakeKey("BATTLE_LOSE");

		public static void Setup()
		{
			PopupDB.Add(PARTY, delegate
			{
				return DB.g.deckPopupPrf.Instantiate();
			});

			// todo: battle popup
		}
	}
}