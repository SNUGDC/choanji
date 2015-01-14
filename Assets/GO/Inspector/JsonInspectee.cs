using System;
using Gem;
using LitJson;

namespace Choanji
{
	public sealed class JsonInspectee : IInspectee
	{
		public JsonInspectee(JsonData _data)
		{
			data = _data;
		}

		public readonly JsonData data;

		public Action<InspectRequest> onInspect;

		public override bool CanStart()
		{
			return base.CanStart() && (onInspect != null);
		}

		protected override void DoStart(InspectRequest _request)
		{
			onInspect.CheckAndCall(_request);
		}
	}
}