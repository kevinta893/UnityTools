using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectScript : MonoBehaviour {

	public NetworkStarter networkStarter;
	public Text ipText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Connect(){

		if (networkStarter == null || ipText == null) {
			Debug.LogWarning("Warning: Network manager or ip textbox null references");
			return;
		}


		networkStarter.Connect (ipText.text);
	}
}
