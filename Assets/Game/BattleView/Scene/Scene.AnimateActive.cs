using System;
using Choanji.UI;
using Gem;

namespace Choanji.Battle
{
	public partial class Scene
	{
		private const float MSG_DELAY = 1;

		private static string BattlerName(Battler _battler)
		{
			return _battler.data.name;
		}

		public float Animate(Digest _digest)
		{
			var _descript = _digest.Descript();
			if (_descript == null || _descript.Empty())
				return 0;
			msg.Push(new MessageView.Message {txts = _descript});
			return _descript.Count*MSG_DELAY;
		}

		private float AnimateCommon(Digest _digest)
		{
			return 0;
// 			var _invoker = _digest.invoker;
// 
// 			var _delay = 0f;
// 
// 			if (_invoker != null)
// 				_delay += PushMessageUseActive(_invoker, _invoker.card.data.active);
// 
// 			if (_digest is DmgDigest)
// 			{
// 				var _theResult = (DmgDigest)_digest;
// 				_delay += PushMessageDamage(_theResult);
// 			}
// 
// 			return _delay;
		}

		private float AnimateBattler()
		{
			return 0;
// 			if (TheBattle.state.IsA(_digest.invoker.battler))
// 				return AnimateBattlerA(_digest);
// 			else
// 				return AnimateBattlerB(_digest);
		}

		private float AnimateBattlerA(Digest _digest)
		{
			if (_digest is DmgDigest)
			{
				var _theResult = (DmgDigest)_digest;
				if (_theResult.dmg.HasValue)
				{
					var _dmg = _theResult.dmg.Value;
					Timer.g.Add(MSG_DELAY, () => poper.PopDmg(_dmg));
				}
				return 0;
			}

			return 0;
		}

		private float AnimateBattlerB(Digest _result)
		{
			return 0;
		}

		public void AnimateBattleEnd(Result _result, Action _onDone)
		{
			Popup _popup = null;

			switch (_result.type)
			{
				case ResultType.WIN_A:
					_popup = ThePopup.Open(Popups.BATTLE_WIN);
					break;
				case ResultType.WIN_B:
					_popup = ThePopup.Open(Popups.BATTLE_LOSE);
					break;
			}

			if (_popup)
				_popup.onClose += _onDone;
		}

		private float PushMessageDamage(DmgDigest _digest)
		{
			if (_digest.dmg.HasValue)
			{
				var _dmg = _digest.dmg.Value;

				var _ele = ElementDB.Get(_dmg.ele);
				msg.Push("<size=24>" + _ele.name + "</size> 데미지 <color=#" + _ele.theme.ToHex() + ">" + _dmg.val + "</color>!" );
			}
			else if (_digest.block)
			{
				// todo: implement
				// poper.PopText("BLOCK", Color.blue);
				msg.Push("방어!");
			}
			else
			{
				// todo: implement
				// poper.PopText("MISS", Color.red);
				msg.Push("빗나감!");
			}

			return MSG_DELAY;
		}
	}
}