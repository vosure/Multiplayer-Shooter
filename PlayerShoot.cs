using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    LayerMask mask;

    private void Start()
    {
        if (camera == null)
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, weapon.range, mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerHasBeenShot(hit.collider.name);
            }
        }
    }

    [Command]
    void CmdPlayerHasBeenShot(string ID)
    {
        Debug.Log(ID + " has been shot");
    }
}
