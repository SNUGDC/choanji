using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

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
			else if (_digest is ActiveFireDigest)
			{
				var _msgDelay = AnimateDescript(_digest);
				var _invoker = _digest.invoker;
				return AnimateActive(_invoker.battler, _invoker.card.data.active);
			}
			else if (_digest is DmgDigest)
			{
				var _isA = TheBattle.state.IsA(_digest.invoker);
				var _msgDelay = AnimateDescript(_digest);

				var _delay = 0f;

				var d = (DmgDigest) _digest;
				if (d.dmg.HasValue)
				{
					field.enemyParent.Play(FXDB.g.enemyHit);

					mTimer.Add(_delay += UNIT, () =>
					{
						if (_isA)
							poper.PopDmg(d.dmg.Value); 
						else
							hp.Hit(d.hpAfter);

						SoundManager.PlaySFX(SoundDB.g.hit);
					});
				}
				else
				{
					if (_isA)
					{
						_delay = _msgDelay;

						if (d.block)
						{
							poper.PopText(new RichText("방어!").AddSize(36).AddColor(Color.blue));
						}
						else
						{
							field.enemyParent.Play(FXDB.g.enemyMiss);

							poper.PopText(new RichText("회피!").AddSize(36).AddColor(Color.red));
						}
					}
				}

				return _delay;
			}
			else if (_digest is HealDigest)
			{
				var d = (HealDigest)_digest;
				hp.Heal(d.after);
				return AnimateDescript(_digest);
			}
			else if (_digest is APChangeDigest)
			{
				var _isA = TheBattle.state.IsA(_digest.invoker);
				if (_isA)
				{
					var d = (APChangeDigest)_digest;
					ap.Set((float)d.after, true);
				}
				return 0;
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
	}
}