using Gem;
using UnityEngine;

namespace Choanji
{
	public class WorldScene : MonoBehaviour
	{
		private static WorldScene mG;
		public static WorldScene g
		{
			get { return mG ?? (mG = FindObjectOfType<WorldScene>()); }
		}

		public GameObject world { get { return TheWorld.parent.gameObject; } }
		public GameObject ui;

		public void Setup()
		{
			TheChoanji.g.context = ContextType.WORLD;
			UI.UI.RegKey();
			UI.Popups.Setup();
			TheInput.world.Reg(new InputBind(InputCode.ESC,
				new InputHandler { down = () => ThePopup.Close(), }));
		}

		public void SetActive(bool _val)
		{
			gameObject.SetActive(_val);
			world.SetActive(_val);
			ui.SetActive(_val);
		}
	}
}