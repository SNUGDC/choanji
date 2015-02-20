using System;
using Gem;

namespace Choanji.Battle
{
	public partial class Scene
	{
		private const float MSG_DELAY = 1;

		public void AnimateActive(Battler _battler, Card _card, PerformResult _result, Action _done)
		{
			var _delay = 0f;

			_delay += AnimateCommon(_battler, _card, _result);

			if (TheBattle.state.battlerA == _battler)
				_delay += AnimateA(_battler, _card, _result);
			else
				_delay += AnimateB(_battler, _card, _result);

			Timer.g.Add(_delay, _done);
		}

		private float AnimateCommon(Battler _battler, Card _card, PerformResult _result)
		{
			float _delay = 0;

			_delay += PushMessageUseActive(_battler, _card.data.active);

			if (_result is PerformDmgResult)
			{
				var _theResult = (PerformDmgResult)_result;
				_delay += PushMessageDamage(_theResult);
			}

			return _delay;
		}

		private float AnimateA(Battler _battler, Card _card, PerformResult _result)
		{
			if (_result is PerformDmgResult)
			{
				var _theResult = (PerformDmgResult)_result;
				if (_theResult.dmg.HasValue)
				{
					var _dmg = _theResult.dmg.Value;
					Timer.g.Add(MSG_DELAY, () => poper.PopDmg(_dmg));
				}
				return 0;
			}

			return 0;
		}

		private float AnimateB(Battler _battler, Card _card, PerformResult _result)
		{
			return 0;
		}

		private static string BattlerName(Battler _battler)
		{
			return _battler.data.name;
		}

		private float PushMessageUseActive(Battler _battler, ActiveData _active)
		{
			msg.Push(BattlerName(_battler) + "은(는) " + _active.name + "을(를) 사용했다!");
			return MSG_DELAY;
		}

		private float PushMessageDamage(PerformDmgResult _result)
		{
			if (_result.dmg.HasValue)
			{
				var _dmg = _result.dmg.Value;

				var _ele = ElementDB.Get(_dmg.ele);
				msg.Push("<size=24>" + _ele.name + "</size> 데미지 <color=#" + _ele.theme.ToHex() + ">" + _dmg.val + "</color>!" );
			}
			else if (_result.block)
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