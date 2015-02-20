using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public partial class TAManager
	{
		private readonly List<TA> mTAs = new List<TA>();

		~TAManager()
		{
			Clear();
		}

		public TAHandle Add(Battler _battler, TA _ta)
		{
			mTAs.Add(_ta);
			Enable(_battler, _ta);
			return _ta;
		}

		public void Remove(TAHandle _handle)
		{
			var _ta = mTAs.FindAndRemoveIf(e => e == _handle);
			Disable(_ta);
		}

		public void Clear()
		{
			foreach (var _ta in mTAs)
				Disable(_ta);
			mTAs.Clear();
		}
	}
}