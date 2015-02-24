using System;
using System.Collections.Generic;
using Choanji.UI;
using Gem;

namespace Choanji.Battle
{
	public partial class Scene
	{
		private const float UNIT = 0.5f;
		private const float MSG_DELAY = 1;

		private static string BattlerName(Battler _battler)
		{
			return _battler.data.name;
		}

		public float Animate(Digest _digest)
		{
			if (_digest is TypedDigest)
			{
				var d = (TypedDigest) _digest;
				switch (d.type) 
				{
					case DigestType.TURN_START:
					{
						var _fx = FXDB.g.phaseDone.Instantiate();
						_fx.transform.SetParent(canvas.transform, false);
						return _fx.GetComponent<PropertyFloat>().value;
					}
					case DigestType.CARD_SELECT:
						GatherCards(((Action<List<Card>>)d.arg));
						return 0;
					default:
						return 0;
				}
			}
			else if (_digest is DmgDigest)
			{
				AnimateDescript(_digest);
				var _delay = 0f;
				var d = (DmgDigest) _digest;
				if (d.dmg.HasValue)
				{
					mTimer.Add(_delay += UNIT, () =>
					{
						hp.Hit(d.hpAfter);
						poper.PopDmg(d.dmg.Value);
						SoundManager.PlaySFX(SoundDB.g.hit);
					});
				}
				return _delay;
			}

			return AnimateDescript(_digest);
		}

		public float AnimateDescript(Digest _digest)
		{
			var _descript = _digest.Descript();
			if (_descript == null || _descript.Empty())
				return 0;
			msg.Push(new MessageView.Message {txts = _descript});
			return _descript.Count*MSG_DELAY;
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
	}
}