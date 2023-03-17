
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Ambience : UdonSharpBehaviour
{
    public AudioSource ambience1;
    public AudioSource ambience2;
    public AudioSource ambience3;

    public UdonBehaviour SpawnScript;
    [UdonSynced]
    private float ambienceTimer = 0;
    private void Update()
    {
        if ((bool)SpawnScript.GetProgramVariable("start"))
        {
            ambienceTimer += Time.deltaTime;
            if (ambienceTimer == 280)
            {
                ambience1.Play();
            }
            else if (ambienceTimer == 80)
            {
                ambience2.Play();
            }
            else if (ambienceTimer == 430)
            {
                ambience3.Play();
            }
            else if (ambienceTimer >= 600)
            {
                ambienceTimer = 0;
            }
        }
    }
}
