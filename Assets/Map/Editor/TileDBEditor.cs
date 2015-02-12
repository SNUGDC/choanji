using Gem;
using UnityEditor;
using UnityEngine;

namespace Choanji
{
    public class TileDBEditor : Editor<TileDB>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("build"))
            {
//                 AssetDatabase.LoadAssetAtPath("");
//                 target.ch = new Prefab<Character>();
            }
        }
    }
}