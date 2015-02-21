using Gem;
using LitJson;

namespace Choanji
{
	public static class DialogDB
	{
		private static JsonData sDic = JsonHelper.DataWithRaw(new FullPath("Resources/Dialog/fallback.json"));

		public static DialogProvider Make(string _key)
		{
			JsonData _dialog;

			if (sDic.TryGet(_key, out _dialog))
				return new DialogProvider(_dialog);
			else
				return null;
		}
	}
}