using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Gem;
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

		public static string MapSortingLayerName(string _objName)
		{
			switch (_objName)
			{
				case "FLR":
				case "RUG":
				case "WALL":
					return "Floor";
				case "ATTACH":
					return "Attach";
				case "STD_LOW":
					return "StandLower";
				case "STD_UP":
					return "StandUpper";
				case "OBJ_LOW":
					return "ObjectLower";
				case "OBJ_UP":
					return "ObjectUpper";
				default:
					L.W("no matching layerName for " + _objName);
					return null;
			}
		}

		public void HandleMapProperties(GameObject _go, IDictionary<string, string> _props)
		{
		}

		public void CustomizeMap(GameObject _prefab) 
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

			MapDB.TryLoad();

			var _tiledMap = _prefab.GetComponent<TiledMap>();
			_prefab.transform.SetPosY(_tiledMap.TileHeight);

			var _meta = TiledParser.ParseMeta(_tmxRoot, _name);
			MapDB.Replace(new MapStatic(_meta));
			MapDB.Save();

			var _grid = TiledParser.ParseData(_tmxRoot);
			MapUtil.SaveTileGrid(_meta.name, _grid);

			_prefab.AddComponent<MapStaticComp>().binName = _name;
		}

		public void CustomizeGO(GameObject _go, IDictionary<string, string> _props, TileData _tileData)
		{
		}
	}
}