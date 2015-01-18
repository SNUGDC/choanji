using System.Collections.Generic;
using UnityEngine;

namespace Choanji
{

	public class DialogTest : MonoBehaviour
	{
		public List<string> txts;

		void Start()
		{
			var _provider = new DialogProvider { txts };
			TheDialog.g.Open(_provider, null);
		}

	}


}