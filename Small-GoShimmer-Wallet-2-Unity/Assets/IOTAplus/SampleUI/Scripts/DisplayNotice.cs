using UnityEngine;
using UnityEngine.UI;

namespace IOTAplus.SampleUI
{
	/// <summary>
	/// A sample script to display a notification on the UI. Can be used to indicate success or non-fatal failure of
	/// a transaction. Attach to a Text object.
	/// </summary>
	public class DisplayNotice : MonoBehaviour
	{
		private Text _text; 
			
		private void Awake ()
		{
			_text         = GetComponent<Text> ();
			_text.enabled = false;
		}

		public void DisplayText ()
		{
			EnableText ();
			Invoke (nameof(DisableText), 2f);
		}

		protected void EnableText ()
		{
			_text.enabled = true;
		}
	
		protected void DisableText ()
		{
			_text.enabled = false;
		}
	}
}
