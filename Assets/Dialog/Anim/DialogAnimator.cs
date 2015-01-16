using Gem;
using UnityEngine;

namespace Choanji
{
	public class DialogAnimator : MonoBehaviour
	{
		public Dialog dialog;

		public GameObject animStrollDonePrf;

		private GameObject mAnimStrollDone;
	
		void Start ()
		{
			dialog.events.onStrollDone += OnStrollDone;
			dialog.events.onRebase += OnRebase;
		}

		void OnDestroy()
		{
			dialog.events.onStrollDone -= OnStrollDone;
			dialog.events.onRebase -= OnRebase;
		}

		void OnStrollDone(bool _end)
		{
			if (mAnimStrollDone)
			{
				L.E(L.M.CALL_RETRY("create anim stroll"));
				return;
			}

			mAnimStrollDone = animStrollDonePrf.Instantiate();
			mAnimStrollDone.transform.SetParent(transform, false);
		}

		void OnRebase()
		{
			if (!mAnimStrollDone)
			{
				L.E(L.M.CALL_INVALID);
				return;
			}

			Destroy(mAnimStrollDone);
		}
	}
}