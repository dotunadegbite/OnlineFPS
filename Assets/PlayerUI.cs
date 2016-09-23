using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;
    

    void Start()
    {
        PauseMenuScript.isPaused = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
	}


   void TogglePauseMenu ()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenuScript.isPaused = pauseMenu.activeSelf;
    }
}
