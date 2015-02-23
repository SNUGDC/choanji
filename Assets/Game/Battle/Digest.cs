using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
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

		public Invoker(Battler _battler, Card _card, CardMode? _mode)
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

	public class StringDigest : Digest
	{
		private readonly List<string> mDescription;

		public StringDigest(Invoker _invoker, string _txt)
			: base(_invoker)
		{
			mDescription = new List<string> { _txt };
		}

		public StringDigest(Invoker _invoker, List<string> _txt)
			: base(_invoker)
		{
			mDescription = _txt;
		}

		public override List<string> Descript()
		{
			return mDescription;
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
					_descript = battlerName + "은(는) " + activeName + "을(를) 사용했다!";
					break;
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

		public DmgDigest(Invoker _invoker)
			: base(_invoker)
		{ }
	}

	public class HealDigest : Digest
	{
		public readonly HP? val;
		public readonly Percent? per;

		public HealDigest(Invoker _invoker, HP _hp)
			: base(_invoker)
		{
			val = _hp;
		}

		public HealDigest(Invoker _invoker, Percent? _per)
			: base(_invoker)
		{
			per = _per;
		}

		public override List<string> Descript()
		{
			if (val.HasValue)
				return new List<string> { "회복 +" + val.Value };
			else
				return new List<string> { "회복 " + per.Value + "%" };
		}
	}
}