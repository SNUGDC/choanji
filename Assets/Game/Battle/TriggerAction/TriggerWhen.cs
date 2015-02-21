using System;
using Gem;
using LitJson;

namespace Choanji.Battle
{
	public enum TriggerWhenType
	{
		BATTLE_START,
		BEFORE_START_TURN,
		BEFORE_HIT,
		AFTER_HIT,
		DEAD,
	}

	public class TriggerWhen
	{
		public readonly TriggerWhenType type;

		public TriggerWhen(TriggerWhenType _type)
		{
			type = _type;
		}

		public static implicit operator TriggerWhenType(TriggerWhen _this)
		{
			return _this.type;
		}
	}

	public static partial class TriggerFactory
	{
		public static TriggerWhen MakeWhen(JsonData _data)
		{
			if (_data.IsString)
				return new TriggerWhen(EnumHelper.ParseOrDefault<TriggerWhenType>((string)_data));

			JsonData _typeJs;
			if (!_data.TryGet("type", out _typeJs))
				return null;

			var _type = EnumHelper.ParseOrDefault<TriggerWhenType>((string)_typeJs);

			switch (_type)
			{
				default:
					L.E("when " + _type + " is not handled.");
					return null;
			}
		}
	}
}