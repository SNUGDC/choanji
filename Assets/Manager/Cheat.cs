using Gem;
using LitJson;

namespace Choanji
{
	public static class Cheat
	{
		private static readonly JsonData sData = JsonHelper.DataWithRaw(new FullPath("Resources/Config/cheat.json"));

		public static bool BoolOrDefault(string _key, bool _default = false)
		{
			if (sData == null) return _default;
			return sData.BoolOrDefault(_key, _default);
		}
	}
}