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

			JsonData _limit;
			if (_data.TryGet("limit", out _limit))
				limit = (int)_limit;

			delay = _data.IntOrDefault("delay");

			JsonData _triggerData;
			if (_data.TryGet("trigger", out _triggerData))
				trigger = new Trigger(_triggerData);

			action = ActionFactory.Make(_data["action"]);
		}

		private static TAHandle sAlloc;

		public readonly TAHandle handle;

		public readonly int? limit;
		public readonly int delay;
		public readonly Trigger trigger;
		public readonly Action_ action;

		public static implicit operator TAHandle(TA _this)
		{
			return _this.handle;
		}
	}
}