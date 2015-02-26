using System.Diagnostics;
using Gem;
using LitJson;
using UnityEngine;

namespace Choanji
{
	[DebuggerDisplay("{key}")]
	public class ActiveData
	{
		public static readonly Color32 DEFAULT_THEME = new Color32(220, 20, 60, 255);

		public ActiveData(string _key, JsonData _data)
		{
			id = (ActiveID)HashEnsure.Do(_key);
			key = _key;

			string _usage;
			if (_data.TryGet("usage", out _usage))
				usage = EnumHelper.ParseOrDefault<CardUsage>(_usage);

			name = (string)_data["name"];
			detail = (string) _data["detail"];
			theme = _data.StringOrDefault("theme", DEFAULT_THEME.ToHex());
			cost = (AP) (int) _data["cost"];
			perform = new Battle.TA(_data["perform"]);
		}

		public readonly ActiveID id;
		public readonly string key;
		public readonly CardUsage usage;
		public readonly string name;
		public readonly string detail;
		public readonly string theme;

		public string richName { get { return UIHelper.RichAddColor(name, theme); }}

		public readonly AP cost;

		public readonly Battle.TA perform;

		public static implicit operator ActiveID(ActiveData _this)
		{
			return _this.id;
		}
	}
}