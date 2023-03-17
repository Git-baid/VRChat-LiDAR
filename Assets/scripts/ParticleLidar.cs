
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ParticleLidar : UdonSharpBehaviour
{
    public LineRenderer scanLine;
    public LineRenderer scanLine1;
    public LineRenderer scanLine2;
    public LineRenderer scanLine3;
    public Camera lidarCam;

    public GameObject entity;
    public GameObject entity2;
    public GameObject entity3;
    public GameObject entity4;
    public GameObject entity5;
    public GameObject entity6;

    public Rigidbody entityRB;
    public Rigidbody entityRB2;
    public Rigidbody entityRB3;
    public Rigidbody entityRB4;
    public Rigidbody entityRB5;
    public Rigidbody entityRB6;

    public GameObject entityGroan;
    public GameObject entity1Groan;
    public GameObject entity2Groan;
    public GameObject entity3Groan;
    public GameObject entity4Groan;
    public GameObject entity5Groan;
    
    public AudioSource scanSound;
    public AudioSource shutdownSound;

    public float castDensity = 0.015f;
    public int scanSpeed = 100;
    public GameObject particleSystemGO;
    public ParticleSystem m_System;
    private ParticleSystem.Particle[] m_Particles;
    public bool activeScan = false;
    private float scanTime;
    //number of rays currently shot
    private int scanCount;
    //max amount of rays to shoot before stopping scan
    private float targetCount;
    //max amount of particles
    private int totalParticleCount;
    private int entityHitCount;
    public bool entityHitBool = false;
    Vector2 rayPos;

    private float shutdownTime = 0;

    public bool shutdownEvent = false;

    public bool stageTwo = false;
    public bool stageThree = false;
    public UdonBehaviour SpawnScript;
    public UdonBehaviour chaseSequence;

    private bool scanHasEntity;
    [UdonSynced]
    private bool scannerUse;

    public GameObject scannerAlarm;
    public GameObject scannerScreen;
    public Material redScreen;
    public Material lidarMaterial;

    public GameObject LidarScanner;
    public GameObject SpawnButton;

    public int EntityHitVal;
    public bool hasScanCheckedChaseSeq;
    public float chaseSequenceRandInt;
    private const float chaseSequenceChance = 0.35f;
    public bool reset;

    public AudioSource scareSFX;
    [UdonSynced]
    private bool nextScanWillChase;
    private float scanLineRandX = 0;
    private float scanLineRandX1 = 0;
    private float scanLineRandX2 = 0;
    private float scanLineRandX3 = 0;

    public Rigidbody smilerRB;
    public GameObject smilerObj;

    public UdonBehaviour VRCWorldScript;

    private VRCPlayerApi playerApi = Networking.LocalPlayer;

    //layer mask for raycasts to only hit gameobjects on layer 22 (ScannerCollider)
    public int scannerLayerMask = 1 << 22;

    //reset game
    public void ResetParticleLidar()
    {
        Debug.Log("Resetting ParticleLidar");
        for (int i = 0; i < m_System.main.maxParticles; i++)
        {
            m_Particles[i].startColor = Color.white;
            m_Particles[i].position = particleSystemGO.transform.position;
        }
        chaseSequenceRandInt = 0;
        stageTwo = false;
        stageThree = false;
        shutdownTime = 0;
        shutdownEvent = false;
        totalParticleCount = 0;
        entityHitCount = 0;
        entityHitBool = false;
        scanCount = 0;
        castDensity = 0.015f;
        reset = false;
        EntityHitVal = 0;
        nextScanWillChase = false;
    }
    private void Update()
    {
        if (scannerUse)
        {
            scanSound.Play();
            if(stageThree && !(bool)chaseSequence.GetProgramVariable("chaseActive")) CheckChaseSequence();
            hasScanCheckedChaseSeq = false;
            entityHitBool = false;
            entityHitCount = 0;
            rayPos = new Vector2(0.0f, 1.0f);
            targetCount = (1 / castDensity) * (1 / castDensity);
            scanCount = 1;
            scanTime = 0;

            if (m_System == null)
                m_System = GetComponent<ParticleSystem>();

            if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
                m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];

            activeScan = true;
            scannerUse = false;
        }
        //if scan is running AND there is less dots than target
        if (activeScan && (scanCount < targetCount))
        {

            m_System.GetParticles(m_Particles);
            //scan speed (multiply by seconds for faster)
            if (true) //scanCount < scanTime
            {
                //enable Line renderer
                scanLine.enabled = true;
                scanLine1.enabled = true;
                scanLine2.enabled = true;
                scanLine3.enabled = true;
                
                //4 scanlines from scanner
                Ray scanLineRay = lidarCam.ViewportPointToRay(new Vector2(scanLineRandX, rayPos.y));
                RaycastHit scanLineHitInfo;
                Physics.Raycast(scanLineRay, out scanLineHitInfo, Mathf.Infinity, scannerLayerMask);
                scanLine.SetPosition(1, scanLineHitInfo.point);

                Ray scanLineRay1 = lidarCam.ViewportPointToRay(new Vector2(scanLineRandX1, rayPos.y));
                RaycastHit scanLineHitInfo1;
                Physics.Raycast(scanLineRay1, out scanLineHitInfo1, Mathf.Infinity, scannerLayerMask);
                scanLine1.SetPosition(1, scanLineHitInfo1.point);

                Ray scanLineRay2 = lidarCam.ViewportPointToRay(new Vector2(scanLineRandX2, rayPos.y));
                RaycastHit scanLineHitInfo2;
                Physics.Raycast(scanLineRay2, out scanLineHitInfo2, Mathf.Infinity, scannerLayerMask);
                scanLine2.SetPosition(1, scanLineHitInfo2.point);

                Ray scanLineRay3 = lidarCam.ViewportPointToRay(new Vector2(scanLineRandX3, rayPos.y));
                RaycastHit scanLineHitInfo3;
                Physics.Raycast(scanLineRay3, out scanLineHitInfo3, Mathf.Infinity, scannerLayerMask);
                scanLine3.SetPosition(1, scanLineHitInfo3.point);

                //if scanline raycasts hit entity rigidbody (or smiler), set color to red. Otherwise set color to white
                if (scanLineHitInfo.rigidbody == entityRB || scanLineHitInfo.rigidbody == entityRB2 ||
                            scanLineHitInfo.rigidbody == entityRB3 || scanLineHitInfo.rigidbody == entityRB4 ||
                            scanLineHitInfo.rigidbody == entityRB5 || scanLineHitInfo.rigidbody == entityRB6 || scanLineHitInfo.rigidbody == smilerRB)
                {
                    scanLine.startColor = Color.red; scanLine.endColor = Color.red;
                }
                else { scanLine.startColor = Color.white; scanLine.endColor = Color.white; }
                if (scanLineHitInfo1.rigidbody == entityRB || scanLineHitInfo1.rigidbody == entityRB2 ||
            scanLineHitInfo1.rigidbody == entityRB3 || scanLineHitInfo1.rigidbody == entityRB4 ||
            scanLineHitInfo1.rigidbody == entityRB5 || scanLineHitInfo1.rigidbody == entityRB6 || scanLineHitInfo1.rigidbody == smilerRB)
                {
                    scanLine1.startColor = Color.red; scanLine1.endColor = Color.red;
                }
                else { scanLine1.startColor = Color.white; scanLine1.endColor = Color.white; }
                if (scanLineHitInfo2.rigidbody == entityRB || scanLineHitInfo2.rigidbody == entityRB2 ||
            scanLineHitInfo2.rigidbody == entityRB3 || scanLineHitInfo2.rigidbody == entityRB4 ||
            scanLineHitInfo2.rigidbody == entityRB5 || scanLineHitInfo2.rigidbody == entityRB6 || scanLineHitInfo2.rigidbody == smilerRB)
                {
                    scanLine2.startColor = Color.red; scanLine2.endColor = Color.red;
                }
                else { scanLine2.startColor = Color.white; scanLine2.endColor = Color.white; }
                if (scanLineHitInfo3.rigidbody == entityRB || scanLineHitInfo3.rigidbody == entityRB2 ||
            scanLineHitInfo3.rigidbody == entityRB3 || scanLineHitInfo3.rigidbody == entityRB4 ||
            scanLineHitInfo3.rigidbody == entityRB5 || scanLineHitInfo3.rigidbody == entityRB6 || scanLineHitInfo3.rigidbody == smilerRB)
                {
                    scanLine3.startColor = Color.red; scanLine3.endColor = Color.red;
                }
                else { scanLine3.startColor = Color.white; scanLine3.endColor = Color.white; }


                //scanTime += scanSpeed * Time.deltaTime;
                for (int i = 0; i < targetCount * castDensity; i++)
                {
                    if (totalParticleCount >= m_Particles.Length)
                        totalParticleCount = 0;
                    Ray ray = lidarCam.ViewportPointToRay(rayPos);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, scannerLayerMask))
                    {
                        //if smiler is hit, activate smiler
                        if (hitInfo.rigidbody == smilerRB)
                        {
                            smilerObj.SetActive(true);
                            scanLine.startColor = Color.red; scanLine.endColor = Color.red;
                            scanLine1.startColor = Color.red; scanLine1.endColor = Color.red;
                            scanLine2.startColor = Color.red; scanLine2.endColor = Color.red;
                            scanLine3.startColor = Color.red; scanLine3.endColor = Color.red;
                        }

                            //If Entity is hit
                            if (hitInfo.rigidbody == entityRB || hitInfo.rigidbody == entityRB2 ||
                            hitInfo.rigidbody == entityRB3 || hitInfo.rigidbody == entityRB4 ||
                            hitInfo.rigidbody == entityRB5 || hitInfo.rigidbody == entityRB6)
                        {
                            scannerScreen.GetComponent<MeshRenderer>().material = redScreen;
                            if (scannerAlarm.activeSelf == false)
                                scannerAlarm.SetActive(true);
                            if (stageTwo && EntityHitVal == 0)
                            {
                                if (hitInfo.rigidbody == entityRB)
                                {
                                    EntityHitVal = 1;
                                }
                                else if (hitInfo.rigidbody == entityRB2)
                                {
                                    EntityHitVal = 2;
                                }
                                else if (hitInfo.rigidbody == entityRB3)
                                {
                                    EntityHitVal = 3;
                                }
                                else if (hitInfo.rigidbody == entityRB4)
                                {
                                    EntityHitVal = 4;
                                }
                                else if (hitInfo.rigidbody == entityRB5)
                                {
                                    EntityHitVal = 5;
                                }
                                else if (hitInfo.rigidbody == entityRB6)
                                {
                                    EntityHitVal = 6;
                                }
                                Debug.Log("particlelidar entityhitval = " + EntityHitVal);
                            }
                            if (!stageTwo)
                            {
                                if (hitInfo.rigidbody == entityRB)
                                {
                                    entityGroan.SetActive(true);
                                }
                                if (hitInfo.rigidbody == entityRB2)
                                {
                                    entity1Groan.SetActive(true);
                                }
                                if (hitInfo.rigidbody == entityRB3)
                                {
                                    entity2Groan.SetActive(true);
                                }
                                if (hitInfo.rigidbody == entityRB4)
                                {
                                    entity3Groan.SetActive(true);
                                }
                                if (hitInfo.rigidbody == entityRB5)
                                {
                                    entity4Groan.SetActive(true);
                                }
                                if (hitInfo.rigidbody == entityRB6)
                                {
                                    entity5Groan.SetActive(true);
                                }

                                m_Particles[totalParticleCount].startColor = Color.red;
                            }
                            if (stageThree && !(bool)chaseSequence.GetProgramVariable("chaseActive") && !hasScanCheckedChaseSeq && nextScanWillChase)
                            {
                                hasScanCheckedChaseSeq = true;
                                nextScanWillChase = false;
                                chaseSequence.SetProgramVariable("chaseBool", (bool)true);
                                Debug.Log("Sent chaseBool = true");
                            }
                            if (!entityHitBool)
                                castDensity = 0.005f;
                            scanHasEntity = true;
                            entityHitBool = true;
                            entityHitCount++;

                            if (entityHitCount >= 80 && !stageTwo)
                            {
                                EntityHit();
                                return;
                            }
                        } else if(entityHitCount != 0 && entityHitBool && !smilerObj.activeSelf)
                        {
                            scanLine.startColor = Color.white; scanLine.endColor = Color.white;
                            scanLine1.startColor = Color.white; scanLine1.endColor = Color.white;
                            scanLine2.startColor = Color.white; scanLine2.endColor = Color.white;
                            scanLine3.startColor = Color.white; scanLine3.endColor = Color.white;
                            entityHitBool = false;
                            castDensity = 0.015f;
                            m_Particles[totalParticleCount].startColor = Color.white;
                        }
                    }
                    //if collider hit
                    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, scannerLayerMask)) {
                        //if its stage Two and an entity was hit, dont draw point, turn on groan for respective entity hit, and change scanner material to red
                        if (stageTwo && entityHitBool == true && !smilerObj.activeSelf)
                        {
                            if (hitInfo.rigidbody == entityRB)
                            {
                                entityGroan.SetActive(true);
                            }
                            else if (hitInfo.rigidbody == entityRB2)
                            {
                                entity1Groan.SetActive(true);
                            }
                            else if (hitInfo.rigidbody == entityRB3)
                            {
                                entity2Groan.SetActive(true);
                            }
                            else if (hitInfo.rigidbody == entityRB4)
                            {
                                entity3Groan.SetActive(true);
                            }
                            else if (hitInfo.rigidbody == entityRB5)
                            {
                                entity4Groan.SetActive(true);
                            }
                            else if (hitInfo.rigidbody == entityRB6)
                            {
                                entity5Groan.SetActive(true);
                            }
                        }
                        else
                        {
                            m_Particles[totalParticleCount].position = hitInfo.point;
                        }
                    }
                    //if ray is not at end of row, move over one column
                    if (rayPos.x < 1.0f)
                    {
                        rayPos.x += castDensity;
                    }
                    else if (rayPos.x >= 1.0f)
                    {
                        i = (int)(targetCount * castDensity);
                        scanCount--;
                    }
                    totalParticleCount++;
                    scanCount++;
                }
                //if ray is at end of row, move down one column, reset row
                if (rayPos.x >= 1.0f)
                {
                    rayPos.x = 0.0f;
                    rayPos.y -= castDensity;
                }
                m_System.SetParticles(m_Particles, m_Particles.Length);
            }
            /*else if (scanCount >= scanTime)
            {
                scanTime += scanSpeed * Time.deltaTime;
            }*/
            //if target dot count is reached
        }
        //runs once scan is over
        else if (activeScan && scanCount >= targetCount)
        {

            if (scannerAlarm.activeSelf == true)
                scannerAlarm.SetActive(false);
            scannerScreen.GetComponent<MeshRenderer>().material = lidarMaterial;
            if (scanHasEntity && stageTwo)
            {
                if (entityHitCount >= 60)
                {
                    SpawnScript.SetProgramVariable("buttonPressed", (bool)true);
                    entityGroan.SetActive(false); entity1Groan.SetActive(false);
                    entity2Groan.SetActive(false); entity3Groan.SetActive(false);
                    entity4Groan.SetActive(false); entity5Groan.SetActive(false);
                }
                scanHasEntity = false;
            }
            scanLine.enabled = false;
            scanLine1.enabled = false;
            scanLine2.enabled = false;
            scanLine3.enabled = false;
            activeScan = false;
            EntityHitVal = 0;
        }

        if (shutdownEvent)
        {
            scanLine.enabled = false;
            scanLine1.enabled = false;
            scanLine2.enabled = false;
            scanLine3.enabled = false;
            if (scannerAlarm.activeSelf == false)
            {
                scannerAlarm.SetActive(true);
                scannerScreen.GetComponent<MeshRenderer>().material = redScreen;
            }
            shutdownTime += Time.deltaTime;
        } if (shutdownTime >= 7.0f)
        {
            castDensity = 0.015f;
            scannerScreen.GetComponent<MeshRenderer>().material = lidarMaterial;
            scannerAlarm.SetActive(false);
            stageTwo = true;
            particleSystemGO.SetActive(true);
            shutdownEvent = false;
            shutdownTime = 0;
        }
        castDensity = 0.015f;
        if (reset)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ResetParticleLidar");
        }
    }

    //entity looks at scanner
    void LateUpdate()
    {
        scanLine.SetPosition(0, lidarCam.transform.position);
        scanLine1.SetPosition(0, lidarCam.transform.position);
        scanLine2.SetPosition(0, lidarCam.transform.position);
        scanLine3.SetPosition(0, lidarCam.transform.position);
        scanLineRandX = Random.Range(0f, 1f);
        scanLineRandX1 = Random.Range(0f, 1f);
        scanLineRandX2 = Random.Range(0f, 1f);
        scanLineRandX3 = Random.Range(0f, 1f);

        Quaternion lookRotation = Quaternion.LookRotation(LidarScanner.transform.position - entity.transform.position);
        lookRotation = Quaternion.Euler(entity.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, entity.transform.rotation.eulerAngles.z);
        entity.transform.rotation = lookRotation;

        lookRotation = Quaternion.LookRotation(LidarScanner.transform.position - entity2.transform.position);
        lookRotation = Quaternion.Euler(entity2.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, entity2.transform.rotation.eulerAngles.z);
        entity2.transform.rotation = lookRotation;

        lookRotation = Quaternion.LookRotation(LidarScanner.transform.position - entity3.transform.position);
        lookRotation = Quaternion.Euler(entity3.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, entity3.transform.rotation.eulerAngles.z);
        entity3.transform.rotation = lookRotation;

        lookRotation = Quaternion.LookRotation(LidarScanner.transform.position - entity4.transform.position);
        lookRotation = Quaternion.Euler(entity4.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, entity4.transform.rotation.eulerAngles.z);
        entity4.transform.rotation = lookRotation;

        lookRotation = Quaternion.LookRotation(LidarScanner.transform.position - entity5.transform.position);
        lookRotation = Quaternion.Euler(entity5.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, entity5.transform.rotation.eulerAngles.z);
        entity5.transform.rotation = lookRotation;

        lookRotation = Quaternion.LookRotation(LidarScanner.transform.position - entity6.transform.position);
        lookRotation = Quaternion.Euler(entity6.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, entity6.transform.rotation.eulerAngles.z);
        entity6.transform.rotation = lookRotation;


        /*entity.transform.LookAt(entity.transform.position - lidarCam.transform.rotation.eulerAngles.y * Vector3.forward,
            lidarCam.transform.rotation.eulerAngles.y * Vector3.up);
        entity2.transform.LookAt(entity2.transform.position - lidarCam.transform.rotation.eulerAngles.y * Vector3.forward,
            lidarCam.transform.rotation.eulerAngles.y * Vector3.up);
        entity3.transform.LookAt(entity3.transform.position - lidarCam.transform.rotation.eulerAngles.y * Vector3.forward,
            lidarCam.transform.rotation.eulerAngles.y * Vector3.up);
        entity4.transform.LookAt(entity4.transform.position - lidarCam.transform.rotation.eulerAngles.y * Vector3.forward,
            lidarCam.transform.rotation.eulerAngles.y * Vector3.up);
        entity5.transform.LookAt(entity5.transform.position - lidarCam.transform.rotation.eulerAngles.y * Vector3.forward,
            lidarCam.transform.rotation.eulerAngles.y * Vector3.up);
        entity6.transform.LookAt(entity6.transform.position - lidarCam.transform.rotation.eulerAngles.y * Vector3.forward,
            lidarCam.transform.rotation.eulerAngles.y * Vector3.up);*/
    }

    public void CheckChaseSequence()
    {
        if (Networking.LocalPlayer.IsOwner(LidarScanner))
        {
            chaseSequenceRandInt = Random.Range(0f, 1f);
            Debug.Log("chaseSequenceRandInt = " + chaseSequenceRandInt);
            if (chaseSequenceRandInt <= chaseSequenceChance)
            {
                Debug.Log("Next scan will chase");
                nextScanWillChase = true;
            }
            else
            {
                nextScanWillChase = false;
            }
        }
    }
    //                chaseSequence.SetProgramVariable("chaseBool", (bool)true);
    //                Debug.Log("Sent chaseBool = true");
    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ScannerPickupDown");
    }
    public override void OnPickup()
    {
        if (!Networking.IsOwner(LidarScanner))
        {
            Networking.SetOwner(Networking.LocalPlayer, LidarScanner);
            Debug.Log(Networking.LocalPlayer.displayName + " is new Owner of scanner");
        }
        if (!Networking.IsOwner(SpawnButton))
        {
            Networking.SetOwner(Networking.LocalPlayer, SpawnButton);
            Debug.Log(Networking.LocalPlayer.displayName + " is new Owner of spawnbutton");
        }
        if (!Networking.IsOwner(entity))
        {
            Networking.SetOwner(Networking.LocalPlayer, entity);
        }
        if (!Networking.IsOwner(entity2))
        {
            Networking.SetOwner(Networking.LocalPlayer, entity2);
        }
        if (!Networking.IsOwner(entity3))
        {
            Networking.SetOwner(Networking.LocalPlayer, entity3);
        }
        if (!Networking.IsOwner(entity4))
        {
            Networking.SetOwner(Networking.LocalPlayer, entity4);
        }
        if (!Networking.IsOwner(entity5))
        {
            Networking.SetOwner(Networking.LocalPlayer, entity5);
        }
        if (!Networking.IsOwner(entity6))
        {
            Networking.SetOwner(Networking.LocalPlayer, entity6);
        }
    }
    public void ScannerPickupDown()
    {
        if (!activeScan && !shutdownEvent)
        {
            scannerUse = true;
        }
    }



    private void EntityHit()
    {
        activeScan = false;
        for (int i = 0; i < m_System.main.maxParticles; i++)
        {
            m_Particles[i].startColor = Color.white;
            m_Particles[i].position = particleSystemGO.transform.position;
        }
        if (!scareSFX.isPlaying)
            scareSFX.Play();
        shutdownSound.Play();
        particleSystemGO.SetActive(false);
        entity.SetActive(false);
        shutdownEvent = true;
    }


}
