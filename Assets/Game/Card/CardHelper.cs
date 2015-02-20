using Gem;

namespace Choanji
{
	public static class CardHelper
	{
		public static CardID MakeID(string _key)
		{
			return (CardID)HashEnsure.Do(_key);
		}

		public static PassiveID MakePassiveID(string _key)
		{
			return (PassiveID)HashEnsure.Do(_key);
		}

		public static ActiveID MakeActiveID(string _key)
		{
			return (ActiveID)HashEnsure.Do(_key);
		}
	}

}