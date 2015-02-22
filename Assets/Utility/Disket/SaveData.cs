using LitJson;

namespace Choanji
{
	public class SaveData
	{
		public struct User
		{
			public Gold gold;
		}

		public struct Character
		{
			public CharacterSkins.Key skin;
		}

		public struct World
		{
			public string key;
			public Coor pos;
		}

		public readonly User user;
		public readonly JsonData player;
		public readonly Character ch;
		public readonly World world;
		public readonly JsonData data;

		public SaveData()
		{}

		public SaveData(User _user, JsonData _player, Character _ch, World _world, JsonData _data)
		{
			user = _user;
			player = _player;
			ch = _ch;
			world = _world;
			data = _data;
		}
	}
}