
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class ButtonCount : UdonSharpBehaviour
{
    public bool buttonCountInc = false;
    private int buttonCount = 0;
    public Text buttonCountText;
    public Transform endSpawn;
    public AudioSource shutdownSound;
    public AudioSource endMusic;

    public UdonBehaviour ParticleLidar;
    public UdonBehaviour SpawnScript;

    public AudioSource buttonSound;
    public AudioSource button1Sound;
    public AudioSource button2Sound;
    public AudioSource button3Sound;
    public AudioSource button4Sound;

    public VRC_Pickup Pickup;

    public bool reset;

    public void ResetButtonCount()
    {
        Debug.Log("Resetting ButtonCount");
        buttonCount = 0;
        buttonCountText.text = "0/5";
        shutdownSound.Stop();
        endMusic.Stop();
        reset = false;
        buttonSound.maxDistance = 21; button1Sound.maxDistance = 21;
        button2Sound.maxDistance = 21; button3Sound.maxDistance = 21;
        button4Sound.maxDistance = 21;
    }

    private void Update()
    {
        if (buttonCountInc)
        {
            buttonCount++;
            buttonCountText.text = (buttonCount + "/5");
            Debug.Log("buttonCount = " + buttonCount);
            buttonCountInc = false;
        }

        if (buttonCount >= 5)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EndGame");
            buttonCount = 0;
        }
        if (buttonCount == 4)
        {
            if (buttonSound.maxDistance != 100)
            {
                buttonSound.maxDistance = 100; button1Sound.maxDistance = 100;
                button2Sound.maxDistance = 100; button3Sound.maxDistance = 100;
                button4Sound.maxDistance = 100;
            }
        }

        if (buttonCount == 3)
        {
            if (buttonSound.maxDistance != 50)
            {
                buttonSound.maxDistance = 50; button1Sound.maxDistance = 50;
                button2Sound.maxDistance = 50; button3Sound.maxDistance = 50;
                button4Sound.maxDistance = 50;
            }
        }

        if (buttonCount == 2)
        {
            if ((bool)ParticleLidar.GetProgramVariable("stageThree") != true)
                ParticleLidar.SetProgramVariable("stageThree", (bool)true);
        }

        if (reset)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ResetButtonCount");
        }
    }


    public void EndGame()
    {
        Pickup.Drop();
        Networking.LocalPlayer.TeleportTo(endSpawn.position, endSpawn.rotation);
        SpawnScript.SetProgramVariable("start", (bool)false);
        ParticleLidar.SetProgramVariable("stageTwo", (bool)false);
        shutdownSound.Stop();
        endMusic.Play();
    }
}
