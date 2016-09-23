using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking.Match;

public class PauseMenuScript : MonoBehaviour {

    public static bool isPaused = false;
    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }


    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
