using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace IOTAplus.Utilities
{
	public class ImageLoader : MonoBehaviour
	{
//		public string   url = "https://i.pinimg.com/originals/9e/1d/d6/9e1dd6458c89b03c506b384f537423d9.jpg";
		public string   url = "https://i.pinimg.com/originals/9e/1d/d6/9e1dd6458c89b03c506b384f537423d9.jpg";
		public RawImage thisRenderer;

		void Start ()
		{
			StartCoroutine (LoadFromLikeCoroutine ()); // execute the section independently
		}

		private IEnumerator LoadFromLikeCoroutine ()
		{
			Debug.Log ("Loading ....");
			WWW wwwLoader = new WWW (url); // create WWW object pointing to the url
			yield return wwwLoader;        // start loading whatever in that url ( delay happens here )

			Debug.Log ("Loaded");
			thisRenderer.texture = wwwLoader.texture; // set loaded image
		}
	}
}