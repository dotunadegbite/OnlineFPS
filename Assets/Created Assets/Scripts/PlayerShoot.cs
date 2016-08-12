using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;
    public PlayerWeapon weapon;

    [SerializeField]
    private LayerMask mask;

    

    void Start()
    {
        if(cam == null)
        {
            Debug.Log("No cam");
            this.enabled = false;
        }
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
            return;

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position,cam.transform.forward,out _hit,weapon.range,mask))
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
            
        }
    }

    [Command]

    void CmdPlayerShot(string playerID, int _damage)
    {
        Debug.Log(playerID + " got shot");
        Player _player = GameManager.GetPlayer(playerID);
        _player.RpcTakeDamage(_damage);

    }
}
