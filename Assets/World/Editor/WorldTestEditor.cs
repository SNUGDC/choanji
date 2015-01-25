using UnityEditor;
using UnityEngine;

namespace Choanji
{
	[CustomEditor(typeof(WorldTest))]
	public class WorldTestEditor : Editor
	{
		private WorldTest mThis;

		void OnEnabled()
		{
			mThis = (WorldTest) target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (TheWorld.world == null)
				return;

			if (GUILayout.Button("update"))
				TheWorld.Update();
		}
	}


}