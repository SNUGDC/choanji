using System.Collections.Generic;
using Gem;
using UnityEditor;
using UnityEngine;

namespace Choanji.Battle
{
	[CustomEditor(typeof(SCDB))]
	public class SCDBEditor : Editor<SCDB>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("build"))
			{
				var _list = new List<SCData>(target.db.Count);

				foreach (var _data in target.db)
				{
					if (_data.uiIcon == null)
					{
						var _iconPath = "Assets/R/BattleUI/SC/" + "SC_" + _data.type + ".png";
						_data.uiIcon = AssetDatabase.LoadAssetAtPath(_iconPath, typeof(Sprite)) as Sprite;
					}

					var _idx = (int) _data.type;
					if (_idx >= _list.Count)
						_list.Resize(_idx + 1);
					_list[_idx] = _data;
				}

				target.db = _list;
			}
		}
	}

}