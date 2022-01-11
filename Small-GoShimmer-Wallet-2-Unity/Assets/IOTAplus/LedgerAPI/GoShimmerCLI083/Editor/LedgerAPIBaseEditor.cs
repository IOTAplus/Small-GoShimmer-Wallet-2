using UnityEditor;
using UnityEngine;

namespace IOTAplus.LedgerAPI
{
	[CustomEditor (typeof(LedgerAPIBase))]
	public class LedgerAPIBaseEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();

			var script = (LedgerAPIBase) target;

			if (GUILayout.Button ("Get Balance"))
			{
				script.GetBalance ();
			}
		}
	}
}