using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public class ActionManager 
	{
		public int count;

		private readonly Queue<Digest> mResults = new Queue<Digest>();

		public void Enq(Digest _result)
		{
			mResults.Enqueue(_result);
		}

		public Digest Deq()
		{
			return mResults.Dequeue();
		}

		public Digest Fire(Invoker _invoker, Action_ _action, object _arg)
		{
			var _result = _action.Invoke(_invoker, _arg);
			if (_result != null) Enq(_result);
			return _result;
		}
	}
}