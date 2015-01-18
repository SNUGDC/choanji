#pragma warning disable 0168
using UnityEngine;

namespace Choanji
{
	public class ChoanjiInit : MonoBehaviour
	{
		void Start()
		{
			var _choanjiAwake = TheChoanji.g;
		}
	}
}
