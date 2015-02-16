using Choanji.UI;
using Gem;
using UnityEngine;

namespace Choanji
{
	public class UISetup : MonoBehaviour
	{
		void Start()
		{
			TheChoanji.g.context = ContextType.WORLD;
			UI.UI.RegKey();
			Popups.Setup();
			PopupHelper.RegKey();
			Destroy(this);
		}
	}
}