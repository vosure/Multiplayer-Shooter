using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera camera;

    [SerializeField]
    LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    private void Start()
    {
        if (camera == null)
        {
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (PauseMenu.isOn)
            return;

        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon.fireRate <= 0.0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0.0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    [Command]
    void CmdOnHit(Vector3 position, Vector3 normal)
    {
        RpcDoHitEffect(position, normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 position, Vector3 normal)
    {
       GameObject hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, position, Quaternion.LookRotation(normal)) as GameObject;
        Destroy(hitEffect, 2.0f);
    }

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerHasBeenShot(hit.collider.name, currentWeapon.damage);
            }

            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    void CmdPlayerHasBeenShot(string playerID, int damage)
    {
        Debug.Log(playerID + " has been shot");

        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
    }
}
