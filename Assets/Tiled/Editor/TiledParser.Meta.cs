using System.Xml;
using Gem;

namespace Choanji
{
	public static partial class TiledParser
	{
		public static MapMeta ParseMeta(XmlNode _tmxRoot, string _name)
		{
			var _mapAttrs = _tmxRoot["map"].Attributes;

			var _size = new Point(
				_mapAttrs["width"].AsInt(), 
				_mapAttrs["height"].AsInt());

			var _meta = new MapMeta(_name, _size);

			var _propsNode = _tmxRoot["map"]["properties"];
			if (_propsNode == null)
			{
				L.D(L.M.KEY_NOT_EXISTS("properties"));
				return _meta;
			}

// 			var _props = ParseProps(_propsNode);
// 
// 			JsonData _prop;

			return _meta;
		}
	}
}
