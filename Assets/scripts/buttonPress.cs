
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class buttonPress : UdonSharpBehaviour
{

    public Transform lidarScanner;
    public AudioSource buttonPressSound;
    public GameObject Button;

    public UdonBehaviour SpawnScript;
    public AudioSource shutdownSound;

    public UdonBehaviour ButtonCount;
    public UdonBehaviour smiler;

    //VRCPlayerApi[] Players = new VRCPlayerApi[8];

    public override void Interact()
    {
        Button.SetActive(false);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ButtonPress");
    }

    public void ButtonPress()
    {
        buttonPressSound.Play();
        Button.SetActive(false);
        SpawnScript.SetProgramVariable("buttonPressed", (bool)true);
        ButtonCount.SetProgramVariable("buttonCountInc", (bool)true);
        smiler.SetProgramVariable("respawnSmiler", (bool)true);
    }

}
