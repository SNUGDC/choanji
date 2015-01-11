using System;
using Gem;
using LitJson;

namespace Choanji
{
	[Serializable]
	public class TileData
	{
		public bool occupied;
		public Direction wall;
		private JsonData[] mDatas;

		public JsonData this[MapLayerType _layer]
		{
			get
			{
				if (mDatas == null) return null;
				return mDatas[(int)_layer];
			}
		}

		public void Merge(MapLayerType _layer, JsonData _data)
		{
			if (mDatas == null)
				mDatas = new JsonData[(int) MapLayerType._COUNT];

#if UNITY_EDITOR
			var _layerData = mDatas[(int) _layer];
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
		}
	}
}