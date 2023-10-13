using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class PingCounter : NetworkBehaviour
{
  public string address; // Server address
  [SerializeField] float updateSeconds = 5f; // How often to ping the server
  [SerializeField] string pingPreffix = "Ping: "; // Preffix on the UI element

  [SerializeField] Color32 goodPingColour = new Color32(105, 255, 5, 255);
  [SerializeField] int acceptablePing = 150;
  [SerializeField] Color32 acceptablePingColour = new Color32(255, 252, 5, 255);
  [SerializeField] int badPing = 300;
  [SerializeField] Color32 badPingColour = new Color32(252, 57, 3, 255);
  [SerializeField] UnityTransport transport;

  private Text pingText; //Text element to render ping value
  private TimeSpan timer = DateTime.Now.TimeOfDay; // last ping received on
  private float lastPing = 0f;

  public override void OnNetworkSpawn() {
    //Debug.Log("ping meter Initialized");
    transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    if(IsHost) {
      address = "127.0.0.1";
      //Debug.Log("Ping meter IP address: " + address);
    }
    else {
      address = transport.ConnectionData.Address;
      //Debug.Log("Ping meter IP address: " + address);
    }
  }

  void Update()
  {
    if(!IsHost || !IsClient) return;
    pingText = gameObject.GetComponent<Text>();
    if(DateTime.Now.TimeOfDay.Subtract(timer).Seconds >= updateSeconds) {
      StopCoroutine("PingUpdate");
      timer = DateTime.Now.TimeOfDay;
      StartCoroutine(PingUpdate());
    }
  }

  IEnumerator PingUpdate()
  {
    var ping = new Ping(address);
    yield return new WaitUntil(() => ping.isDone);

    if(ping.time > badPing && pingText.color != badPingColour) {
      pingText.color = badPingColour;
    } else if (ping.time > acceptablePing && pingText.color != acceptablePingColour) {
      pingText.color = acceptablePingColour;
    } else if (pingText.color != goodPingColour) {
      pingText.color = goodPingColour;
    }
    if (lastPing != ping.time) {
      pingText.text = $"{pingPreffix}{ping.time}";
    }
  }
}
