using UnityEngine;

namespace Choanji
{

	public class TheCharacterTest : MonoBehaviour
	{
		public Character ch;

		void Start()
		{
			TheCharacter.g = ch;
		}
	}


}