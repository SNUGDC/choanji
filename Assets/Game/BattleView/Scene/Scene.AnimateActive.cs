using System;
using Gem;

namespace Choanji.Battle
{
	public partial class Scene
	{
		private const float MSG_DELAY = 1;

		public void AnimateActive(ActionInvoker _invoker, ActionResult _result, Action _done)
		{
			var _delay = 0f;

			_delay += AnimateCommon(_invoker, _result);

			if (TheBattle.state.battlerA == _invoker.battler)
				_delay += AnimateA(_invoker, _result);
			else
				_delay += AnimateB(_invoker, _result);

			Timer.g.Add(_delay, _done);
		}

		private float AnimateCommon(ActionInvoker _invoker, ActionResult _result)
		{
			float _delay = 0;

			_delay += PushMessageUseActive(_invoker, _invoker.card.data.active);

			if (_result is ActionDmgResult)
			{
				var _theResult = (ActionDmgResult)_result;
				_delay += PushMessageDamage(_theResult);
			}

			return _delay;
		}

		private float AnimateA(ActionInvoker _invoker, ActionResult _result)
		{
			if (_result is ActionDmgResult)
			{
				var _theResult = (ActionDmgResult)_result;
				if (_theResult.dmg.HasValue)
				{
					var _dmg = _theResult.dmg.Value;
					Timer.g.Add(MSG_DELAY, () => poper.PopDmg(_dmg));
				}
				return 0;
			}

			return 0;
		}

		private float AnimateB(ActionInvoker _invoker, ActionResult _result)
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

		private float PushMessageDamage(ActionDmgResult _result)
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