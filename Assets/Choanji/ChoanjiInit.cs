#pragma warning disable 0168
using UnityEngine;

namespace Choanji
{
	public class ChoanjiInit : MonoBehaviour
	{
		public Battle.SCDB sc;

		void Start()
		{
			Battle.SCDB.g = sc;
			var _choanjiAwake = TheChoanji.g;
		}
	}
}
