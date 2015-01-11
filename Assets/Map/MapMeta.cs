using System;
using Gem;

namespace Choanji
{
	[Serializable]
	public class MapMeta
	{
		public MapMeta(string _name)
		{
			name = _name;
			id = (MapID) name.GetHashCode();
		}

		public readonly MapID id;
		public readonly string name;
		public bool outdoor;
		public Point regionMapPosition;
		public bool showMilestone = false;
		public bool cameraBounded = false;
		public BGMList wildBattleBGM;
		public BGMList trainerBattleBGM;
		public BGMList wildVictoryBGM;
		public BGMList trainerVictoryBGM;
	}

}