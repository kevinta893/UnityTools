using UnityEngine;
using UnityEngine;
using System.Collections;

public class UpdateNetworkUI : MonoBehaviour {

	public GameObject ipTextBox;
	public GameObject startServerButton;
	public GameObject connectButton;
	public GameObject disconnectButton;

	void OnServerInitialized(){
		StartServerClicked ();
	}


	public void StartServerClicked(){
		ipTextBox.SetActive (false);
		connectButton.SetActive (false);
		startServerButton.SetActive(false);
		disconnectButton.SetActive (true);
	}

	public void ConnectClicked(){
		ipTextBox.SetActive (false);
		connectButton.SetActive (false);
		startServerButton.SetActive(false);
		disconnectButton.SetActive (true);
	}

	//Goes back to the main menu
	public void ResetMainButtons(){
		ipTextBox.SetActive (true);
		connectButton.SetActive (true);
		startServerButton.SetActive(true);
		disconnectButton.SetActive (false);
	}
}
