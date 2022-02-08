using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using IOTAplus.LedgerAPI.WalletAPI;


namespace IOTAplus.LedgerAPI.GoShimmerCLI083
{
	/// <summary>
	/// GoShimmerCLI083 is an implementation of LedgerAPI, descended from LedgerAPIBase, that uses the compiled
	/// command-line interface.
	/// 
	/// -- NOT RECOMMENDED FOR PRODUCTION. --
	/// 
	/// This has been developed with and for v0.8.3 of the GoShimmer CLI. As a protocol under
	/// development, earlier and later versions of the CLI might have different commands and/or output and so this
	/// version of the LedgerAPI may not work with any version of the CLI other than v0.8.3.
	/// 
	/// The GoShimmer CLI executables can be downloaded from https://github.com/iotaledger/goshimmer/releases.
	/// The CLI executables must be in ./Assets/StreamingAssets.
	/// </summary>
	public class GoShimmerCLI083 : LedgerAPIBase
	{
		private IWallet _wallet;
		
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		private string path = Application.streamingAssetsPath + "/cli-wallet-mac";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		private string path = Application.streamingAssetsPath + "/cli-wallet-win.exe";
#elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
		private string path = Application.streamingAssetsPath + "/cli-wallet-linux";
#else
		private string path = "CLI not supported on this platform.";
		Debug.LogError (path);
#endif


		private void Start ()
		{
			_wallet = GetComponent<IWallet> ();

			if (_wallet == null)
				throw new System.ApplicationException("ERROR! No Wallet component found.");
		}

	
		public override void GetBalance ()
		{
			LedgerAPI.GoShimmerCLI083.GetBalance command = new GetBalance (this, _wallet);
			StartCoroutine (command.Execute (_OnGetBalanceSuccess, _OnGetBalanceFailure));
		}

		
		public override void SendToken (float amountToSend, string colorToSend, string destinationAddress)
		{
			LedgerAPI.GoShimmerCLI083.SendToken command = new SendToken (this, _wallet);
			StartCoroutine (command.Execute (amountToSend, colorToSend, destinationAddress,
					_OnSendTokenSuccess, _OnSendTokenFailure));
		}

		
		/// <summary>
		/// Calls the GoShimmer wallet CLI, passing in the provided parameters.
		/// </summary>
		/// <param name="parameters">Parameters to pass to the CLI</param>
		internal IEnumerator StartProcess (string parameters, Action<string> callback = null)
		{
			string logString = string.Concat (this.GetType (), ".", nameof(this.StartProcess), ":\n");

			if (string.IsNullOrEmpty (parameters))
			{
				Debug.LogError (string.Concat(logString, " -- ERROR -- No parameters provided."));
			}

			Process process = new Process ();
			process.StartInfo.FileName  = path;
			process.StartInfo.Arguments = parameters;

			process.StartInfo.UseShellExecute        = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError  = true;
			process.StartInfo.CreateNoWindow         = true;

			process.Start ();

#if DEBUG
			float startTime = Time.realtimeSinceStartup;
#endif
			
			while (!process.HasExited) yield return null;

#if DEBUG
			float readStartTime = Time.realtimeSinceStartup;
#endif

			// Check for error messages
			string err = process.StandardError.ReadToEnd ();
			
#if DEBUG
			float endTime = Time.realtimeSinceStartup;
			Debug.Log (String.Concat("CLI exited.\nTotal time : ", (endTime - startTime).ToString(), " seconds.\n",
					"Read Time: ", (endTime - readStartTime).ToString("0.0000000000"), " seconds."));
#endif

			if (err.Length > 0)
				Debug.LogError (String.Concat (logString, "Process Error!\n", err));

			// Read output
			string output = process.StandardOutput.ReadToEnd ();

#if DEBUG
			if (output.Length > 0)
				Debug.Log (string.Concat (logString, "Process complete.\n", output));
#endif

			callback?.Invoke (output);
		}
	}
}