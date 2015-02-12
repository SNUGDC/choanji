using System;
using Gem;

namespace Choanji
{
    [Serializable]
    public struct TileNPCData
    {
        public TileNPCData(string _key)
        {
            key = _key;
            dir = null;
            agent = CharacterAgentType.NONE;
            battle = null;
        }

        public readonly string key;
        public Direction? dir;
        public CharacterAgentType agent;
        public string battle;
    }
}