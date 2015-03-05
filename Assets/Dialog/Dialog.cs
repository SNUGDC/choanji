using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji
{
	public class Dialog : MonoBehaviour
	{
		private const int MAX_LEN = 100;
		private const float STROLL_DELAY = 0.02f;
		
		public Text uiText;

		public readonly DialogEvents events = new DialogEvents();

		public string orgText
		{
			get { return mStroller.orgText; }
			set
			{
				if (mStroller.isSetuped)
				{
					if (!mStroller.isEnded)
						L.W("trying to set orgText while strolling.");
					mStroller.StopAndReset();
					events.onRebase.CheckAndCall();
				}

				if (value != null)
					mStroller.Start(value);
			}
		}

		private SawFloat mNextTimer
			= new SawFloat(STROLL_DELAY);

		private readonly RichTextStroller mStroller 
			= new RichTextStroller(MAX_LEN);

		void Update()
		{
			if (!mStroller.isSetuped || mStroller.isStrollDone)
				return;

			mNextTimer.Add(Time.deltaTime);

			if (!mNextTimer.isDefault)
				return;

			if (mStroller.Next())
				uiText.text = mStroller.text;

			if (mStroller.isStrollDone)
				events.onStrollDone.CheckAndCall(mStroller.isEnded);
		}

		public bool Rebase()
		{
			var _ret = mStroller.Rebase();
			if (_ret) events.onRebase.CheckAndCall();
			return _ret;
		}
	}

}