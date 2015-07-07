using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkStarter : MonoBehaviour {



	[Header("Server Network Configuration")]
	public int port = 59999;
	public int maxConnections = 1;
	public bool natPunchthrough = false;
	[Tooltip("The number of network updates per second")]
	public float sendRate = 15.0f;

	[Header("Initialization")]
	[Tooltip("Keeps the game running in the background even when not active. Useful for localhost debugging")]
	public bool keepActiveInBackground = true;
	public bool autoConnectAsClient = false;
	public bool autoStartServer = false;
	public string autoConnectIP = "127.0.0.1";
	public float retryMinTime = 1.0f;
	[Tooltip("Max retry timer, every retry attempt goes up by retryMinTime attempt until this value")]
	public float retryMaxTime = 5.0f;
	public int retryCount = 5;

	[Header("Debugging Config")]
	public Text statusText;
	public bool showDebugOnScreenText;
	public bool showDebugConsoleLog;


	private string serverIp;
	private NetworkView netView;

	private int autoRetryCount;
	private float retryTimer;

	// Use this for initialization
	void Start () {
		Network.sendRate = sendRate;


		netView = this.GetComponent<NetworkView> ();


		statusText.gameObject.SetActive(showDebugOnScreenText);
		SetStatusText("Disconnected");

		Application.runInBackground = keepActiveInBackground;

		if (autoConnectAsClient == true) {
			runRetry = true;
		}

		if (autoStartServer == true) {
			StartServer ();
		}


	}
	
	private bool runRetry = false;

	// Update is called once per frame
	void Update () {

		retryTimer -= Time.deltaTime;

		if (autoConnectAsClient == false) {
			return;
		}

		if (runRetry == true) {


			if (retryTimer <= 0.0f ) {
				runRetry = false;		//stop and wait till event happens


				//attempt retry if not connected
				Connect (autoConnectIP);
			}
	
			retryTimer -= Time.deltaTime;
		}
	}


	public void StartServer(){
		if (Network.peerType == NetworkPeerType.Disconnected) {
			NetworkConnectionError err = Network.InitializeServer(maxConnections, port, natPunchthrough);

			if (err == NetworkConnectionError.NoError){

			}
			else{
				//report error
				SetStatusText("Error: " + err.ToString());
			}
		}
	}


	public void Connect(string ip){

		if (ip.Length < 7) {
			SetStatusText("Please enter a valid IP address to connect");
			return;
		}

		if (Network.peerType == NetworkPeerType.Disconnected) {
			SetStatusText("Connecting...");
			NetworkConnectionError err = Network.Connect(ip, port);

			serverIp = ip;
			if (err == NetworkConnectionError.NoError){

			}
			else{
				//report error
				SetStatusText("Error: " + err.ToString());
			}
		}
	}

	public void Disconnect(){
		SetStatusText("Disconnected");
		Network.Disconnect ();
	}


	public void SetStatusText(string message){

		if (showDebugOnScreenText == true) {
			if (statusText != null) {
				statusText.text = message + "\nConnected: " + Network.connections.Length;
			}
		} else {
			statusText.gameObject.SetActive(false);
		}


		if (showDebugConsoleLog) {
			if (message.Length != 0){
				Debug.Log (message);
			}
		}
	}


	//=======================================================================
	//Connection events

	//Client connects to server successfully
	void OnConnectedToServer(){
		autoRetryCount = 0;
		runRetry = false;
		SetStatusText("Connected to: " + serverIp);
	}

	void OnFailedToConnect(NetworkConnectionError error) {




		if (autoRetryCount <= retryCount) {
			SetStatusText ("Connection to server failed\nError: " + error.ToString() + "\nretry #" + autoRetryCount);
			runRetry = true;
			retryTimer = retryMinTime + (Mathf.Min(autoRetryCount * retryMinTime, retryMaxTime));
		} else {
			//retry count max reached, stop
			SetStatusText ("Connection to server failed\nError: " + error.ToString() + "\nmax retries reached #" + autoRetryCount);
			autoRetryCount = 0;		//reset for next call to Connect();
		}
		autoRetryCount++;
	}
	


	//Player connects to server
	void OnPlayerConnected(NetworkPlayer player){
		SetStatusText("Player Connected");
	}

	//player disconnects from server
	void OnPlayerDisconnected(NetworkPlayer player){
		SetStatusText("Player disconnected");
	}
	
	//server finished initalizing and ready
	void OnServerInitialized(){
		SetStatusText("Server started: " + Network.player.ipAddress);
	}
}
