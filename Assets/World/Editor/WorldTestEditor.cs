using UnityEditor;
using UnityEngine;

namespace Choanji
{
	[CustomEditor(typeof(WorldTest))]
	public class WorldTestEditor : Gem.Editor<WorldTest>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
				return;

			if (TheWorld.g == null)
				return;

			if (GUILayout.Button("update"))
				TheWorld.UpdateCam();
		}
	}


}