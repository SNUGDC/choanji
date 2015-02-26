using System.Diagnostics;
using Gem;
using LitJson;
using UnityEngine;

namespace Choanji
{
	[DebuggerDisplay("{key}")]
	public class PassiveData
	{
		public static readonly Color32 DEFAULT_THEME = new Color32(30, 144, 255, 255);

		public PassiveData(string _key, JsonData _data)
		{
			id = (PassiveID)HashEnsure.Do(_key);
			key = _key;

			string _usage;
			if (_data.TryGet("usage", out _usage))
				usage = EnumHelper.ParseOrDefault<CardUsage>(_usage);

			name = (string) _data["name"];
			detail = (string) _data["detail"];
			theme = _data.StringOrDefault("theme", DEFAULT_THEME.ToHex());
			perform = new Battle.TA(_data["perform"]);
		}

		public readonly PassiveID id;
		public readonly string key;
		public readonly CardUsage usage;
		public readonly string name;
		public readonly string detail;
		public readonly string theme;
		public readonly Battle.TA perform;

		public string richName { get { return UIHelper.RichAddColor(name, theme); } }

		public static implicit operator PassiveID(PassiveData _this)
		{
			return _this.id;
		}
	}

}