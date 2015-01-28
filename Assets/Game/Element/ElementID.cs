using Gem;

namespace Choanji
{
	public enum ElementID { }

	public struct ElementIDConverter 
		: IBiConverter<ElementID, int>
	{
		public int Convert(ElementID a)
		{
			return (int) a;
		}

		public ElementID Convert(int b)
		{
			return (ElementID) b;
		}
	}

}