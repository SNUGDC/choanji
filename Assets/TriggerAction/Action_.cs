using System;

namespace Choanji
{
	public abstract class Action_
	{
		public readonly ActionType type;

		public Action onDone;

		protected Action_(ActionType _type)
		{
			type = _type;
		}

		public abstract void Do(object _data);
	}
}