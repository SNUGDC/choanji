using System;
using Gem;
using LitJson;

namespace Choanji
{
	public class JsonInspectable : IInspectable
	{
		public JsonInspectable(JsonData _data)
		{
			data = _data;
		}

		public readonly JsonData data;

		public Action<InspectData> onInspect;

		public bool Inspect(InspectData _data)
		{
			if (onInspect == null)
				return false;
			onInspect.CheckAndCall(_data);
			return true;
		}
	}
}