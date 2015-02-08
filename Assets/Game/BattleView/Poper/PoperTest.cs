#if UNITY_EDITOR

using System.Collections;
using UnityEngine;

namespace Choanji.Battle
{
	public class PoperTest : MonoBehaviour
	{
		public Poper poper;

		void Start()
		{
			StartCoroutine("PopDmg");
		}

		IEnumerator PopDmg()
		{
			foreach (var _ele in ElementDB.GetEnum())
			{
				yield return new WaitForSeconds(1);
				poper.PopDmg(new Damage { ele = _ele, val = Random.Range(50, 300) });
			}
		}
	}

}

#endif