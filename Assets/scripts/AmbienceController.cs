
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AmbienceController : UdonSharpBehaviour
{
    public AudioSource ambience1;
    public AudioSource ambience2;
    public AudioSource ambience3;
    public AudioSource ambience4;
    public AudioSource ambience5;
    public AudioSource ambience6;
    public AudioSource ambience7;
    public AudioSource ambience8;
    public AudioSource ambience9;
    public AudioSource ambience10;
    public AudioSource ambience11;
    public AudioSource ambience12;


    public UdonBehaviour SpawnScript;
    public UdonBehaviour ParticleLidar;
    public UdonBehaviour chaseSequence;
    [UdonSynced]
    private float ambienceTimer = 0;
    private bool randomRespawning = false;

    private void Update()
    {
        if ((bool)ParticleLidar.GetProgramVariable("stageTwo"))
        {
            ambienceTimer += Time.deltaTime;

            switch ((int)ambienceTimer)
            {
                case 80:
                    ambience2.Play();
                    break;
                case 180:
                    ambience1.Play();
                    break;
                case 280:
                    ambience4.Play();
                    break;
                case 430:
                    ambience3.Play();
                    break;
                case 680:
                    ambience5.Play();
                    break;
                case 780:
                    ambience6.Play();
                    break;
                case 850:
                    ambience7.Play();
                    break;
                case 920:
                    ambience8.Play();
                    break;
                case 1000:
                    ambience9.Play();
                    break;
                case 1200:
                    ambience10.Play();
                    break;
                case 1330:
                    ambience11.Play();
                    break;
                case 1500:
                    ambience12.Play();
                    ambienceTimer = 0;
                    break;
            }

            if ((int)ambienceTimer % 25 == 0 && !(bool)chaseSequence.GetProgramVariable("chaseBool"))
            {
                if (!randomRespawning)
                {
                    randomRespawning = true;
                    SpawnScript.SetProgramVariable("buttonPressed", (bool)true);
                }
            }
            else
            {
                randomRespawning = false;
            }
        }
    }
}
