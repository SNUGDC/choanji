using Gem;
using UnityEngine;

namespace Choanji
{

	public class CharacterInspectTest : MonoBehaviour
	{
		private static readonly Path_ FILE_PATH = new Path_("Resources/Dialog");

		public CharacterInspectee ch;
		public string dialog;

		void Start()
		{
			ch.inspectee = new DialogInspectee { dialog = new DialogProvider(new FullPath(FILE_PATH / dialog)) };
		}

	}

}