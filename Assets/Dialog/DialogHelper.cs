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

		public static DialogProvider MakeProvider(string _name)
		{
			var _fullpath = FullPath(_name);
			if (_fullpath.Exists())
				return new DialogProvider(_fullpath);
			else
				return DialogDB.Make(_name);
		}
	}
}