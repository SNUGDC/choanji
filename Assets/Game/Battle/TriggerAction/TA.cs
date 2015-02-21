using Gem;
using LitJson;

namespace Choanji.Battle
{
	public enum TAHandle { }

	public class TA
	{
		public TA(JsonData _data)
		{
			handle = ++sAlloc;

			delay = _data.IntOrDefault("delay", 0);

			JsonData _triggerData;
			if (_data.TryGet("trigger", out _triggerData))
				trigger = new Trigger(_triggerData);

			action = ActionFactory.Make(_data["action"]);
		}

		private static TAHandle sAlloc;

		public readonly TAHandle handle;

		public readonly int delay;
		public readonly Trigger trigger;
		public readonly Action_ action;

		public static implicit operator TAHandle(TA _this)
		{
			return _this.handle;
		}
	}
}