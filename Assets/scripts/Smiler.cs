
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Smiler : UdonSharpBehaviour
{

    readonly private float minDistance = 15f;
    readonly private float maxDistance = 75f;
    readonly private float smilerKillDistance = 3f;
    public float smilerSpeed = 45f;
    public GameObject smilerObj;
    public GameObject smilerParent;

    public GameObject smilerSFX;
    public AudioSource smilerVanish;
    public AudioSource smilerVanishSFX;
    public AudioSource smilerJS;

    public AudioSource smilerButton;
    public UdonBehaviour ParticleLidar;

    public GameObject scannerObj;
    public VRC_Pickup scannerPickup;
    public Transform playerSpawn;
    public Transform DeathSpawn;
    public AudioSource smilerScream;

    public Transform smilerSpawn;
    public Transform smilerSpawn1;
    public Transform smilerSpawn2;
    public Transform smilerSpawn3;
    public Transform smilerSpawn4;
    public Transform smilerSpawn5;


    private int smilerCount = 0;
    private int smilerSpawnVal = 0;

    public AudioSource shutdownSound;
    public AudioSource chaseMusic;
    public AudioSource heartbeatSFX;
    public AudioSource deathMusic;
    public AudioSource deathSFX;

    private bool smilerAttack = false;
    public bool respawnSmiler = false;

    public bool reset;
    private void Update()
    {
        //If its the first smiler spotting, when player gets minDistance away from smiler, teleport to spawn as warning
        if (Vector3.Distance(smilerObj.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && smilerCount == 0 && smilerObj.activeSelf)
        {
            SmilerDrop();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SmilerVanish");
        }
        //If its not first smiler spotting, when player gets minDistance away from smiler, smiler attacks local player.
        if (Vector3.Distance(smilerObj.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && smilerCount > 0 && smilerObj.activeSelf)
            smilerAttack = true;
        //If smiler is active, but players walks maxDistance away, respawn smiler
        if (Vector3.Distance(smilerObj.transform.position, Networking.LocalPlayer.GetPosition()) >= maxDistance && smilerObj.activeSelf)
        {
            respawnSmiler = true;
        }
        if (smilerAttack)
        {
            if (Vector3.Distance(smilerObj.transform.position, Networking.GetOwner(scannerObj).GetPosition()) > smilerKillDistance)
            {
                if(!smilerJS.isPlaying)
                    smilerJS.Play();
                smilerObj.transform.position = Vector3.MoveTowards(smilerObj.transform.position, Networking.GetOwner(scannerObj).GetPosition() + new Vector3(0,1.5f,0), smilerSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(smilerObj.transform.position, Networking.GetOwner(scannerObj).GetPosition()) <= smilerKillDistance)
            {
                Debug.Log("Localplayer Died!");

                if (Networking.LocalPlayer.IsOwner(scannerObj))
                    scannerPickup.Drop();
                shutdownSound.Stop();
                shutdownSound.mute = true;
                chaseMusic.Stop();
                heartbeatSFX.Stop();
                deathMusic.Play();
                deathSFX.Play();
                smilerVanishSFX.Play();
                smilerParent.SetActive(false);
                Networking.LocalPlayer.TeleportTo(DeathSpawn.position, DeathSpawn.rotation);
                smilerAttack = false;
            }
        }
        if (respawnSmiler)
        {
            spawnSmiler();
            respawnSmiler = false;
        }
        if (reset)
        {
            smilerObj.SetActive(false);
            smilerParent.SetActive(true);
            smilerParent.transform.position += new Vector3(0, 500, 0);
        }
    }

    public void SmilerVanish()
    {
        if (smilerCount == 0)
            ParticleLidar.SetProgramVariable("shutdownEvent", (bool)true);

        smilerScream.Play();
        smilerVanish.Play();
        smilerVanishSFX.Play();

        smilerCount++;

        spawnSmiler();
    }

    public void SmilerDrop()
    {
        if(Networking.LocalPlayer.IsOwner(scannerObj))
            scannerPickup.Drop();
        Networking.LocalPlayer.TeleportTo(playerSpawn.position,playerSpawn.rotation);
    }

    public void spawnSmiler()
    {
        smilerObj.SetActive(false);
        smilerSpawnVal = Random.Range(0, 5);
        switch (smilerSpawnVal)
        {
            case 0:
                smilerParent.transform.position = smilerSpawn.position + new Vector3(0, 2.5f, 0);
                break;
            case 1:
                smilerParent.transform.position = smilerSpawn1.position + new Vector3(0, 2.5f, 0);
                break;
            case 2:
                smilerParent.transform.position = smilerSpawn2.position + new Vector3(0, 2.5f, 0);
                break;
            case 3:
                smilerParent.transform.position = smilerSpawn3.position + new Vector3(0, 2.5f, 0);
                break;
            case 4:
                smilerParent.transform.position = smilerSpawn4.position + new Vector3(0, 2.5f, 0);
                break;
            case 5:
                smilerParent.transform.position = smilerSpawn5.position + new Vector3(0, 2.5f, 0);
                break;
        }

    }
}
