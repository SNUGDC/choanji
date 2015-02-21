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

	public class TriggerWhenTarget : TriggerWhen
	{
		public readonly TargetType target;
		
		public TriggerWhenTarget(TriggerWhenType _type, JsonData _data)
			: base(_type)
		{
			JsonData _targetJs;
			if (_data.TryGet("target", out _targetJs))
				EnumHelper.TryParse((string) _targetJs, out target);
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
				case TriggerWhenType.AFTER_HIT:
					return new TriggerWhenTarget(_type, _data);

				default:
					L.E("when " + _type + " is not handled.");
					return null;
			}
		}
	}
}