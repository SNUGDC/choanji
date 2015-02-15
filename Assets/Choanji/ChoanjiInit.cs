#pragma warning disable 0168
using UnityEngine;

namespace Choanji
{
	public class ChoanjiInit : MonoBehaviour
	{
		public PrefabDB prefab;
		public TileDB tile;
		public CharacterSkins skin;
		public Battle.SCDB sc;
		
		void Start()
		{
			PrefabDB.g = prefab;
			TileDB.g = tile;
			CharacterSkins.g = skin;
			Battle.SCDB.g = sc;
			NPCDB.Load();
			var _choanjiAwake = TheChoanji.g;
		}
	}
}
