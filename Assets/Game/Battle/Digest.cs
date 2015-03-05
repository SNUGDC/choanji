using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;

namespace Choanji.Battle
{
	public enum DigestType
	{
		TURN_START,
		CARD_SELECT,
	}

	public static partial class Helper
	{
		public static List<string> ToList(string _descript)
		{
			return new List<string>{_descript};
		}
	}

	public class Invoker
	{
		public readonly Battler battler;
		public readonly Card card;
		public readonly CardMode? mode;

		public Invoker(Battler _battler = null, Card _card = null, CardMode? _mode = null)
		{
			battler = _battler;
			card = _card;
			mode = _mode;
		}

		public static implicit operator Battler(Invoker _this)
		{
			return _this.battler;
		}

		public static implicit operator Card(Invoker _this)
		{
			return _this.card;
		}
	}

	public class Digest
	{
		public readonly Invoker invoker;

		public string battlerName
		{
			get
			{
				if (invoker.battler != null)
				{
					return invoker.battler.name;
				}
				else
				{
					L.E("NO_BATTLER");
					return "NO_BATTLER";
				}
			}
		}

		public string otherName
		{
			get
			{
				if (invoker.battler != null)
				{
					return TheBattle.state.Other(invoker.battler).name;
				}
				else
				{
					L.E("NO_BATTLER");
					return "NO_BATTLER";
				}
			}
		}

		public string cardName
		{
			get
			{
				if (invoker.card != null)
				{
					return invoker.card.data.richName;
				}
				else
				{
					L.E("NO_CARD");
					return "NO_CARD";
				}
			}
		}

		public string moveName
		{
			get
			{
				if (!invoker.mode.HasValue)
				{
					L.E("NO_CARD_MODE");
					return "NO_CARD_MODE";
				}
				else if (invoker.mode.Value == CardMode.PASSIVE)
					return invoker.card.data.passive.richName;
				else
					return invoker.card.data.active.richName;
			}
		}

		public string passiveName
		{
			get
			{
				if (invoker.card != null)
				{
					return invoker.card.data.passive.richName;
				}
				else
				{
					L.E("NO_PASSIVE");
					return "NO_PASSIVE";
				}
			}
		}

		public string activeName
		{
			get
			{
				if (invoker.card != null)
				{
					return invoker.card.data.active.richName;
				}
				else
				{
					L.E("NO_ACTIVE");
					return "NO_ACTIVE";
				}
			}
		}

		public Digest(Invoker _invoker)
		{
			invoker = _invoker;
		}

		public virtual List<string> Descript() { return null; }
	}

	public sealed class TypedDigest : Digest
	{
		public readonly DigestType type;
		public readonly object arg;

		public TypedDigest(Invoker _invoker, DigestType _type, object _arg = null) 
			: base(_invoker)
		{
			type = _type;
			arg = _arg;
		}

		public static implicit operator DigestType(TypedDigest _this)
		{
			return _this.type;
		}
	}

	public sealed class StringDigest : Digest
	{
		private readonly List<string> mDescripts;

		public StringDigest(Invoker _invoker, string _txt)
			: base(_invoker)
		{
			mDescripts = new List<string> { _txt };
		}

		public StringDigest(Invoker _invoker, List<string> _txt)
			: base(_invoker)
		{
			mDescripts = _txt;
		}

		public override List<string> Descript()
		{
			return mDescripts;
		}
	}

	public sealed class ActiveFireDigest : Digest
	{
		public ActiveFireDigest(Invoker _invoker)
			: base(_invoker)
		{}

		public override List<string> Descript()
		{
			return Helper.ToList(battlerName + "은(는) " + activeName + "을(를) 사용했다!");
		}
	}

	public sealed class PassiveFireDigest : Digest
	{
		public PassiveFireDigest(Invoker _invoker)
			: base(_invoker)
		{ }

		public override List<string> Descript()
		{
			return Helper.ToList(battlerName + "의 " + cardName + " 효과발동! " + passiveName);
		}
	}

	public class PhaserDigest : Digest
	{
		public readonly PhaseResult result;

		public PhaserDigest(Invoker _invoker, PhaseResult _result)
			: base(_invoker)
		{
			result = _result;
		}

		public override List<string> Descript()
		{
			string _descript;

			switch (result)
			{
				case PhaseResult.PERFORM:
					// _descript = battlerName + "은(는) " + activeName + "을(를) 사용했다!";
					return null;
				case PhaseResult.NO_AP:
					_descript = battlerName + "은(는) " + activeName + "을(를) 사용하려 했지만 AP가 부족하다!";
					break;
				case PhaseResult.DONE:
					_descript = "턴종료";
					break;
				default:
					L.E("PHASER_UNHANDLED");
					_descript = "PHASER_UNHANDLED";
					break;
			}

			return new List<string> { _descript };
		}
	}

	public class DmgDigest : Digest
	{
		public bool hit;
		public bool block;
		public Damage? dmg;
		public HP hpAfter;

		public DmgDigest(Invoker _invoker)
			: base(_invoker)
		{}

		public override List<string> Descript()
		{
			string _descript;

			if (dmg.HasValue)
			{
				var _dmg = dmg.Value;
				var _ele = ElementDB.Get(_dmg.ele);
				_descript = "<size=16>" + battlerName + "의" + moveName + "</size>, " 
					+ "<size=24>" + _ele.name + "</size> 데미지 <color=#" + _ele.theme.ToHex() + ">" + _dmg.val + "</color>!";
			}
			else if (block)
			{
				_descript = otherName + "은(는) " + moveName + "를 <color=#00aa00>방어했다!</color>";
			}
			else
			{
				_descript = otherName + "은(는) " + moveName + "는 <color=#00aa00>피했다!</color>";
			}

			return new List<string>{_descript};
		}
	}

	public class HealDigest : Digest
	{
		public readonly HP after;
		public readonly HP? val;
		public readonly Percent? per;

		public HealDigest(Invoker _invoker, HP _after, HP _hp)
			: base(_invoker)
		{
			after = _after;
			val = _hp;
		}

		public HealDigest(Invoker _invoker, HP _after, Percent? _per)
			: base(_invoker)
		{
			after = _after;
			per = _per;
		}

		public override List<string> Descript()
		{
			if (val.HasValue)
				return new List<string> { moveName + ", 회복 +<color=#32cd32>" + val.Value + "</color>" };
			else
				return new List<string> { moveName + ", 회복 <color=#32cd32>" + per.Value + "%</color>" };
		}
	}

	public class APChangeDigest : Digest
	{
		public readonly AP after;

		public APChangeDigest(Invoker _invoker, AP _after) 
			: base(_invoker)
		{
			after = _after;
		}
	}

	public class APChargeDigest : Digest
	{
		public readonly AP charge;
		public readonly AP after;

		public APChargeDigest(Invoker _invoker, AP _charge, AP _after) 
			: base(_invoker)
		{
			charge = _charge;
			after = _after;
		}

		public override List<string> Descript()
		{
			return Helper.ToList(moveName + ", AP 충전 <color=#ffa500>" + charge + "</color>");
		}
	}

	public class StatModDigest : Digest
	{
		public readonly Battler target;
		public readonly StatSet stat;
		public readonly int? dur;

		public StatModDigest(Invoker _invoker, Battler _target, StatSet _stat, int? _dur) 
			: base(_invoker)
		{
			target = _target;
			stat = _stat;
			dur = _dur;
		}

		public override List<string> Descript()
		{
			var _descript = new List<string>();
			var _prefix = target.name + ", ";
			if (dur.HasValue)
				_prefix += dur.Value + "턴 동안 ";

			foreach (var _type in EnumHelper.GetValues<StatType>())
			{
				var _val = stat[_type];
				if (_val == 0) continue;
				var _buffOrNuff = _val > 0 ? "상승" : "하강";
				_descript.Add(_prefix + _type.Name() + "이(가) " 
					+ Math.Abs(_val) + " " + _buffOrNuff +"!");
			}

			foreach (var _ele in ElementDB.GetEnum())
			{
				var _rst = stat.GetRst(_ele);
				if (_rst == 0) continue;
				var _buffOrNuff = _rst > 0 ? "상승" : "하강";
				_descript.Add(_prefix + StatHelper.GetRstName(_ele) + "이(가) "
					+ Math.Abs((int)_rst) + " " + _buffOrNuff + "!");
			}

			return _descript;
		}
	}

	public class BuffAtkDigest : Digest
	{
		public readonly ElementID ele;
		public readonly Percent per;

		public BuffAtkDigest(Invoker _invoker, ElementID _ele, Percent _per) : base(_invoker)
		{
			ele = _ele;
			per = _per;
		}

		public override List<string> Descript()
		{
			var _buffOrNuff = per > 0 ? "강화" : "약화";
			return Helper.ToList(ElementDB.Get(ele).richName + "의 공격력이 " + per + "% " + _buffOrNuff + "된다.");
		}
	}
}