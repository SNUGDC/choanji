using System;
using Gem;

namespace Choanji
{
	[Serializable]
	public class MapMeta
	{
		public MapMeta(string _name)
		{
			id = MapIDHelper.Make(_name);
			name = _name;
		}

		public readonly MapID id;
		public readonly string name;

		public Point size;

		public bool outdoor;
		public Point regionMapPosition;
		public bool showMilestone = false;
		public bool cameraBounded = false;
		public BGMList wildBattleBGM;
		public BGMList trainerBattleBGM;
		public BGMList wildVictoryBGM;
		public BGMList trainerVictoryBGM;

		public static implicit operator MapID(MapMeta _this) { return _this.id; }

		public override int GetHashCode()
		{
			return (int) id;
		}
	}

}