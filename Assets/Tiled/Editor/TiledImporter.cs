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
		private const string TMX_PATH = "./TMX/";

		public void HandleCustomProperties(GameObject _go, IDictionary<string, string> _props)
		{
		}

		public void CustomizePrefab(GameObject _prefab)
		{
			var _name = _prefab.name;
			var _tmxPath = TMX_PATH + _name + ".tmx";
			var _tmxRoot = new XmlDocument();

			try
			{
				using (var _f = new FileStream(_tmxPath, FileMode.Open, FileAccess.Read))
					_tmxRoot.Load(_f);
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
				return;
			}

			var _mapData = _prefab.AddComponent<MapData>();
			_mapData.meta = TiledParser.ParseMeta(_tmxRoot, _name);
			_mapData.grid = TiledParser.ImportData(_tmxRoot);
		}
	}


}