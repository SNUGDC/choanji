using System;
using System.Collections;
using System.Collections.Generic;
using Gem;
using LitJson;

namespace Choanji
{
	[Serializable]
	public class TileData : IEnumerable<JsonData>
	{
		public bool occupied;
		public Direction wall;
		public MapLayerType inspectable;
		private JsonData[] mDatas;

		public JsonData GetInspectableData()
		{
			if (inspectable.IsDefault()) return null;
			return mDatas[(int) inspectable - 1];
		}

		public JsonData this[MapLayerType _layer]
		{
			get
			{
				if (mDatas == null) return null;
				return mDatas[(int)_layer - 1];
			}
		}

		public void Merge(MapLayerType _layer, JsonData _data)
		{
			if (mDatas == null)
				mDatas = new JsonData[(int) MapLayerType._COUNT];

#if UNITY_EDITOR
			var _layerData = mDatas[(int) _layer - 1];
			if (_layerData != null)
				L.E(L.M.CALL_RETRY("merge data " + _layer));
#endif

			if (!occupied)
			{
				JsonData _occupied;
				if (_data.TryGet("occupied", out _occupied))
					occupied |= (bool)_occupied;
			}

			do
			{
				JsonData _wall;
				if (!_data.TryGet("wall", out _wall))
					break;
				wall = DirectionHelper.MakeWithAbbr((string) _wall);
			} while (false);

#if !UNITY_EDITOR
			if (!inspectable)
#endif
			{
				JsonData _inspJson;
				if (_data.TryGet("inspectable", out _inspJson))
				{
					var _insp = (bool) _inspJson;
#if UNITY_EDITOR
					if (inspectable != default(MapLayerType) && _insp)
						L.W(L.M.CALL_RETRY("set inspectable"));
#endif
					if (_insp) inspectable = _layer;
				}
			}
		}

		public IEnumerator<JsonData> GetEnumerator()
		{
			return mDatas.GetReverseEnum().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}