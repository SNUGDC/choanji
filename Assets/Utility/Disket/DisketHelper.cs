using Gem;
using UnityEngine;

namespace Choanji
{
	public static class DisketHelper
	{
		public static void SetupCommon()
		{
			D.Assert(Disket.isLoaded);
			var _data = Disket.data;

			Wallet.gold = _data.user.gold;
		}

		public static void SetupWorld()
		{
			D.Assert(Disket.isLoaded);
			var _data = Disket.data;

			TheWorld.Read(_data.world.key);

			if (TheCharacter.ch)
			{
				L.W("character already exists. destroy and reset.");
				var _chOld = TheCharacter.ch;
				TheCharacter.ch = null;
				Object.Destroy(_chOld.gameObject);
			}

			var _ch = PrefabDB.g.ch.Instantiate();
			_ch.position = _data.world.pos;
			_ch.renderer_.SetSkin(_data.ch.skin);

			TheCharacter.ch = _ch;
			TheWorld.UpdateRect(new PRect(_data.world.pos));
		}
	}

}