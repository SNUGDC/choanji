using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public class SlotBar : MonoBehaviour
	{
		private const float INTV = 32;
		private const float MARGIN = 6;
		private const float DISPOSE_DUR = 0.5f;
		private const float DISPOSE_Y = 32;

		public enum Key {}

		public Slot slotPrefab;

		private Key mNextKey = (Key) 1;

		private class SlotState
		{
			public Key key;
			public int idx;
			public Slot slot;
			public bool anim;
		}

		private readonly List<SlotState> mSlots = new List<SlotState>();

		private class SlotDispose
		{
			public Slot slot;
			public float elapsed;
		}

		private readonly List<SlotDispose> mDisposing = new List<SlotDispose>();

		private static float SlotX(int _idx)
		{
			return (INTV + MARGIN)*_idx;
		}

		void Update()
		{
			foreach (var _slot in mSlots)
			{
				if (_slot.anim)
				{
					var _go = _slot.slot;
					var s = _go.transform.localPosition.x;
					var d = SlotX(_slot.idx);
					var x = Mathf.Lerp(s, d, 10 * Time.deltaTime);
					if (x - d > 0.5f)
						_go.transform.SetLPosX(x);
					else
					{
						_slot.anim = false;
						_go.transform.SetLPosX(d);
					}
				}
			}

			foreach (var _dispose in mDisposing)
			{
				_dispose.elapsed += Time.deltaTime;
				var _progress = _dispose.elapsed / DISPOSE_DUR;

				if (_progress > 1)
				{
					Destroy(_dispose.slot.gameObject);
					_dispose.slot = null;
				}
				else
				{
					var _slot = _dispose.slot;
					_slot.icon.SetA(1 - _progress);
					_slot.transform.SetLPosY(_progress * DISPOSE_Y);
				}
			}

			mDisposing.RemoveAll(_slot => _slot.slot == null);
		}

		public Key Push(SlotData _data)
		{
			var _slot = slotPrefab.Instantiate();
			_slot.SetData(_data);
			_slot.transform.SetParent(transform);
			_slot.transform.localPosition = new Vector2(SlotX(mSlots.Count), 0);

			mSlots.Add(new SlotState
			{
				key = mNextKey,
				idx = mSlots.Count,
				slot = _slot,
				anim = true,
			});

			return mNextKey++;
		}

		public void Remove(Key _key)
		{
			var _slotState = mSlots.FindAndRemoveIf(_slot => _slot.key == _key);

			if (_slotState == null)
			{
				L.E(L.M.KEY_NOT_EXISTS(_key));
				return;
			}

			var _i = _slotState.idx;
			for (; _i != mSlots.Count; ++_i)
			{
				var _slot = mSlots[_i];
				_slot.idx = _i;
				_slot.anim = true;
			}

			mDisposing.Add(new SlotDispose
			{
				slot = _slotState.slot,
			});
		}
	}

}