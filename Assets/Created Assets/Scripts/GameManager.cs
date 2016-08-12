using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public MatchSettings matchSettings;
    public static GameManager instance;

    [SerializeField]
    private GameObject sceneCamera;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than One Game Manager");
        } else {
            instance = this;
        }
    }


    public void SetSceneCamera(bool isActive)
    {
        if (sceneCamera == null)
            return;
        sceneCamera.SetActive(isActive);
    }


    #region Player Registering

    private const string PLAYER_ID_PREFIX = "Player ";
   
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    
    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX+ _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    #endregion

   
}
