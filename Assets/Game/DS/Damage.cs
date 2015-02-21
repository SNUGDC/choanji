using System;
using System.Collections.Generic;
using System.Linq;
using Gem;

namespace Choanji
{
	public struct Damage
	{
		public ElementID ele;
		public HP val;

		public Damage(ElementID _ele, HP _val)
		{
			ele = _ele;
			val = _val;
		}

		public static implicit operator HP(Damage _this)
		{
			return _this.val;
		}
	}

	public class DamageBuilder
	{
		public enum Key {}

		private readonly LinkedList<KeyValuePair<Key, Func<Damage, Damage>>> mBuilders 
			= new LinkedList<KeyValuePair<Key, Func<Damage, Damage>>>();

		public Key Add(Func<Damage, Damage> _builder, PositionType _pos = PositionType.BACK)
		{
			var _key = (Key)MathHelper.RandPosInt();
			mBuilders.Add(new KeyValuePair<Key, Func<Damage, Damage>>(_key, _builder), _pos);
			return _key;
		}

		public void Remove(Key _key)
		{
			mBuilders.RemoveIf(_kv => _kv.Key == _key);
		}

		public Damage Build(Damage _dmg)
		{
			if (mBuilders.Empty()) return _dmg;
			_dmg = mBuilders.Aggregate(_dmg, (_current, _builder) => _builder.Value(_current));
			return _dmg;
		}
	}
}