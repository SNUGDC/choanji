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

			// var _tiledMap = _prefab.GetComponent<TiledMap>();
			// _prefab.transform.SetPosY(_tiledMap.TileHeight);

			var _meta = TiledParser.ParseMeta(_tmxRoot, _name);
			var _mapStatic = new MapStatic(_meta);
			_mapStatic.grid = TiledParser.ParseData(_tmxRoot);
			MapDB.Replace(_mapStatic);

			var _mapStaticComp = _prefab.AddComponent<MapStaticComp>();
			_mapStaticComp.binName = _name;
			_mapStaticComp.data = _mapStatic;
		}

		public bool CustomizeGO(
			GameObject _go, IDictionary<string, string> _props, 
			MapStatic _map, Coor _coor, TileData _tile)
		{
			string _type;
			if (!_props.TryGet("type", out _type))
				return false;

			switch (_type)
			{
				case "Door":
					return CustomizeDoor(_go, _props, _map, _coor, _tile);

				default:
					return false;
			}
		}

		private bool CustomizeDoor(
			GameObject _go, IDictionary<string, string> _props, 
			MapStatic _map, Coor _coor, TileData _tile)
		{
			if (_map.meta.doors == null)
				_map.meta.doors = new Dictionary<TileDoorKey, Coor>();

			var _key = TileDataHelper.MakeDoorKey(_props["key"]);
			_map.meta.doors.Add(_key, _coor);

			D.Assert(_tile.door == null);
			_tile.door = new TileDoorData(_key, 
				_props["exit_world"], _props["exit_room"], 
				_props["exit_map"], TileDataHelper.MakeDoorKey(_props["exit_door"]));

			return false;
		}

		public void BeforeSave(GameObject _prefab)
		{
			MapDB.Save();
			var _mapStaticComp = _prefab.GetComponent<MapStaticComp>();
			MapUtil.SaveTileGrid(_mapStaticComp.binName, _mapStaticComp.data.grid);
		}
	}
}