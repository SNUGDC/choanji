using UnityEngine;

namespace Choanji
{
	public class MapDataComp : MonoBehaviour
	{
		public string binName;
		public MapData data { get; private set; }

		void Awake()
		{
			Load();
		}

		public void Load()
		{
			data = MapData.Load(binName);
		}

		public static implicit operator MapData(MapDataComp _this)
		{
			return _this.data;
		}
	}
}