﻿using System;
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

		private WorldID mWorld;
		public WorldID world { get { return mWorld; } }

		public Point size;

		public bool outdoor;
		public Point regionMapPosition;
		public bool showMilestone = false;
		public bool cameraBounded = false;
		public BGMList wildBattleBGM;
		public BGMList trainerBattleBGM;
		public BGMList wildVictoryBGM;
		public BGMList trainerVictoryBGM;

		public void SetWorld(string _world)
		{
			D.Assert(world == 0);
			mWorld = (WorldID) _world.GetHashCode();
		}

		public static implicit operator MapID(MapMeta _this) { return _this.id; }
	}

}