using UnityEngine;

namespace Choanji
{
	public class SoundDB : MonoBehaviour
	{
		public static SoundDB g;

		// bgm
		public AudioClip battleDefault;

		// se
		public AudioClip choose;

		public AudioClip hit;

		public AudioClip battleWin;
	}
}