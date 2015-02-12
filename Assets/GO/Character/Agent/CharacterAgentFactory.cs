using Gem;
using UnityEngine;

namespace Choanji
{

	public static class CharacterAgentFactory
	{
		public static Component Add(CharacterAgentType _type, CharacterCtrl _ch)
		{
			switch (_type)
			{
				case CharacterAgentType.RAND:
					return _ch.AddComponent<CharacterRandomAgent>();

				default:
					L.E(L.M.CASE_INVALID(_type));
					return null;
			}
		}
	}

}