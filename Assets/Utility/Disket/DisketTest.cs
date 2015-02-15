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

				if (TheCharacter.ch)
				{
					var _chOld = TheCharacter.ch;
					TheCharacter.ch = null;
					Destroy(_chOld.gameObject);
				}

				var _ch = PrefabDB.g.ch.Instantiate();
				_ch.position = _data.world.pos;
				_ch.renderer_.SetSkin(_data.ch.skin);

				TheCharacter.ch = _ch;
				TheWorld.UpdateRect(new PRect(_data.world.pos));
			}
		}
	}
}

#endif