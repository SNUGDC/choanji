using Gem;

namespace Choanji
{
	public static class DialogHelper
	{
		private static readonly Path_ DIR = new Path_("Resources/Dialog");

		public static FullPath FullPath(string _name)
		{
			return new FullPath(DIR / (_name + ".json"));
		}
	}
}