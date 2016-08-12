using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    private NetworkManager manager;
    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

	// Use this for initialization
	void Start () {

        manager = NetworkManager.singleton;
        if(manager.matchMaker == null)
        {
            manager.StartMatchMaker();
        }

        RefreshRoomList();
	
	}
	
    public void RefreshRoomList()
    {
        ClearRoomList();
        manager.matchMaker.ListMatches(0, 20, "", OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(ListMatchResponse matchList)
    {
        status.text = "";

        if(matchList == null)
        {
            status.text = "Coudln't find matched";
            return;
        }

        

        foreach(MatchDesc match in matchList.matches)
        {
            GameObject roomListItem = Instantiate(roomListItemPrefab);
            roomListItem.transform.SetParent(roomListParent);

            RoomListItem _roomList = roomListItem.GetComponent<RoomListItem>();
            if(_roomList != null)
            {
                _roomList.Setup(match,JoinRoom);
            }

            //Component takes care of the name and amount of Users and proper connection to room

            roomList.Add(roomListItem);


        }

        if(roomList.Count == 0)
        {
            status.text = "No current games";
        }
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void JoinRoom(MatchDesc _match)
    {
        manager.matchMaker.JoinMatch(_match.networkId, "", manager.OnMatchJoined);
        ClearRoomList();
        
    }
	
}
