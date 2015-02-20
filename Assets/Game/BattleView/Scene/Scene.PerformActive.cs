using System;
using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public partial class Scene
	{
		public void Perform(Battler _battler, Card _card, PerformResult _result, Action _done)
		{
			if (TheBattle.state.battlerA == _battler)
				PerformA(_battler, _card, _result, _done);
			else
				PerformB(_battler, _card, _result, _done);
		}

		private void PerformA(Battler _battler, Card _card, PerformResult _result, Action _done)
		{
			msg.Push(new MessageView.Message
			{
				txts = new List<string> { "플레이어는 " + _card.data.name + "을(를) 사용했다!" }
			});

			if (_result is PerformDmgResult)
			{
				var _theResult = (PerformDmgResult)_result;
				if (_theResult.hit)
				{
					var _dmg = _theResult.dmg.Value;
					poper.PopDmg(_dmg);

					var _ele = ElementDB.Get(_dmg.ele);
					msg.Push(new MessageView.Message
					{
						txts = new List<string>{ "<size=24>" + _ele.name + "</size> 데미지 <color=#" + _ele.theme.ToHex() + ">" + _dmg.val + "</color>!" }
					});
				}
				else
				{
					// todo: implement
					// poper.PopText()
					msg.Push(new MessageView.Message
					{
						txts = new List<string>{ "빗나감!" },
					});
				}

				Timer.g.Add(1, _done);
			}
		}

		private void PerformB(Battler _battler, Card _card, PerformResult _result, Action _done)
		{
			msg.Push(new MessageView.Message
			{
				txts = new List<string> { _battler.data.name + "은(는) " + _card.data.name + "을(를) 사용했다!" }
			});

			Timer.g.Add(1, _done);
		}
	}
}