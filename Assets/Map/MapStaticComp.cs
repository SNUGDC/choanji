using Gem;
using UnityEngine;

namespace Choanji
{
	public class MapStaticComp : MonoBehaviour
	{
		[HideInInspector]
		public string binName;

		private MapStatic mData;
		public MapStatic data {
			get
			{
				if (mData != null) 
					return mData;
				MapDB.TryGet(MapIDHelper.Make(binName), out mData);
				return mData;
			}

#if UNITY_EDITOR
			set
			{
				D.Assert(mData == null);
				mData = value;
			}
#endif
		}
	}
}