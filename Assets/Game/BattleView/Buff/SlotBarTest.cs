#if UNITY_EDITOR
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class SlotBarTest : MonoBehaviour
	{
		public SlotBar bar;

		public List<Sprite> icon;
		public Sprite frame;

		private int mLastTime;
		private readonly Timer mTimer = new Timer();

		void Update()
		{
			mTimer.Update(Time.deltaTime);

			if ((int) Time.time == mLastTime)
				return;

			mLastTime = (int) Time.time;

			var _key = bar.Push(new SlotData
			{
				icon = icon.Rand(),
				frame = frame, 
			});

			mTimer.Add(Random.Range(0.5f, 5f), 
				() => bar.Remove(_key));
		}
	}

}

#endif