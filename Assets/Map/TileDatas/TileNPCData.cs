using System;
using Gem;

namespace Choanji
{
    [Serializable]
    public struct TileNPCData
    {
        public TileNPCData(string _key)
        {
	        id = NPCHelper.MakeID(_key);
            key = _key;
            dir = null;
            agent = CharacterAgentType.NONE;
	        dialog = null;
            battle = null;
        }

		public NPCID id;
		public string key;
        public Direction? dir;
        public CharacterAgentType agent;
	    public string dialog;
        public string battle;
    }
}