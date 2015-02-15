using System;
using Gem;

namespace Choanji
{
	public class Trigger
	{
		public readonly TriggerType type;
		public Action<object> onTrigger;

		private bool mIsEnabled;
		public bool isEnabled
		{
			get { return mIsEnabled; }
			set
			{
				if (isEnabled == value)
				{
					L.W("trying to set again.");
					return;
				}

				mIsEnabled = value;

				if (isEnabled)
					DoEnable();
				else
					DoDisable();
			}
		}

		protected Trigger(TriggerType _type)
		{
			type = _type;
		}

		~Trigger()
		{
			if (isEnabled) 
				isEnabled = false;
		}

		public void Invoke(object _data)
		{
			D.Assert(isEnabled);
			onTrigger.CheckAndCall(_data);
		}

		protected virtual void DoEnable() {}
		protected virtual void DoDisable() {}
	}
}