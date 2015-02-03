using Gem;

namespace Choanji
{
	public class PassiveData
	{
		PassiveData(string _name, string _detail)
		{
			id = (PassiveID) HashEnsure.Do(_name);
			name = _name;
			detail = _detail;
		}

		public readonly PassiveID id;
		public readonly string name;
		public readonly string detail;
	}

}