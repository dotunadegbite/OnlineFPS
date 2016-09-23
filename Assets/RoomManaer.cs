using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoomManaer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void ChangeLevel (string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
