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
		public readonly Character ch;
		public readonly World world;

		public SaveData()
		{}

		public SaveData(User _user, Character _ch, World _world)
		{
			user = _user;
			ch = _ch;
			world = _world;
		}
	}
}