#pragma warning disable 0168
using UnityEngine;

namespace Choanji
{
	public class ChoanjiInit : MonoBehaviour
	{
		public static bool sIsInited;

		public PrefabDB prefab;
		public TileDB tile;
		public CharacterSkins skin;
		public UI.DB ui;
		public Battle.SCDB battleSC;
		public Battle.FXDB battleFX;
		
		void Start()
		{
			if (sIsInited)
			{
				Destroy(gameObject);
				return;
			}

			sIsInited = true;

			ElementDB.Load();
			MoveDB.Load();
			CardDB.Load();
			Battle.BattlerDB.Load();

			PrefabDB.g = prefab;
			TileDB.g = tile;
			CharacterSkins.g = skin;
			UI.DB.g = ui;
			Battle.SCDB.g = battleSC;
			Battle.FXDB.g = battleFX;
			NPCDB.Load();

			var _choanjiAwake = TheChoanji.g;

			Destroy(gameObject, 0.1f);
		}
	}
}
