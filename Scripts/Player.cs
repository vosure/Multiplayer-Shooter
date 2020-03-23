using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeaths;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;


    private void Update()
    {
        if (!isLocalPlayer)
            return;

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    RpcTakeDamage(9999);
        //}

        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = !Cursor.visible;

            Cursor.lockState = (Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;
        }

    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActice(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
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

    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;


        Debug.Log(transform.name + " has " + currentHealth + " health");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectsOnDeaths.Length; i++)
        {
            disableGameObjectsOnDeaths[i].SetActive(false);
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        GameObject deathEffectInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEffectInstance, 3.0f);

        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActice(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " is dead!");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.singleton.matchSettings.respawnTime);

        Transform startPosition = NetworkManager.singleton.GetStartPosition();
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

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

        for (int i = 0; i < disableGameObjectsOnDeaths.Length; i++)
        {
            disableGameObjectsOnDeaths[i].SetActive(true);
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        GameObject spawnEffectInstance = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(spawnEffectInstance, 3.0f);
    }
}
