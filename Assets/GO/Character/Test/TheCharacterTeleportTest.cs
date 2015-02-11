#if UNITY_EDITOR

using Gem;
using UnityEngine;

namespace Choanji {

	public class TheCharacterTeleportTest : MonoBehaviour {

		public float delay;
		public string world;
		public string map;
		public Coor coor;
		
		void Start () {
			Timer.g.Add(delay, () => TheCharacter.TeleportNextUpdate(
				new WorldAddress(world, 
					WorldBluePrint.Room.MakeKey(map), 
					coor)));
		}
	}

}

#endif