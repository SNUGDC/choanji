using Gem;

namespace Choanji
{
	public static class CardHelper
	{
		public static CardID MakeID(string _name)
		{
			return (CardID) HashEnsure.Do(_name);
		}
	}

}