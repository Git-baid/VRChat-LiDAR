
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ResetGame : UdonSharpBehaviour
{

    public UdonBehaviour GameTimer;

    public UdonBehaviour chaseSequence;
    public UdonBehaviour ButtonCount;

    public UdonBehaviour SpawnScript;
    public UdonBehaviour ParticleLidar;
    public UdonBehaviour ambienceController;
    public UdonBehaviour Smiler;

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ResetGameVariables");
    }

    public void ResetGameVariables()
    {
        Debug.Log("resetting game");
        ParticleLidar.SetProgramVariable("reset", (bool)true);
        SpawnScript.SetProgramVariable("reset", (bool)true);
        ButtonCount.SetProgramVariable("reset", (bool)true);
        chaseSequence.SetProgramVariable("reset", (bool)true);
        GameTimer.SetProgramVariable("gameTimer", 0.0f);
        ambienceController.SetProgramVariable("ambienceTimer", 0);
        Smiler.SetProgramVariable("reset", (bool)true);

    }
}
