using System;
using Gem;
using LitJson;
using Random = UnityEngine.Random;

namespace Choanji.Battle
{
	public enum ActionType
	{
		NONE = 0,
		LAMBDA,
		TA,
		DICE,
		DMG,
		HEAL,
		AVOID_HIT,
		AP_CHARGE,
		STAT_MOD,
		BUFF_ATK,
		SC_IMPOSE,
		SC_IMMUNE,
		SC_HEAL,
	}

	public enum TargetType
	{
		SELF = 0, OTHER,
	}

	public static class ActionHelper
	{
		public static bool Dice(Percent _prob)
		{
			if ((int)_prob >= 100) return true;
			return Random.Range(0, 100) < (int)_prob;
		}

		public static Battler Other(Battler _battler)
		{
			return TheBattle.state.Other(_battler);
		}

		public static Battler Choose(TargetType _target, Battler _self)
		{
			return _target == TargetType.SELF 
				? _self : TheBattle.state.Other(_self);
		}
	}

	public abstract class Action_
	{
		public readonly ActionType type;

		protected Action_(ActionType _type)
		{
			type = _type;
		}

		public static implicit operator ActionType(Action_ _this)
		{
			return _this.type;
		}

		public abstract Digest Invoke(Invoker _invoker, object _arg);
	}

	public sealed class ActionLambda : Action_
	{
		private readonly Func<Invoker, object, Digest> mAction;

		public ActionLambda(Func<Invoker, object, Digest> _action)
			: base(ActionType.LAMBDA)
		{
			mAction = _action;
		}

		public ActionLambda(Action_ _action)
			: this(_action.Invoke)
		{}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			return mAction(_invoker, _arg);
		}
	}

	public sealed class ActionTA : Action_
	{
		public readonly TA ta;

		public ActionTA(JsonData _data)
			: base(ActionType.TA)
		{
			ta = new TA(_data);
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			TheBattle.trigger.Add(_invoker, ta);
			return null;
		}
	}


	public sealed class ActionDice : Action_
	{
		public Percent prob;
		public Action_ action;

		public ActionDice(JsonData _data)
			: base(ActionType.DICE)
		{
			prob = (Percent)(int)_data["prob"];
			action = ActionFactory.Make(_data["action"]);
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			if (ActionHelper.Dice(prob))
				return TheBattle.trigger.Fire(_invoker, action, _arg);
			else 
				return null;
		}
	}

	public sealed class ActionDmg : Action_
	{
		public readonly Damage dmg;
		public readonly Percent accuracy;

		public ActionDmg(JsonData _data)
			: base(ActionType.DMG)
		{
			var _ele = ElementDB.Search((string)_data["ele"]);
			var _val = (HP)(int)_data["dmg"];
			dmg = new Damage(_ele, _val);
			accuracy = (Percent)_data.IntOrDefault("accuracy", 100);
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			var _battler = _invoker.battler;
			var _state = TheBattle.state;
			var _hitter = _state.Other(_battler);

			if (ActionHelper.Dice(accuracy))
			{
				var _dmg = _battler.attackBuilder.Build(dmg);

				_hitter.beforeHit.CheckAndCall(_dmg);

				if (_hitter.blockHitOneTime)
				{
					_hitter.blockHitOneTime = false;
					return new DmgDigest(_invoker)
					{
						hit = true,
						block = true,
						hpAfter = _hitter.hp,
					};
				}
				else
				{
					var _trueDmg = new Damage(_dmg.ele, _hitter.Hit(_dmg));
					_hitter.afterHit.CheckAndCall(_trueDmg);

					return new DmgDigest(_invoker)
					{
						hit = true,
						dmg = _trueDmg,
						hpAfter = _hitter.hp,
					};
				}
			}
			else
			{
				return new DmgDigest(_invoker)
				{
					hit = false,
					hpAfter = _hitter.hp,
				};
			}
		}
	}

	public sealed class ActionHeal : Action_
	{
		public readonly HP? val;
		public readonly Percent? per;

		public ActionHeal(JsonData _data)
			: base(ActionType.HEAL)
		{
			JsonData _val;

			if (_data.TryGet("val", out _val))
				val = (HP)(int) _val;
			else if (_data.TryGet("per", out _val))
				per = (Percent)(int)_val;
			else
			{
				D.Assert(false);
				val = 0;
			}
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			var _battler = _invoker.battler;
			if (val.HasValue)
			{
				_battler.Heal(val.Value);
				return new HealDigest(_invoker, val.Value);
			}
			else 
			{
				_battler.Heal(per.Value);
				return new HealDigest(_invoker, per.Value);
			}
		}
	}

	public sealed class ActionAvoidHit : Action_
	{
		public ActionAvoidHit() : base(ActionType.AVOID_HIT)
		{
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			_invoker.battler.blockHitOneTime = true;
			return null;
		}
	}

	public sealed class ActionAPCharge : Action_
	{
		public readonly AP val;

		public ActionAPCharge(JsonData _data)
			: base(ActionType.AP_CHARGE)
		{
			val = (AP)_data.IntOrDefault("val", 0);
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			var _after = _invoker.battler.ChargeAP(val);
			return new APChargeDigest(_invoker, val, _after);
		}
	}

	public sealed class ActionBuffEle : Action_
	{
		public readonly ElementID ele;
		public readonly Percent per;

		public ActionBuffEle(ActionType _type, JsonData _data)
			: base(_type)
		{
			ele = ElementDB.Search((string) _data["ele"]);
			per = (Percent)(int)_data["per"];
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			if (type == ActionType.BUFF_ATK)
			{
				_invoker.battler.attackModifier[ele] += (int)per;
				return new BuffAtkDigest(_invoker, ele, per);
			}

			return null;
		}
	}

	public sealed class ActionStatMod : Action_
	{
		public readonly TargetType target;
		public readonly StatSet stat;
		public readonly int? dur;

		public ActionStatMod(JsonData _data)
			: base(ActionType.STAT_MOD)
		{
			stat = new StatSet(_data);

			target = TargetType.SELF;
			JsonData _targetJs;
			if (_data.TryGet("target", out _targetJs))
				EnumHelper.TryParse((string) _targetJs, out target);

			JsonData _durJs;
			if (_data.TryGet("dur", out _durJs))
				dur = (int)_durJs;
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			var _target = ActionHelper.Choose(target, _invoker.battler);
			if (!dur.HasValue)
				_target.dynamicStat += stat;
			else
				_target.ApplyForDuration(stat, dur.Value);
			return new StatModDigest(_invoker, _target, stat, dur);
		}
	}

	public sealed class ActionSCImpose : Action_
	{
		public readonly SC sc;
		public readonly Percent accuracy;

		public ActionSCImpose(JsonData _data)
			: base(ActionType.SC_IMPOSE)
		{
			EnumHelper.TryParse((string)_data["sc"], out sc);
			accuracy = (Percent)_data.IntOrDefault("accuracy", 100);
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			if (ActionHelper.Other(_invoker.battler).TryImposeSC(sc, accuracy))
				return null;
			else
				return new StringDigest(_invoker, "상태이상 " + SCDB.g[sc].richName + "을(를) 거는데 실패하였다!");
		}
	}

	public sealed class ActionSCImmune : Action_
	{
		public readonly SC sc;

		public ActionSCImmune(JsonData _data)
			: base(ActionType.SC_IMMUNE)
		{
			EnumHelper.TryParse((string)_data["sc"], out sc);
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			_invoker.battler.AddImmune(sc);
			return new StringDigest(_invoker, "상태이상 " + SCDB.g[sc].richName + "에 면역되었다!");
		}
	}

	public sealed class ActionSCHeal : Action_
	{
		public readonly Percent accuracy;

		public ActionSCHeal(JsonData _data)
			: base(ActionType.SC_HEAL)
		{
			accuracy = (Percent)_data.IntOrDefault("accuracy", 100);
		}

		public override Digest Invoke(Invoker _invoker, object _arg)
		{
			if (ActionHelper.Dice(accuracy))
			{
				_invoker.battler.HealSC();
				return new StringDigest(_invoker, "상태이상을 치료하였다!");
			}
			else
			{
				return new StringDigest(_invoker, "상태이상을 치료하지 못했다!");
			}
		}
	}

	public static class ActionFactory
	{
		public static Action_ Make(JsonData _data)
		{
			var _type = EnumHelper.ParseOrDefault<ActionType>((string)_data["type"]);

			switch (_type)
			{
				case ActionType.TA:
					return new ActionTA(_data);
				case ActionType.DICE:
					return new ActionDice(_data);
				case ActionType.DMG:
					return new ActionDmg(_data);
				case ActionType.HEAL:
					return new ActionHeal(_data);
				case ActionType.AVOID_HIT:
					return new ActionAvoidHit();
				case ActionType.AP_CHARGE:
					return new ActionAPCharge(_data);
				case ActionType.STAT_MOD:
					return new ActionStatMod(_data);
				case ActionType.BUFF_ATK:
					return new ActionBuffEle(_type, _data);
				case ActionType.SC_IMPOSE:
					return new ActionSCImpose(_data);
				case ActionType.SC_IMMUNE:
					return new ActionSCImmune(_data);
				case ActionType.SC_HEAL:
					return new ActionSCHeal(_data);
				default:
					L.W(L.M.CASE_INVALID(_type));
					return null;
			}
		}
	}
}