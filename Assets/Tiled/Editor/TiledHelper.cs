using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Gem;
using LitJson;
using Tiled2Unity;
using UnityEngine;

namespace Choanji
{
	public enum TileID
	{
		FIRST = 0,
	}

	public enum TileGID {}

	public static class TiledHelper
	{
		private const string TMX_PATH = "./TMX/";

		public class TileDatas
		{
			private readonly List<JsonData> mList = new List<JsonData>();

			public int count { get { return mList.Count;  }}

			public void Resize(TileGID _id)
			{
				mList.Resize((int) _id);
			}

			public JsonData this[TileGID _id]
			{
				get { return mList[(int)_id]; }
				set { mList[(int)_id] = value; }
			}
		}

		public class TilesetData
		{
			public TileGID firstGID;
			public TileGID endGID { get { return firstGID + count; } }
			public int tileSize;
			public Point imgSize;
			public int count { get; private set; }
			public Point gridSize { get; private set; }
			public List<TileData> dic { private get; set; }

			public bool IsMine(TileGID _id)
			{
				return (_id >= firstGID) && (_id < endGID);
			}

			public void Commit()
			{
				gridSize = imgSize / tileSize;
				count = gridSize.x * gridSize.y;
			}

			public TileGID ToGID(TileID _id)
			{
				return firstGID + (int) _id;
			}
		}

		public class TilesetDatas
		{
			public void Add(TilesetData _tileset)
			{
				mList.Add(_tileset);
				if (mLastMatchTileset == null)
					mLastMatchTileset = _tileset;
			}

			public TilesetData Get(TileGID _id)
			{
				if (mLastMatchTileset.IsMine(_id))
					return mLastMatchTileset;

				foreach (var _tileset in mList)
				{
					if (_tileset == mLastMatchTileset)
						continue;
					if (_tileset.IsMine(_id))
						return _tileset;
				}

				L.E(L.M.KEY_NOT_EXISTS(_id));
				return null;
			}

			private TilesetData mLastMatchTileset;
			private readonly List<TilesetData> mList = new List<TilesetData>();
		}

		public static Grid<TileData> ImportData(string _xmlName)
		{
			var _tmxPath = TMX_PATH + _xmlName;
			var _tmxRoot = new XmlDocument();
			using (var _f = new FileStream(_tmxPath, FileMode.Open, FileAccess.Read))
				_tmxRoot.Load(_f);

			var _tmx = _tmxRoot["map"];
			var _tiles = new TileDatas();
			var _tilesets = new TilesetDatas();

			var _mapAttrs = _tmx.Attributes;
			var _mapSize = new Point(_mapAttrs["width"].AsInt(), _mapAttrs["height"].AsInt());

			// parse tileset
			foreach (var _nodeObj in _tmx)
			{
				var _node = (XmlNode) _nodeObj;
				if (_node.Name != "tileset") continue;
				_tilesets.Add(ParseTileset(_node, ref _tiles));
			}

			// parse layer
			var _layers = new Dictionary<MapLayerType, Grid<JsonData>>();

			foreach (var _nodeObj in _tmx)
			{
				var _node = (XmlNode) _nodeObj;
				if (_node.Name != "layer") continue;

				MapLayerType _layerType;
				var _attrs = _node.Attributes;

				if (!EnumHelper.TryParse(_attrs["name"].Value, out _layerType))
					continue;

				var _layerSize = new Point(_attrs["width"].AsInt(), _attrs["height"].AsInt());
				D.Assert(_mapSize == _layerSize);

				_layers.Add(_layerType, MakeGrid(_node["data"], _mapSize, _tiles));
			}

			return MergeLayers(_layers);
		}

		private static TilesetData ParseTileset(XmlNode _tileset, ref TileDatas _datas)
		{
			var _ret = new TilesetData();
			var _attrs = _tileset.Attributes;

			_ret.firstGID = (TileGID) _attrs["firstgid"].AsInt();

			_ret.tileSize = _attrs["tilewidth"].AsInt();
			D.Assert(_ret.tileSize == _attrs["tileheight"].AsInt());

			var _imgAttrs = _tileset["image"].Attributes;
			_ret.imgSize = new Point(_imgAttrs["width"].AsInt(), _imgAttrs["height"].AsInt());
			_ret.Commit();
			
			if ((int) _ret.endGID > _datas.count)
				_datas.Resize(_ret.endGID);

			foreach (var _tileObj in _tileset)
			{
				var _tile = (XmlElement)_tileObj;
				if (_tile.Name != "tile") continue;
				var _tileID = (TileID) int.Parse(_tile.Attributes["id"].Value);
				_datas[_ret.ToGID(_tileID)] = ParseTile(_tile);
			}

			return _ret;
		}

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

		private static JsonData ParseTile(XmlNode _tile)
		{
			var _propsNode = _tile["properties"];
			if (_propsNode == null) return null;

			var _props = new JsonData();

			foreach (var _propObj in _propsNode)
			{
				var _propCheck = ParseProp((XmlNode) _propObj);
				if (_propCheck == null) continue;

				var _prop = _propCheck.Value;
				var _propName = _prop.Key;

				object _parsed;
				XmlHelper.ParseValue(_prop.Value, out _parsed);
				_props.AssignPrimitive(_propName, _parsed);
			}

			return _props;
		}

		private static Grid<JsonData> MakeGrid(XmlNode _xml, Point _size, TileDatas _tiles)
		{
			var _ret = new Grid<JsonData>(_size);
			var _idx = 0;

			foreach (var _tileObj in _xml)
			{
				var _tileNode = (XmlNode) _tileObj;
				var _gid = (TileGID) _tileNode.Attributes["gid"].AsInt();
				var _pos = new Point(_idx%_size.x, _size.y - 1 - _idx/_size.y);
				_ret[_pos] = _tiles[_gid];
				++_idx;
			}

			return _ret;
		}
		
		private static Grid<TileData> MergeLayers(Dictionary<MapLayerType, Grid<JsonData>> _layers)
		{
			if (_layers.Count == 0) 
				return null;

			var _size = _layers.First().Value.size;
			var _ret = new Grid<TileData>(_size);

			foreach (var p in _size.Range())
			{
				TileData _tile = null;
				foreach (var _layerKV in _layers)
				{
					var _layerTile = _layerKV.Value[p];
					if (_layerTile == null) continue;
					if (_tile == null) _tile = new TileData();
					_tile.MergeData(_layerKV.Key, _layerTile);
				}
				_ret[p] = _tile;
			}

			return null;
		}
	}
}