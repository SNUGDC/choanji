#if UNITY_EDITOR
using Gem;
using UnityEngine;

namespace Choanji
{
	public class DisketTest : MonoBehaviour
	{

		void Start()
		{
			if (!Disket.isLoaded)
			{
				Disket.LoadOrDefault("test");

				var _data = Disket.data;

				Wallet.gold = _data.user.gold;
				TheWorld.bluePrint = WorldBluePrint.Read(_data.world.key);

				if (TheCharacter.g)
				{
					var _chOld = TheCharacter.g;
					TheCharacter.g = null;
					Destroy(_chOld.gameObject);
				}

				var _ch = PrefabDB.g.ch.Instantiate();
				_ch.position = _data.world.pos;
				_ch.renderer_.SetSkin(_data.ch.skin);

				TheCharacter.g = _ch;
				TheWorld.UpdateRect(new PRect(_data.world.pos));

				Disket.Save();
			}
		}
	}
}

#endif