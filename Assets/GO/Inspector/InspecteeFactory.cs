using LitJson;

namespace Choanji
{
	public static class InspecteeFactory
	{
		public static IInspectee Make(JsonData _data)
		{
			return new JsonInspectee(_data);
		}
	}
}