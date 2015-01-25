using Gem;
using UnityEditor;
using UnityEngine;

namespace Choanji
{
	[CustomEditor(typeof(GameCamera))]
	public class GameCameraEditor : Editor
	{
		private GameCamera mThis;

		void OnEnable()
		{
			mThis = (GameCamera) target;
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