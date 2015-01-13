using UnityEngine;

namespace Choanji
{

	public class CharacterInspectTest : MonoBehaviour
	{
		public CharacterInspectee ch;

		void Start()
		{
			ch.inspectee = new DialogInspectee();
		}

	}

}