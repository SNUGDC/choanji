using System.Collections.Generic;
using System.Diagnostics;
using Gem;
using UnityEngine;

namespace Choanji
{
	using Elements = ReadOnlyBitSlot<ElementID, ElementIDConverter>;

	[DebuggerDisplay("{key}")]
	public class ElementRaw
	{
		public readonly string key;
		public readonly string name;
		public readonly string theme;
		public readonly List<string> weak;
		public readonly List<string> resist;
		public readonly List<string> immune;

		public ElementRaw(CSVRow _row)
		{
			key = _row.Read();
			name = _row.Read();
			theme = _row.Read();
			_row.Read().Parse(out weak);
			_row.Read().Parse(out resist);
			_row.Read().Parse(out immune);
		}
	}

	[DebuggerDisplay("{key}")]
	public class ElementData
	{
		public ElementData(ElementRaw _raw, Dictionary<string, ElementID> _map)
		{
			_map.TryGet(_raw.key, out id);
			key = _raw.key;
			name = _raw.name;
			UnityHelper.TryParse(_raw.theme, out theme);
			weak = Map(_raw.weak, _map);
			resist = Map(_raw.resist, _map);
			immune = Map(_raw.immune, _map);
		}

		public readonly ElementID id;
		public readonly string key;
		public readonly string name;
		public readonly Color32 theme;
		public readonly Elements weak;
		public readonly Elements resist;
		public readonly Elements immune;

		public string richName
		{
			get { return new RichText(name).AddColor(theme); }
		}

		private Elements Map(List<string> _raws, Dictionary<string, ElementID> _map)
		{
			var _elements = new BitSlot<ElementID, ElementIDConverter>(ElementConst.MAX);
			foreach (var _raw in _raws)
			{
				ElementID _id;
				if (_map.TryGet(_raw, out _id))
					_elements.Add(_id);
			}
			return _elements;
		}

		public static implicit operator ElementID(ElementData _this)
		{
			return _this.id;
		}
	}
}