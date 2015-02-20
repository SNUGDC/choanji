using System;
using Gem;
using LitJson;

namespace Choanji.Battle
{
	public enum TriggerWhereType
	{
		TRUE,
		CUR_DMG_TYPE,
	}

	public enum TriggerWhereComparison
	{
		EQUAL, NOT_EQUAL, 
	}

	public class TriggerWhere
	{
		public readonly TriggerWhereType type;

		public TriggerWhere(TriggerWhereType _type)
		{
			type = _type;
		}

		public static implicit operator TriggerWhereType(TriggerWhere _this)
		{
			return _this.type;
		}
	}

	public sealed class TriggerWhereEleType : TriggerWhere
	{
		public TriggerWhereComparison cmp;
		public ElementID ele;

		public TriggerWhereEleType(TriggerWhereType _type, JsonData _data)
			: base(_type)
		{
			JsonData _cmp;
			if (_data.TryGet("cmp", out _cmp))
				cmp = EnumHelper.ParseOrDefault<TriggerWhereComparison>((string)_cmp);

			ele = ElementDB.Search((string) _data["ele"]);
		}

		public bool Test(ElementID _ele)
		{
			switch (cmp)
			{
				case TriggerWhereComparison.EQUAL:
					return ele == _ele;
				case TriggerWhereComparison.NOT_EQUAL:
					return ele != _ele;
				default:
					D.Assert(false);
					return false;
			}
		}
	}

	public static partial class TriggerFactory
	{
		public static TriggerWhere MakeWhere(JsonData _data)
		{
			JsonData _typeJs;
			if (!_data.TryGet("type", out _typeJs))
				return null;

			var _type = EnumHelper.ParseOrDefault<TriggerWhereType>((string)_typeJs);

			switch (_type)
			{
				case TriggerWhereType.CUR_DMG_TYPE:
					return new TriggerWhereEleType(TriggerWhereType.CUR_DMG_TYPE, _data);
				default:
					return new TriggerWhere(_type);
			}
		}
	}
}