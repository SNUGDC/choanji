using System;
using LitJson;

namespace Choanji
{
	[Serializable]
	public enum NPCID { }

	public static class NPCHelper
	{
		public static NPCID MakeID(string _key)
		{
			return (NPCID)_key.GetHashCode();
		}
	}

	public class NPCData
	{
		public NPCData(JsonData _json)
		{
			skin = (CharacterSkins.Key)(int)_json["skin"];
		}

		public readonly CharacterSkins.Key skin;
	}

}