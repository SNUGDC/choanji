using System;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class SubmitButton : MonoBehaviour
	{
		public Action onClick;

		public void OnClick()
		{
			onClick.CheckAndCall();
		}
	}

}