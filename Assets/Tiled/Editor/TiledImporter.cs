using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Tiled2Unity;
using UnityEngine;

namespace Choanji
{
	[CustomTiledImporter]
	public class TiledImporter : ICustomTiledImporter
	{
		private static string GetTMXPath(string _name)
		{
			return "./TMX/" + _name + ".tmx";
		}

		public void HandleCustomProperties(GameObject _go, IDictionary<string, string> _props)
		{
		}

		public void CustomizePrefab(GameObject _prefab)
		{
			var _name = _prefab.name;
			var _tmxPath = GetTMXPath(_name);
			var _tmxRoot = new XmlDocument();

			try
			{
				using (var _f = new FileStream(_tmxPath, FileMode.Open, FileAccess.Read))
					_tmxRoot.Load(_f);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return;
			}

			var _meta = TiledParser.ParseMeta(_tmxRoot, _name);
			MapDB.TryLoad();
			MapDB.Replace(_meta);
			MapDB.Save();

			var _mapData = TiledParser.ParseData(_tmxRoot);
			MapUtil.SaveTileGrid(_meta.name, _mapData);

			_prefab.AddComponent<MapDataComp>().binName = _name;
		}
	}


}