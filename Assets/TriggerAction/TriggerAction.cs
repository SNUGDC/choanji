namespace Choanji
{
	public class TriggerAction
	{
		public readonly TriggerActionID id;
		public readonly TriggerActionMode mode;
		public readonly Trigger trigger;
		public readonly Action_ action;

		public bool isEnabled
		{
			get { return trigger.isEnabled; }
			set { trigger.isEnabled = value; }
		}

		public TriggerAction(TriggerActionID _id, Trigger _trigger, Action_ _action, TriggerActionMode _mode)
			: this(_trigger, _action, _mode)
		{
			id = _id;
		}

		public TriggerAction(Trigger _trigger, Action_ _action, TriggerActionMode _mode)
		{
			trigger = _trigger;
			action = _action;
			mode = _mode;
			trigger.onTrigger = onTrigger;
		}

		~TriggerAction()
		{
			trigger.onTrigger -= onTrigger;
		}

		private void onTrigger(object _data)
		{
			action.Do(_data);
			if (mode == TriggerActionMode.ONCE)
				trigger.isEnabled = false;
		}
	}
}