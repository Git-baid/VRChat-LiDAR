
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using System;
public class GameTimer : UdonSharpBehaviour
{

    [UdonSynced]
    public float gameTimer = 0;
    public UdonBehaviour SpawnScript;
    public Text timerText;
    public Text deathTimerText;
    void Update()
    {
        if ((bool)SpawnScript.GetProgramVariable("start")){
            gameTimer += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(gameTimer);
        timerText.text = time.ToString(@"mm\:ss\:fff");
        deathTimerText.text = time.ToString(@"mm\:ss\:fff");
    }
}
