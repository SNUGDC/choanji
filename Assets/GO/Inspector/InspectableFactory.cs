using LitJson;

namespace Choanji
{
	public static class InspectorFactory
	{
		public static IInspectable Make(JsonData _data)
		{
			return new JsonInspectable(_data);
		}
	}
}