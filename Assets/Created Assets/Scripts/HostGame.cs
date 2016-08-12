using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {
    [SerializeField]
    private uint roomSize = 6;

    private string roomName;
    private NetworkManager manager;


    void Start()
    {
        manager = NetworkManager.singleton;
        if (manager.matchMaker == null)
            manager.StartMatchMaker();
    }

    public void SetRoomName(string name)
    {
        roomName = name;
        Debug.Log(name);
    }


    public void CreateRoom()
    {
        
        if(roomName != null && roomName != "")
        {
            Debug.Log("Creating Room: " + roomName);

            //Create Room
            manager.matchMaker.CreateMatch(roomName, roomSize, true, "","","",0,0,manager.OnMatchCreate);
        }
    }
}
