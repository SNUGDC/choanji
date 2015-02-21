using Gem;
using UnityEditor;
using UnityEngine;

namespace Choanji
{
	[CustomEditor(typeof(WorldCamera))]
	public class WorldCameraEditor : Editor
	{
		private WorldCamera mThis;

		void OnEnable()
		{
			mThis = (WorldCamera)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var _cam = mThis.camera;
			if (!_cam) return;

			GUILayout.Label("aspect: " + _cam.aspect);
			GUILayout.Label("size: " + _cam.OrthoHSize());
		}
	}


}