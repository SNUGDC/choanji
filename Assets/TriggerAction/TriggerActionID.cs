using Gem;
using LitJson;

namespace Choanji
{
	public enum TriggerActionID { }

	public static class TriggerActionHelper
	{
		public static TriggerActionID MakeID(string _key)
		{
			return (TriggerActionID)HashEnsure.Do(_key);
		}

		public static JsonData Read(string _name)
		{
			return JsonHelper.DataWithRaw(new FullPath("Resources/TriggerAction/" + _name + ".json"));
		}
	}
}