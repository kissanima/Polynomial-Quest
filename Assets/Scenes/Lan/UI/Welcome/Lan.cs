using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class LanLan : MonoBehaviour
{
    private Player player;

	[SerializeField] TextMeshProUGUI ipAddressLabel;
	[SerializeField] TMP_InputField ipInput;
    [SerializeField] private NetworkPrefabsList _networkPrefabsList;
	[SerializeField] string ipAddress;
	[SerializeField] UnityTransport transport;
	public GameObject welcome, startClient, startHost, enter, characterCreation, createCharacterButton;
	LanGameManager gmScript;

	void Start()
	{
		Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

		gmScript = rootObjects[0].GetComponent<LanGameManager>();
		ipAddress = "0.0.0.0";
		SetIpAddress();
		
		characterCreation = GameObject.FindWithTag("CharacterCreation");
	}

	// To Host a game
	public void StartHost() {
		NetworkManager.Singleton.StartHost();
		GetLocalIPAddress();
		if(PlayerPrefs.GetString("username") != "") {
			ipInput.gameObject.SetActive(false); //hide the ip inputBox
			startClient.SetActive(false); //hide the start Client Button
			startHost.SetActive(false); //hide the start Host Button
			enter.SetActive(true); //show the enter button
		}
		else {
			ipAddressLabel.SetText("no player data found, create new account");
			createCharacterButton.SetActive(true);
		}
	}

	// To Join a game
	public void StartClient() {
		if(ipInput.text != ""){
		ipAddress = ipInput.text;
	    SetIpAddress();
		NetworkManager.Singleton.StartClient();
		welcome.SetActive(false);

		//initialize game manager
		//gmScript.Initialize();
		}
		else {
			ipAddressLabel.color = Color.red;
			ipAddressLabel.SetText("error: IP is empty!");
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
		welcome.SetActive(false);
	}

}
