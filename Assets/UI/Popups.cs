using Gem;

namespace Choanji.UI
{
	public static class Popups
	{
		public static readonly PopupKey PARTY = PopupHelper.MakeKey("PARTY");

		public static void Setup()
		{
			PopupDB.Add(PARTY, delegate
			{
				return DB.g.deckPopupPrf.Instantiate();
			});
		}
	}
}