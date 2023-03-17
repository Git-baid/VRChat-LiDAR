
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class chaseSequence : UdonSharpBehaviour
{
    public GameObject entity;
    public GameObject entity2;
    public GameObject entity3;
    public GameObject entity4;
    public GameObject entity5;
    public GameObject entity6;

    private GameObject chaseEntity;

    public bool chaseBool = false;
    public bool chaseActive = false;
    private readonly float minDistance = 1.2f;
    public float speed = 1;
    private readonly float chaseTimeTarget = 18.0f;
    public Transform DeathSpawn;
    public GameObject LidarScanner;


    public int EntityHitVal;
    [UdonSynced]
    private float chaseTimer = 0;

    public UdonBehaviour ParticleLidar;
    public UdonBehaviour SpawnScript;

    public GameObject runText;

    public AudioSource shutdownSound;
    public AudioSource deathMusic;
    public AudioSource deathSFX;
    public AudioSource chaseMusic;
    public AudioSource heartbeatSFX;

    public VRC_Pickup Pickup;

    public GameObject scannerAlarm;
    public GameObject scannerScreen;
    public Material redScreen;
    public Material lidarMaterial;

    public GameObject entityGroan;
    public GameObject entity1Groan;
    public GameObject entity2Groan;
    public GameObject entity3Groan;
    public GameObject entity4Groan;
    public GameObject entity5Groan;

    public GameObject chaseFootSteps;
    public GameObject chaseFootSteps1;
    public GameObject chaseFootSteps2;
    public GameObject chaseFootSteps3;
    public GameObject chaseFootSteps4;
    public GameObject chaseFootSteps5;

    public bool reset;


    private void Update()
    {
        if (reset)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ResetChaseSequence");
        }
        if (chaseBool)
        {
            Debug.Log("Recieved chaseBool = true");
            if (!chaseActive)
            {
                ChaseSequence();
                //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChaseSequence");
            }
            chaseBool = false;
        }

        if (chaseActive)
        {
            
            chaseTimer += Time.deltaTime;
            //if scanning/not scanning, move character 1xSpeed/0.5xSpeed
            if (Vector3.Distance(chaseEntity.transform.position, Networking.GetOwner(LidarScanner).GetPosition()) > minDistance && chaseActive && !(bool)ParticleLidar.GetProgramVariable("activeScan"))
            {
                chaseEntity.transform.position = Vector3.MoveTowards(chaseEntity.transform.position, Networking.GetOwner(LidarScanner).GetPosition(), speed/2 * Vector3.Distance(chaseEntity.transform.position, Networking.GetOwner(LidarScanner).GetPosition()) * Time.deltaTime);
            }else if(Vector3.Distance(chaseEntity.transform.position, Networking.GetOwner(LidarScanner).GetPosition()) > minDistance && chaseActive && (bool)ParticleLidar.GetProgramVariable("activeScan"))
            {
                chaseEntity.transform.position = Vector3.MoveTowards(chaseEntity.transform.position, Networking.GetOwner(LidarScanner).GetPosition(), (speed/3) * Vector3.Distance(chaseEntity.transform.position, Networking.GetOwner(LidarScanner).GetPosition()) * Time.deltaTime);
            }

            if (Vector3.Distance(chaseEntity.transform.position, Networking.GetOwner(LidarScanner).GetPosition()) <= minDistance)
            {
                Debug.Log("Localplayer Died!");
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChaseSequenceEnd");
                if (Networking.LocalPlayer.IsOwner(LidarScanner))
                {
                    Debug.Log("Calling DeathSequence");
                    DeathSequence();
                }
                else
                {
                    Debug.Log("LocalPlayer is not owner of Scanner Object!");
                }
            }
            if(chaseTimer >= chaseTimeTarget )
            {
                Debug.Log("Timer reached threshold (" + chaseTimer + "/" + chaseTimeTarget + ")");
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChaseSequenceEnd");
            }
            entityGroan.SetActive(true); entity1Groan.SetActive(true);
            entity2Groan.SetActive(true); entity3Groan.SetActive(true);
            entity4Groan.SetActive(true); entity5Groan.SetActive(true);

            scannerAlarm.SetActive(true);
            scannerScreen.GetComponent<MeshRenderer>().material = redScreen;
        }
        //if any player makes contact with an entity outside of chase sequence, respawn all entities
        if (Vector3.Distance(entity.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && !chaseActive)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RespawnEntityonCollision");
        }
        else if (Vector3.Distance(entity2.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && !chaseActive)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RespawnEntityonCollision");
        }
        else if (Vector3.Distance(entity3.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && !chaseActive)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RespawnEntityonCollision");
        }
        else if (Vector3.Distance(entity4.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && !chaseActive)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RespawnEntityonCollision");
        }
        else if (Vector3.Distance(entity5.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && !chaseActive)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RespawnEntityonCollision");
        }
        else if (Vector3.Distance(entity6.transform.position, Networking.LocalPlayer.GetPosition()) <= minDistance && !chaseActive)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RespawnEntityonCollision");
        }

    }
    
    public void RespawnEntityonCollision()
    {
        SpawnScript.SetProgramVariable("buttonPressed", (bool)true);
    }

    public void ChaseSequenceEnd()
    {
        Debug.Log("ChaseSequenceEnd Called");
        chaseActive = false;
        chaseTimer = 0;
        chaseEntity = null;
        entity.SetActive(true);
        entity2.SetActive(true);
        entity3.SetActive(true);
        entity4.SetActive(true);
        entity5.SetActive(true);
        entity6.SetActive(true);
        runText.SetActive(false);
        chaseFootSteps.SetActive(false);
        chaseFootSteps1.SetActive(false);
        chaseFootSteps2.SetActive(false);
        chaseFootSteps3.SetActive(false);
        chaseFootSteps4.SetActive(false);
        chaseFootSteps5.SetActive(false);
        shutdownSound.Play();
        heartbeatSFX.Stop();
        scannerAlarm.SetActive(false);
        scannerScreen.GetComponent<MeshRenderer>().material = lidarMaterial;
        SpawnScript.SetProgramVariable("buttonPressed", (bool)true);

        entityGroan.SetActive(false); entity1Groan.SetActive(false);
        entity2Groan.SetActive(false); entity3Groan.SetActive(false);
        entity4Groan.SetActive(false); entity5Groan.SetActive(false);
    }

    public void ChaseSequence()
    {
        chaseTimer = 0;
        entity.SetActive(false);
        entity2.SetActive(false);
        entity3.SetActive(false);
        entity4.SetActive(false);
        entity5.SetActive(false);
        entity6.SetActive(false);
        runText.SetActive(true);
        chaseFootSteps.SetActive(true);
        chaseFootSteps1.SetActive(true);
        chaseFootSteps2.SetActive(true);
        chaseFootSteps3.SetActive(true);
        chaseFootSteps4.SetActive(true);
        chaseFootSteps5.SetActive(true);
        shutdownSound.Stop();
        chaseMusic.Play();
        heartbeatSFX.Play();

        EntityHitValToChaseEntity();

        chaseActive = true;

    }

    public void EntityHitValToChaseEntity()
    {
        EntityHitVal = (int)ParticleLidar.GetProgramVariable("EntityHitVal");

        Debug.Log("chaseSequence EntityHitVal = " + EntityHitVal);
        switch (EntityHitVal)
        {
            case 0:
                Debug.Log("Error: EntityHitVal cannot be 0, setting fallback case entity = 1");
                chaseEntity = entity;
                entity.SetActive(true);
                break;
            case 1:
                chaseEntity = entity;
                entity.SetActive(true);
                break;
            case 2:
                chaseEntity = entity2;
                entity2.SetActive(true);
                break;
            case 3:
                chaseEntity = entity3;
                entity3.SetActive(true);
                break;
            case 4:
                chaseEntity = entity4;
                entity4.SetActive(true);
                break;
            case 5:
                chaseEntity = entity5;
                entity5.SetActive(true);
                break;
            case 6:
                chaseEntity = entity6;
                entity6.SetActive(true);
                break;
        }
    }

    public void DropPickup()
    {
        Pickup.Drop();
    }

    public void DeathSequence()
    {
        Debug.Log("DeathSequence Called");
        shutdownSound.Stop();
        shutdownSound.mute = true;
        chaseMusic.Stop();
        heartbeatSFX.Stop();
        deathMusic.Play();
        deathSFX.Play();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "DropPickup");
        Debug.Log("teleporting player");
        Networking.LocalPlayer.TeleportTo(DeathSpawn.position, DeathSpawn.rotation);
        SpawnScript.SetProgramVariable("buttonPressed", (bool)true);
    }

    public void ResetChaseSequence()
    {
        Debug.Log("Resetting ChaSeq");
        shutdownSound.mute = false;
        shutdownSound.Stop();
        chaseMusic.Stop();
        heartbeatSFX.Stop();
        deathMusic.Stop();
        chaseActive = false;
        chaseBool = false;
        chaseTimer = 0;
        chaseEntity = null;
        scannerAlarm.SetActive(false);
        scannerScreen.GetComponent<MeshRenderer>().material = lidarMaterial;
        EntityHitVal = 0;

        entityGroan.SetActive(false); entity1Groan.SetActive(false);
        entity2Groan.SetActive(false); entity3Groan.SetActive(false);
        entity4Groan.SetActive(false); entity5Groan.SetActive(false);

        chaseFootSteps.SetActive(false);
        chaseFootSteps1.SetActive(false);
        chaseFootSteps2.SetActive(false);
        chaseFootSteps3.SetActive(false);
        chaseFootSteps4.SetActive(false);
        chaseFootSteps5.SetActive(false);
        reset = false;
    }
}
