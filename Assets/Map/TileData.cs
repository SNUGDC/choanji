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
		public MapLayerType inspectee;
		private JsonData[] mDatas;

		public JsonData GetInspectableData()
		{
			if (inspectee.IsDefault()) return null;
			return mDatas[(int) inspectee - 1];
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
			if (!inspectee.IsDefault())
#endif
			{
				JsonData _inspJson;
				if (_data.TryGet("inspectee", out _inspJson))
				{
					var _insp = (bool) _inspJson;
#if UNITY_EDITOR
					if (!inspectee.IsDefault() && _insp)
						L.W(L.M.CALL_RETRY("set inspectee"));
#endif
					if (_insp) inspectee = _layer;
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