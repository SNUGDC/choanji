using UnityEngine;

namespace Choanji
{
	public class TileTASpawner : MonoBehaviour
	{
		public string data;
		private TriggerAction mTA;

		void Start()
		{
			var _coor = TileHelper.GetLocalCoor(transform);
			mTA = TriggerActionFactory.Make(_coor, TriggerActionHelper.Read(data));
			if (mTA != null) mTA.isEnabled = true;
		}
	}
}