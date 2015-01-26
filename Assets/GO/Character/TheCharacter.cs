using Gem;
using UnityEngine;

namespace Choanji
{
	public static class TheCharacter
	{
		private static Character mG;

		public static Character g
		{
			get { return mG; }
			set
			{
				if (g != null)
				{
					L.W(L.M.SHOULD_NULL("g"));
					Object.Destroy(g.GetComponent<CharacterInputAgent>());
				}

				mG = value;
				D.Assert(g.GetComponent<CharacterInputAgent>() == null);
				g.gameObject.AddComponent<CharacterInputAgent>();
			}
		}
	}

}