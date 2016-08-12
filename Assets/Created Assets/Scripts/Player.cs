using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetUp))]

public class Player : NetworkBehaviour {
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get
        {
            return _isDead;
        }

        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    [SerializeField]
    GameObject[] disableGameObjectsOnDeath;

    private bool firstSetup = true;

    public void SetupPlayer()
    {
        //Switch Cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCamera(false);
            GetComponent<PlayerSetUp>().playerUIInstance.SetActive(true);
        }
        CmdBroadcastNewPlayer();
    }

    [Command]
    private void CmdBroadcastNewPlayer()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

       

        //Create Spawn Effect
        GameObject _spawngfx = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_spawngfx, 3f);
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        Debug.Log(transform.name + "now has" + currentHealth + "health");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        //Disable Components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Disable Components
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }
        //Disable Collider
        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = false;
        }
        //Spawn Death effect
        GameObject _deathgfx = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_deathgfx, 3f);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCamera(true);
            GetComponent<PlayerSetUp>().playerUIInstance.SetActive(false);
        }


        Debug.Log(transform.name + "dead");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        Transform _spawnpoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnpoint.position;
        transform.rotation = _spawnpoint.rotation;

        yield return new WaitForSeconds(0.1f);
        SetupPlayer();
      
        Debug.Log(transform.name + "back to life");
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(100);
        }
    }
}
