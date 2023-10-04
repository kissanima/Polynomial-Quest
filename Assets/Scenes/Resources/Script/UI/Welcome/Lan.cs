using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;
using TMPro;
using System.Linq;

public class Lan : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI ipAddressLabel, welcomeBackText;
	[SerializeField] TMP_InputField ipInput;
    [SerializeField] private NetworkPrefabsList _networkPrefabsList;
	[SerializeField] string ipAddress;
	[SerializeField] UnityTransport transport;
	public GameObject welcome, startClient, startHost, enter, difficultyPanel, createCharacterButton, ui;
	public LanCreateCharacter characterCreation;
	PingCounter pingCounter;
	LanGameManager gmScript;
	bool hasCharacter;
	string tempUsername;
	float finishIntroClient;
	LanNpc[] npcs;
	void Start()
	{
		
		ipAddress = "0.0.0.0";
		SetIpAddress(); // Set the Ip to the above address
		//InvokeRepeating("assignPlayerController", 0.1f, 0.1f);
		//RegisterNetworkPrefabs();
		gmScript = GameObject.FindWithTag("GameManager").GetComponent<LanGameManager>();
		finishIntroClient = PlayerPrefs.GetInt("finishIntro");

		npcs = FindObjectsOfType<LanNpc>();
	}

	private void OnEnable() {
		if((tempUsername = PlayerPrefs.GetString("username")) != "") {
			hasCharacter = true;
			ipInput.gameObject.SetActive(true);
			welcomeBackText.gameObject.SetActive(true);
			welcomeBackText.SetText("Welcome back, " + tempUsername + "\n    Level: " + PlayerPrefs.GetInt("level"));
		}
	}

	// To Host a game
	public void StartHost() {
		NetworkManager.Singleton.StartHost();
		GetLocalIPAddress();
	    difficultyPanel.SetActive(false);
		enter.SetActive(true); //show the enter button
	}

	// To Join a game
	public void StartClient() { //JOIN Button
		if(hasCharacter == false) {
			welcomeBackText.SetText("no player data found, create new character");
			welcomeBackText.gameObject.SetActive(true);
			createCharacterButton.SetActive(true);
		}
		else {
			if(ipInput.text != ""){

			ipAddress = ipInput.text;
	    	SetIpAddress();
			NetworkManager.Singleton.StartClient();
			welcome.SetActive(false);

			if(finishIntroClient == 1) {
				ui.transform.GetChild(10).gameObject.SetActive(true); //minimap
				ui.transform.GetChild(14).gameObject.SetActive(true); //mission
				ui.transform.GetChild(11).gameObject.SetActive(false);
			}
			else {
				ui.transform.GetChild(11).gameObject.SetActive(true); //start intro
			}
		//GameObject.FindWithTag("UI").transform.GetChild(14).gameObject.SetActive(false); //enable mission
		}
		else {
			ipAddressLabel.color = Color.red;
			ipAddressLabel.SetText("error: IP is empty!");
			}
		}
	}

	/* Gets the Ip Address of your connected network and
	shows on the screen in order to let other players join
	by inputing that Ip in the input field */
	// ONLY FOR HOST SIDE 
	public void GetLocalIPAddress() {
		var host = Dns.GetHostEntry(Dns.GetHostName());

		int count = 0;
		foreach (var ip in host.AddressList) {
			count++;
			if(count == host.AddressList.Length) {
				ipAddressLabel.SetText(ip.ToString()); //show the IP
				//ipAddress = ip.ToString();
			}
		}
	}

	/* Sets the Ip Address of the Connection Data in Unity Transport
	to the Ip Address which was input in the Input Field */
	// ONLY FOR CLIENT SIDE
	public void SetIpAddress() {
		transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
		transport.ConnectionData.Address = ipAddress;
	}
	
	 private void RegisterNetworkPrefabs()
    {
        var prefabs = _networkPrefabsList.PrefabList.Select(x => x.Prefab);
        foreach (var prefab in prefabs)
        {
            NetworkManager.Singleton.AddNetworkPrefab(prefab);
        }
    }

	public void ButtonPressedEnter() {
		gmScript.player.StartIntroduction();
		gmScript.StartBackgroundMusic();
		
		foreach (var item in npcs)
		{
			if(!item.isAI && !item.isNpc) {
				item.LoadStatue();
			}
		}
	}

	public void ButtonHost() {
		string usernametemp = PlayerPrefs.GetString("username");
		if(usernametemp != "") {
			welcomeBackText.gameObject.SetActive(true); //enable welcomeback Object
			welcomeBackText.SetText("Welcome back, " + PlayerPrefs.GetString("username") + "\n    Level: " + PlayerPrefs.GetInt("level"));

			ipInput.gameObject.SetActive(false); //hide the ip inputBox
			startClient.SetActive(false); //hide the start Client Button
			startHost.SetActive(false); //hide the start Host Button
			difficultyPanel.SetActive(true); //show select difficulty
		}
		else {
			welcomeBackText.SetText("no player data found, create new character");
	    	createCharacterButton.SetActive(true);
		}
		/*
		ipInput.gameObject.SetActive(false); //hide the ip inputBox
		startClient.SetActive(false); //hide the start Client Button
		startHost.SetActive(false); //hide the start Host Button
		difficultyPanel.SetActive(false); //show select difficulty  */
	}

	public void CreateCharacterPressed() {
		ui.SetActive(false);
		characterCreation.gameObject.SetActive(true);
	}



}
