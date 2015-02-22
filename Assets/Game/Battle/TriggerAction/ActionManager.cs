using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public class ActionManager 
	{
		public int count;

		private readonly Queue<ActionResult> mResults = new Queue<ActionResult>();

		public void Enq(ActionResult _result)
		{
			mResults.Enqueue(_result);
		}

		public ActionResult Deq()
		{
			return mResults.Dequeue();
		}

		public ActionResult Fire(ActionInvoker _invoker, Action_ _action, object _arg)
		{
			var _result = _action.Invoke(_invoker, _arg);
			if (_result != null) Enq(_result);
			return _result;
		}
	}
}