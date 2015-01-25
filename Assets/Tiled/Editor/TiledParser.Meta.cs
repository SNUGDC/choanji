using System.Xml;
using Gem;
using LitJson;

namespace Choanji
{
	public static partial class TiledParser
	{
		public static MapMeta ParseMeta(XmlNode _tmxRoot, string _name)
		{
			var _meta = new MapMeta(_name);

			var _propsNode = _tmxRoot["map"]["properties"];
			if (_propsNode == null)
			{
				L.W(L.M.KEY_NOT_EXISTS("properties"));
				return _meta;
			}

// 			var _props = ParseProps(_propsNode);
// 
// 			JsonData _prop;

			return _meta;
		}
	}
}
