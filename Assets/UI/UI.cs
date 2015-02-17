using Gem;
using Gem.In;
using UnityEngine;

namespace Choanji.UI
{
	public static class UI
	{
		private static Canvas sCanvas;
		public static Canvas canvas
		{
			get { return sCanvas ?? (sCanvas = Object.FindObjectOfType<Canvas>()); }
		}

		#region key binding
		public static readonly InputGroup sBinds = new InputGroup(InputManager.g);

		public static void RegKey()
		{
			if (sBinds.count == 0)
			{
				sBinds.Add(new InputBind(InputCode.ESC, new InputHandler
				{
					down = delegate { CloseTopMenu(); return true; },
				}));

				sBinds.Add(new InputBind(InputCode.RET, new InputHandler
				{
					down = delegate {
						ToggleTopMenu();
						return true;
					},
				}));
			}

			sBinds.Reg();
		}

		public static void UnregKey()
		{
			sBinds.Unreg();
		}
		#endregion

		public static void Update(float _dt)
		{
			UpdateTopMenu(_dt);
		}

		#region top menu
		// note: top menu를 animation으로 하면 
		// 원치않는 동작을 보일 때가 많아 코드로 합니다.
		public const float TOP_MENU_SPEED = 8;

		public static bool isTopMenuOpened { get; private set; }
		public static bool sCanOpenTopMenu = true;
		private static bool? dirTopMenu;
		private static TopMenu sTopMenu;

		public static TopMenu OpenTopMenu()
		{
			if (!sCanOpenTopMenu)
				return null;

			if (isTopMenuOpened)
			{
				L.W("trying to open top menu again.");
				return sTopMenu;
			}

			isTopMenuOpened = true;
			dirTopMenu = true;

			if (sTopMenu)
			{
				sTopMenu.gameObject.SetActive(true);
				return sTopMenu;
			}

			sTopMenu = DB.g.topMenuPrf.Instantiate();
			var _trans = sTopMenu.transform;
			_trans.SetParent(canvas.transform);
			var _rect = ((RectTransform)_trans);
			_rect.Fill();
			_trans.Translate(0, _rect.H(), 0);

			sTopMenu.onPopupOpened += delegate
			{
				sCanOpenTopMenu = false;
				CloseTopMenu();
			};

			sTopMenu.onPopupClosed += delegate
			{
				sCanOpenTopMenu = true;
				OpenTopMenu();
			};

			return sTopMenu;
		}

		public static void CloseTopMenu()
		{
			if (!isTopMenuOpened)
			{
				L.W("trying to close top menu again.");
				return;
			}

			isTopMenuOpened = false;
			dirTopMenu = false;
		}

		public static void ToggleTopMenu()
		{
			if (!isTopMenuOpened)
				OpenTopMenu();
			else
				CloseTopMenu();
		}

		private static void UpdateTopMenu(float _dt)
		{
			if (dirTopMenu.HasValue)
			{
				var _rect = ((RectTransform) sTopMenu.transform);
				var _h = _rect.H();
				var _dy = TOP_MENU_SPEED * _h * _dt;

				if (dirTopMenu.Value)
				{
					_rect.Translate(0, -_dy, 0);
					if (_rect.MaxY() <= 0)
					{
						_rect.Translate(0, _rect.MaxY(), 0);
						dirTopMenu = null;
					}
				}
				else
				{
					_rect.Translate(0, _dy, 0);
					if (_rect.MaxY() > _h)
					{
						dirTopMenu = null;
						_rect.Translate(0, _h - _rect.MaxY(), 0);
						sTopMenu.gameObject.SetActive(false);
					}
				}
			}
		}
		#endregion

	}
}