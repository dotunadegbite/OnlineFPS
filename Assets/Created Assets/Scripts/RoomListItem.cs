using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomName;

    private MatchInfoSnapshot match;

    public void Setup (MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
    {
        match = _match;

        roomName.text = match.name + "  (" + match.currentSize + "/" + match.maxSize + ")";

        joinRoomCallback = _joinRoomCallback;

    }


    public void JoinRoom()
    {
        joinRoomCallback.Invoke(match);
        
    }

}
