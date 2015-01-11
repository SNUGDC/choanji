using System.Collections.Generic;
using System.Xml;
using Gem;
using LitJson;

namespace Choanji
{
	public static partial class TiledParser
	{
		private static KeyValuePair<string, string>? ParseProp(XmlNode _node)
		{
			var _attrs = _node.Attributes;

			string _name = null;
			string _val = null;

			foreach (var _attrObj in _attrs)
			{
				var _attr = (XmlAttribute)_attrObj;
				if (_attr.Name == "name")
					_name = _attr.Value;
				else if (_attr.Name == "value")
					_val = _attr.Value;
			}

			if ((_name == null) || (_val == null))
			{
				L.E(L.M.KEY_NOT_EXISTS("name or value"));
				return null;
			}

			return new KeyValuePair<string, string>(_name, _val);
		}

		public static JsonData ParseProps(XmlNode _node)
		{
			var _props = new JsonData();

			foreach (var _propObj in _node)
			{
				var _propCheck = ParseProp((XmlNode)_propObj);
				if (_propCheck == null) continue;

				var _prop = _propCheck.Value;
				var _propName = _prop.Key;

				object _parsed;
				XmlHelper.ParseValue(_prop.Value, out _parsed);
				_props.AssignPrimitive(_propName, _parsed);
			}

			return _props;
		}
	}
}
