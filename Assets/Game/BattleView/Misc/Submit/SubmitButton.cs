using System;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class SubmitButton : MonoBehaviour
	{
		public Animator animator;
		public Action onClick;

		public void Show()
		{
			animator.SetTrigger("show");
		}

		public void ChooseAndHide()
		{
			animator.SetTrigger("hide");
			SoundManager.PlaySFX(SoundDB.g.choose);
		}

		public void OnClick()
		{
			if (onClick != null)
				onClick.CheckAndCall();
		}
	}

}