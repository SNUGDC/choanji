using Choanji.UI;
using Gem;
using UnityEngine;

namespace Choanji
{
	public class UISetup : MonoBehaviour
	{
		private static readonly InputBind sBindClose = new InputBind(InputCode.ESC,
			new InputHandler { down = () => ThePopup.Close(), });

		void Start()
		{
			TheChoanji.g.context = ContextType.WORLD;
			UI.UI.RegKey();
			Popups.Setup();
			TheInput.world.Reg(sBindClose);
			Destroy(this);
		}
	}
}