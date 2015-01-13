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

		protected override bool DoStart(InspectRequest _data)
		{
			if (onInspect == null)
				return false;
			onInspect.CheckAndCall(_data);
			return true;
		}
	}
}