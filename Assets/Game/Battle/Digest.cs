using System.Collections.Generic;
using Choanji.Battle;

namespace Choanji
{
	public struct Invoker
	{
		public Battler battler;
		public Card card;

		public Invoker(Battler _battler, Card _card)
		{
			battler = _battler;
			card = _card;
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