using System.Collections.Generic;
using System.Linq;
using Gem;
using UnityEngine;
using UnityEngine.UI;

namespace Choanji.Battle
{

	public class MessageView : MonoBehaviour
	{
		private const float TXT_DUR = 0.5f;
		private const float ANIM_DUR = 0.2f;
		private const int INTV = 32;

		public struct Message
		{
			public List<string> txts;
			public bool manual;
		}

		public Text txt1;
		public Text txt2;
		public GameObject waiting;

		public enum Key { }
		private Key mNextKey;

		private struct KeyAndMessage
		{
			public KeyAndMessage(Key _key, Message _msg)
			{
				key = _key;
				msg = _msg;
			}

			public readonly Key key;
			public readonly Message msg;
		}

		private int mTxtIdx;
		private readonly LinkedList<KeyAndMessage> mMessages = new LinkedList<KeyAndMessage>();

		private float mStateElapsed;

		public bool isOccuping { get { return mOccupied; } }
		public bool isAnimating { get { return mAnimLower || mAnimUpper; } }

		private Text mOccupied;

		private Text mAnimLower;
		private Text mAnimUpper;

		void Update()
		{
			mStateElapsed += Time.deltaTime;

			if (isAnimating)
			{
				if (!UpdateAnim())
				{
					D.Assert(mOccupied == null);
					var _occupied = mAnimLower;
					mAnimLower = null;
					BeginOccupied(_occupied);
				}
			}
			else if (isOccuping)
			{
				UpdateOccupied();
			}
		}

		public Key Push(string _txt)
		{
			return Push(new Message { txts = new List<string> { _txt } });
		}

		public Key Push(Message _msg)
		{
			var _key = default(Key);
			if (!_msg.manual)
				_key = mNextKey++;

			mMessages.AddLast(new KeyAndMessage(_key, _msg));

			if (!isAnimating && !isOccuping)
			{
				txt1.text = mMessages.First.Value.msg.txts.First();
				BeginAnim(txt1, null);
			}

			return _key;
		}

		public void Ok(Key _key)
		{
			D.Assert(_key != default(Key));
		}

		private void BeginAnim(Text _lower, Text _upper)
		{
			D.Assert(!isAnimating);

			mStateElapsed = 0;
			mAnimLower = _lower;
			mAnimUpper = _upper;

			if (mAnimLower)
			{
				mAnimLower.gameObject.SetActive(true);
				mAnimLower.transform.SetLPosY(-INTV);
			}

			if (mAnimUpper)
			{
				mAnimUpper.gameObject.SetActive(true);
				mAnimUpper.transform.SetLPosY(0);
			}
		}

		private bool UpdateAnim()
		{
			var _tween = (mStateElapsed < ANIM_DUR)
					? (1 - Mathf.Cos(Mathf.PI * mStateElapsed / ANIM_DUR)) / 2.0f : 1;
			var _oy = INTV * _tween;

			if (mAnimUpper)
			{
				var _trans = mAnimUpper.transform;
				var _pos = _trans.localPosition;

				if (_pos.y < INTV - float.Epsilon)
				{
					_pos.y = _oy;
					_trans.localPosition = _pos;
				}
				else
				{
					mAnimUpper.gameObject.SetActive(false);
					mAnimUpper = null;
				}
			}

			if (mAnimLower)
			{
				var _trans = mAnimLower.transform;
				var _pos = _trans.localPosition;

				if (_pos.y < -float.Epsilon)
				{
					_pos.y = _oy - INTV;
					_trans.localPosition = _pos;
				}
				else
				{
					if (mAnimUpper != null)
					{
						mAnimUpper.gameObject.SetActive(false);
						mAnimUpper = null;
					}

					return false;
				}
			}

			return true;
		}

		private void BeginOccupied(Text _text)
		{
			D.Assert(!isAnimating);
			D.Assert(!isOccuping);
			D.Assert(!mMessages.Empty());

			mStateElapsed = 0;
			mOccupied = _text;
			
			var _kv = mMessages.First();
			var _msg = _kv.msg;

			if (_msg.manual && (mTxtIdx == _msg.txts.Count - 1))
			{
				waiting.SetActive(true);
			}
		}

		private void UpdateOccupied()
		{
			var _kv = mMessages.First();
			var _msg = _kv.msg;

			if (_msg.manual && (mTxtIdx == _msg.txts.Count - 1))
				return;

			if (mStateElapsed < TXT_DUR)
				return;

			var _upper = mOccupied;
			mOccupied = null;

			if (mTxtIdx == _msg.txts.Count - 1)
			{
				mTxtIdx = 0;
				mMessages.RemoveFirst();
			}
			else
			{
				++mTxtIdx;
			}

			if (!mMessages.Empty())
			{
				var _lower = (txt1 != _upper) ? txt1 : txt2;
				_lower.text = mMessages.First.Value.msg.txts[mTxtIdx];
				BeginAnim(_lower, _upper);
			}
			else
			{
				BeginAnim(null, _upper);
			}
		}
	}


}