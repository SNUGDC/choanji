#pragma warning disable 0168
using UnityEngine;

namespace Choanji
{
	public class ChoanjiInit : MonoBehaviour
	{
		public TileDB tile;
		public Battle.SCDB sc;
		
		void Start()
		{
			TileDB.g = tile;
			Battle.SCDB.g = sc;
			var _choanjiAwake = TheChoanji.g;
		}
	}
}
