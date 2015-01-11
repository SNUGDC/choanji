using System.Xml;
using Gem;

namespace Choanji
{
	public static partial class TiledParser
	{
		public static MapMeta ParseMeta(XmlNode _tmxRoot, string _name)
		{
			var _meta = new MapMeta(_name);

			var _props = _tmxRoot["map"]["properties"];
			if (_props == null)
			{
				L.W(L.M.KEY_NOT_EXISTS("properties"));
				return _meta;
			}

			// todo: meta properties.
			// var _propsJson = ParseProps(_props);

			return _meta;
		}
	}
}
