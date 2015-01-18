using Gem;
using UnityEngine;

namespace Choanji
{

	public class CharacterInspectTest : MonoBehaviour
	{
		public CharacterInspectee ch;
		public string dialog;

		void Start()
		{
			ch.inspectee = new DialogInspectee { dialog = new DialogProvider(new Path_(dialog)) };
		}

	}

}