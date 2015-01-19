using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Gem;
using LitJson;

namespace Choanji
{
	public enum TileID { }

	public enum TileGID
	{
		NONE = 0,
	}

	public static partial class TiledParser
	{
		struct TileAndJson
		{
			public TileGID id;
			public JsonData json;
		}

		class TileDatas
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

		class TilesetData
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

		class TilesetDatas
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

		public static Grid<TileData> ParseData(XmlNode _tmxRoot)
		{
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
			var _layers = new Dictionary<MapLayerType, Grid<TileAndJson>>();

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

				var _props = _tile["properties"];
				if (_props != null) 
					_datas[_ret.ToGID(_tileID)] = ParseProps(_props);
			}

			return _ret;
		}

		private static Grid<TileAndJson> MakeGrid(XmlNode _xml, Point _size, TileDatas _tiles)
		{
			var _ret = new Grid<TileAndJson>(_size);
			var _idx = -1;

			foreach (var _tileObj in _xml)
			{
				++_idx;
				var _tileNode = (XmlNode)_tileObj;
				var _gid = (TileGID) _tileNode.Attributes["gid"].AsInt();
				if (_gid == TileGID.NONE) continue;
				var _pos = new Point(_idx%_size.x, _size.y - 1 - _idx/_size.x);
				_ret[_pos] = new TileAndJson {id = _gid, json = _tiles[_gid]};
			}

			return _ret;
		}
		
		private static Grid<TileData> MergeLayers(Dictionary<MapLayerType, Grid<TileAndJson>> _layers)
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

					if (_layerTile.id == TileGID.NONE) 
						continue;

					if (_tile == null) 
						_tile = new TileData();

					if (_layerTile.json != null)
						_tile.Merge(_layerKV.Key, _layerTile.json);
				}
				_ret[p] = _tile;
			}

			return _ret;
		}
	}
}