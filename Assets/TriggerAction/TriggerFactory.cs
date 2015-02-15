using Gem;
using LitJson;

namespace Choanji
{
	public static class TriggerFactory 
	{
		public static Trigger Make(TriggerType _type, LocalCoor _position, JsonData _data)
		{
			switch (_type)
			{
				case TriggerType.ENABLE:
					return new TriggerEnable();
				case TriggerType.INSPECT:
					return new TriggerInspect(_position);
				default:
					D.Assert(false);
					return null;
			}
		}
	}


}