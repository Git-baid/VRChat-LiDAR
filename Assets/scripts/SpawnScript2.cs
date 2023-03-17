
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
//using UnityEngine.UI;

public class SpawnScript2 : UdonSharpBehaviour
{
    public GameObject lidarScanner;
    public GameObject VRCWorldPos;

    public Transform PlayerSpawn;
    public GameObject entity;
    public GameObject entity2;
    public GameObject entity3;
    public GameObject entity4;
    public GameObject entity5;
    public GameObject entity6;
    public Transform EntitySpawn1;
    public Transform EntitySpawn2;
    public Transform EntitySpawn3;
    public Transform EntitySpawn4;

    public Transform SEntitySpawn1;
    public Transform SEntitySpawn2;
    public Transform SEntitySpawn3;
    public Transform SEntitySpawn4;
    public Transform SEntitySpawn5;
    public Transform SEntitySpawn6;
    public Transform SEntitySpawn7;
    public Transform SEntitySpawn8;
    public Transform SEntitySpawn9;
    public Transform SEntitySpawn10;
    public Transform SEntitySpawn11;
    public Transform SEntitySpawn12;
    public Transform SEntitySpawn13;
    public Transform SEntitySpawn14;
    public Transform SEntitySpawn15;
    public Transform SEntitySpawn16;
    public Transform SEntitySpawn17;
    public Transform SEntitySpawn18;
    public Transform SEntitySpawn19;
    public Transform SEntitySpawn20;
    public Transform SEntitySpawn21;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;

    public Transform ButtonSpawn1;
    public Transform ButtonSpawn2;
    public Transform ButtonSpawn3;
    public Transform ButtonSpawn4;
    public Transform ButtonSpawn5;
    public Transform ButtonSpawn6;
    public Transform ButtonSpawn7;
    public Transform ButtonSpawn8;
    public Transform ButtonSpawn9;
    public Transform ButtonSpawn10;
    public Transform ButtonSpawn11;


    [UdonSynced]
    private int button1spawn;
    [UdonSynced]
    private int button2spawn;
    [UdonSynced]
    private int button3spawn;
    [UdonSynced]
    private int button4spawn;
    [UdonSynced]
    private int button5spawn;


    public bool buttonPressed = false;

    [UdonSynced]
    private bool start = false;

    [UdonSynced]
    private int ES1;
    [UdonSynced]
    private int ES2;
    [UdonSynced]
    private int ES3;
    [UdonSynced]
    private int ES4;
    [UdonSynced]
    private int ES5;
    [UdonSynced]
    private int ES6;

    //public Text EntitySpawnIDs;

    //VRCPlayerApi[] Players = new VRCPlayerApi[8];

    public AudioSource monomatJingle;

    private Vector3 vrcWorldOffset = new Vector3(0, -0.3f, 0);
    private Vector3 posOffset = new Vector3(-9, 4.5f, -2);

    public override void Interact()
    {

        VRCWorldPos.transform.position = PlayerSpawn.transform.position;

        start = true;

        //lidarScanner.transform.rotation = PlayerSpawn.rotation;


        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TeleportAllPlayersStart");
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EntitySpawnBP0");


        //set synced random button spawn locations
        button1spawn = Random.Range(0, 11);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Button1");
        do
        {
            button2spawn = Random.Range(0, 11);
        } while (button1spawn == button2spawn);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Button2");
        do
        {
            button3spawn = Random.Range(0, 11);
        } while (button3spawn == button2spawn ||
        button3spawn == button1spawn);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Button3");
        do
        {
            button4spawn = Random.Range(0, 11);
        } while (button4spawn == button3spawn ||
        button4spawn == button2spawn ||
        button4spawn == button1spawn);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Button4");
        do
        {
            button5spawn = Random.Range(0, 11);
        } while (button5spawn == button4spawn ||
        button5spawn == button3spawn ||
        button5spawn == button2spawn ||
        button5spawn == button1spawn);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Button5");


    }

    public void TeleportAllPlayersStart()
    {
        Networking.LocalPlayer.TeleportTo(PlayerSpawn.position, PlayerSpawn.rotation);
        lidarScanner.transform.position = PlayerSpawn.position + new Vector3(1, 0.7f, 0);
        monomatJingle.Stop();
    }

    private void Update()
    {
        if (start)
        {
            if (VRCWorldPos.transform.position != PlayerSpawn.transform.position)
                VRCWorldPos.transform.position = PlayerSpawn.transform.position;
        }
        if (buttonPressed)
        {
            RandomEntityGen();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EntitySpawnBP1");

            //EntitySpawnIDs.text = ES1.ToString() + " - " + ES2.ToString() + " - " + ES3.ToString() + " - " + ES4.ToString() + " - " + ES5.ToString() + " - " + ES6.ToString();
            buttonPressed = false;
        }
    }

    public void Button1()
    {
        //random button 1 spawns
        switch (button1spawn)
        {
            case 0:
                button1.transform.position = ButtonSpawn1.position + posOffset;
                break;
            case 1:
                button1.transform.position = ButtonSpawn2.position + posOffset;
                break;
            case 2:
                button1.transform.position = ButtonSpawn3.position + posOffset;
                break;
            case 3:
                button1.transform.position = ButtonSpawn4.position + posOffset;
                break;
            case 4:
                button1.transform.position = ButtonSpawn5.position + posOffset;
                break;
            case 5:
                button1.transform.position = ButtonSpawn6.position + posOffset;
                break;
            case 6:
                button1.transform.position = ButtonSpawn7.position + posOffset;
                break;
            case 7:
                button1.transform.position = ButtonSpawn8.position + posOffset;
                break;
            case 8:
                button1.transform.position = ButtonSpawn9.position + posOffset;
                break;
            case 9:
                button1.transform.position = ButtonSpawn10.position + posOffset;
                break;
            case 10:
                button1.transform.position = ButtonSpawn11.position + posOffset;
                break;
        }
    }
    public void Button2()
    {

        //random button 2 spawns
        switch (button2spawn)
        {
            case 0:
                button2.transform.position = ButtonSpawn1.position + posOffset;
                break;
            case 1:
                button2.transform.position = ButtonSpawn2.position + posOffset;
                break;
            case 2:
                button2.transform.position = ButtonSpawn3.position + posOffset;
                break;
            case 3:
                button2.transform.position = ButtonSpawn4.position + posOffset;
                break;
            case 4:
                button2.transform.position = ButtonSpawn5.position + posOffset;
                break;
            case 5:
                button2.transform.position = ButtonSpawn6.position + posOffset;
                break;
            case 6:
                button2.transform.position = ButtonSpawn7.position + posOffset;
                break;
            case 7:
                button2.transform.position = ButtonSpawn8.position + posOffset;
                break;
            case 8:
                button2.transform.position = ButtonSpawn9.position + posOffset;
                break;
            case 9:
                button2.transform.position = ButtonSpawn10.position + posOffset;
                break;
            case 10:
                button2.transform.position = ButtonSpawn11.position + posOffset;
                break;
        }
    }
    public void Button3()
    {

        //random button 3 spawns
        switch (button3spawn)
        {
            case 0:
                button3.transform.position = ButtonSpawn1.position + posOffset;
                break;
            case 1:
                button3.transform.position = ButtonSpawn2.position + posOffset;
                break;
            case 2:
                button3.transform.position = ButtonSpawn3.position + posOffset;
                break;
            case 3:
                button3.transform.position = ButtonSpawn4.position + posOffset;
                break;
            case 4:
                button3.transform.position = ButtonSpawn5.position + posOffset;
                break;
            case 5:
                button3.transform.position = ButtonSpawn6.position + posOffset;
                break;
            case 6:
                button3.transform.position = ButtonSpawn7.position + posOffset;
                break;
            case 7:
                button3.transform.position = ButtonSpawn8.position + posOffset;
                break;
            case 8:
                button3.transform.position = ButtonSpawn9.position + posOffset;
                break;
            case 9:
                button3.transform.position = ButtonSpawn10.position + posOffset;
                break;
            case 10:
                button3.transform.position = ButtonSpawn11.position + posOffset;
                break;
        }
    }
    public void Button4()
    {

        //random button 4 spawns
        switch (button4spawn)
        {
            case 0:
                button4.transform.position = ButtonSpawn1.position + posOffset;
                break;
            case 1:
                button4.transform.position = ButtonSpawn2.position + posOffset;
                break;
            case 2:
                button4.transform.position = ButtonSpawn3.position + posOffset;
                break;
            case 3:
                button4.transform.position = ButtonSpawn4.position + posOffset;
                break;
            case 4:
                button4.transform.position = ButtonSpawn5.position + posOffset;
                break;
            case 5:
                button4.transform.position = ButtonSpawn6.position + posOffset;
                break;
            case 6:
                button4.transform.position = ButtonSpawn7.position + posOffset;
                break;
            case 7:
                button4.transform.position = ButtonSpawn8.position + posOffset;
                break;
            case 8:
                button4.transform.position = ButtonSpawn9.position + posOffset;
                break;
            case 9:
                button4.transform.position = ButtonSpawn10.position + posOffset;
                break;
            case 10:
                button4.transform.position = ButtonSpawn11.position + posOffset;
                break;
        }
    }
    public void Button5()
    {

        //random button 5 spawns
        switch (button5spawn)
        {
            case 0:
                button5.transform.position = ButtonSpawn1.position + posOffset;
                break;
            case 1:
                button5.transform.position = ButtonSpawn2.position + posOffset;
                break;
            case 2:
                button5.transform.position = ButtonSpawn3.position + posOffset;
                break;
            case 3:
                button5.transform.position = ButtonSpawn4.position + posOffset;
                break;
            case 4:
                button5.transform.position = ButtonSpawn5.position + posOffset;
                break;
            case 5:
                button5.transform.position = ButtonSpawn6.position + posOffset;
                break;
            case 6:
                button5.transform.position = ButtonSpawn7.position + posOffset;
                break;
            case 7:
                button5.transform.position = ButtonSpawn8.position + posOffset;
                break;
            case 8:
                button5.transform.position = ButtonSpawn9.position + posOffset;
                break;
            case 9:
                button5.transform.position = ButtonSpawn10.position + posOffset;
                break;
            case 10:
                button5.transform.position = ButtonSpawn11.position + posOffset;
                break;
        }
    }

    public void EntitySpawnBP0()
    {
        entity.transform.position = EntitySpawn1.position;
        entity2.transform.position = EntitySpawn2.position;
        entity3.transform.position = EntitySpawn3.position;
        entity4.transform.position = EntitySpawn4.position;

        /*//random starter Entity spawns
        switch (Random.Range(0, 4))
        {
            case 0:
                entity.transform.position = EntitySpawn1.position;
                break;
            case 1:
                entity.transform.position = EntitySpawn2.position;
                break;
            case 2:
                entity.transform.position = EntitySpawn3.position;
                break;
            case 3:
                entity.transform.position = EntitySpawn4.position;
                break;
        }*/
    }
    public void EntitySpawnBP1()
    {

        entity.SetActive(true);

        //random secondary Entity spawns
        switch (ES1)
        {
            case 0:
                entity.transform.position = SEntitySpawn1.position;
                break;
            case 1:
                entity.transform.position = SEntitySpawn2.position;
                break;
            case 2:
                entity.transform.position = SEntitySpawn3.position;
                break;
            case 3:
                entity.transform.position = SEntitySpawn4.position;
                break;
            case 4:
                entity.transform.position = SEntitySpawn5.position;
                break;
            case 5:
                entity.transform.position = SEntitySpawn6.position;
                break;
            case 6:
                entity.transform.position = SEntitySpawn7.position;
                break;
            case 7:
                entity.transform.position = SEntitySpawn8.position;
                break;
            case 8:
                entity.transform.position = SEntitySpawn9.position;
                break;
            case 9:
                entity.transform.position = SEntitySpawn10.position;
                break;
            case 10:
                entity.transform.position = SEntitySpawn11.position;
                break;
            case 11:
                entity.transform.position = SEntitySpawn12.position;
                break;
            case 12:
                entity.transform.position = SEntitySpawn13.position;
                break;
            case 13:
                entity.transform.position = SEntitySpawn14.position;
                break;
            case 14:
                entity.transform.position = SEntitySpawn15.position;
                break;
            case 15:
                entity.transform.position = SEntitySpawn16.position;
                break;
            case 16:
                entity.transform.position = SEntitySpawn17.position;
                break;
            case 17:
                entity.transform.position = SEntitySpawn18.position;
                break;
            case 18:
                entity.transform.position = SEntitySpawn19.position;
                break;
            case 19:
                entity.transform.position = SEntitySpawn20.position;
                break;
            case 20:
                entity.transform.position = SEntitySpawn21.position;
                break;
        }
        switch (ES2)
        {
            case 0:
                entity2.transform.position = SEntitySpawn1.position;
                break;
            case 1:
                entity2.transform.position = SEntitySpawn2.position;
                break;
            case 2:
                entity2.transform.position = SEntitySpawn3.position;
                break;
            case 3:
                entity2.transform.position = SEntitySpawn4.position;
                break;
            case 4:
                entity2.transform.position = SEntitySpawn5.position;
                break;
            case 5:
                entity2.transform.position = SEntitySpawn6.position;
                break;
            case 6:
                entity2.transform.position = SEntitySpawn7.position;
                break;
            case 7:
                entity2.transform.position = SEntitySpawn8.position;
                break;
            case 8:
                entity2.transform.position = SEntitySpawn9.position;
                break;
            case 9:
                entity2.transform.position = SEntitySpawn10.position;
                break;
            case 10:
                entity2.transform.position = SEntitySpawn11.position;
                break;
            case 11:
                entity2.transform.position = SEntitySpawn12.position;
                break;
            case 12:
                entity2.transform.position = SEntitySpawn13.position;
                break;
            case 13:
                entity2.transform.position = SEntitySpawn14.position;
                break;
            case 14:
                entity2.transform.position = SEntitySpawn15.position;
                break;
            case 15:
                entity2.transform.position = SEntitySpawn16.position;
                break;
            case 16:
                entity2.transform.position = SEntitySpawn17.position;
                break;
            case 17:
                entity2.transform.position = SEntitySpawn18.position;
                break;
            case 18:
                entity2.transform.position = SEntitySpawn19.position;
                break;
            case 19:
                entity2.transform.position = SEntitySpawn20.position;
                break;
            case 20:
                entity2.transform.position = SEntitySpawn21.position;
                break;
        }
        switch (ES3)
        {
            case 0:
                entity3.transform.position = SEntitySpawn1.position;
                break;
            case 1:
                entity3.transform.position = SEntitySpawn2.position;
                break;
            case 2:
                entity3.transform.position = SEntitySpawn3.position;
                break;
            case 3:
                entity3.transform.position = SEntitySpawn4.position;
                break;
            case 4:
                entity3.transform.position = SEntitySpawn5.position;
                break;
            case 5:
                entity3.transform.position = SEntitySpawn6.position;
                break;
            case 6:
                entity3.transform.position = SEntitySpawn7.position;
                break;
            case 7:
                entity3.transform.position = SEntitySpawn8.position;
                break;
            case 8:
                entity3.transform.position = SEntitySpawn9.position;
                break;
            case 9:
                entity3.transform.position = SEntitySpawn10.position;
                break;
            case 10:
                entity3.transform.position = SEntitySpawn11.position;
                break;
            case 11:
                entity3.transform.position = SEntitySpawn12.position;
                break;
            case 12:
                entity3.transform.position = SEntitySpawn13.position;
                break;
            case 13:
                entity3.transform.position = SEntitySpawn14.position;
                break;
            case 14:
                entity3.transform.position = SEntitySpawn15.position;
                break;
            case 15:
                entity3.transform.position = SEntitySpawn16.position;
                break;
            case 16:
                entity3.transform.position = SEntitySpawn17.position;
                break;
            case 17:
                entity3.transform.position = SEntitySpawn18.position;
                break;
            case 18:
                entity3.transform.position = SEntitySpawn19.position;
                break;
            case 19:
                entity3.transform.position = SEntitySpawn20.position;
                break;
            case 20:
                entity3.transform.position = SEntitySpawn21.position;
                break;
        }
        switch (ES4)
        {
            case 0:
                entity4.transform.position = SEntitySpawn1.position;
                break;
            case 1:
                entity4.transform.position = SEntitySpawn2.position;
                break;
            case 2:
                entity4.transform.position = SEntitySpawn3.position;
                break;
            case 3:
                entity4.transform.position = SEntitySpawn4.position;
                break;
            case 4:
                entity4.transform.position = SEntitySpawn5.position;
                break;
            case 5:
                entity4.transform.position = SEntitySpawn6.position;
                break;
            case 6:
                entity4.transform.position = SEntitySpawn7.position;
                break;
            case 7:
                entity4.transform.position = SEntitySpawn8.position;
                break;
            case 8:
                entity4.transform.position = SEntitySpawn9.position;
                break;
            case 9:
                entity4.transform.position = SEntitySpawn10.position;
                break;
            case 10:
                entity4.transform.position = SEntitySpawn11.position;
                break;
            case 11:
                entity4.transform.position = SEntitySpawn12.position;
                break;
            case 12:
                entity4.transform.position = SEntitySpawn13.position;
                break;
            case 13:
                entity4.transform.position = SEntitySpawn14.position;
                break;
            case 14:
                entity4.transform.position = SEntitySpawn15.position;
                break;
            case 15:
                entity4.transform.position = SEntitySpawn16.position;
                break;
            case 16:
                entity4.transform.position = SEntitySpawn17.position;
                break;
            case 17:
                entity4.transform.position = SEntitySpawn18.position;
                break;
            case 18:
                entity4.transform.position = SEntitySpawn19.position;
                break;
            case 19:
                entity4.transform.position = SEntitySpawn20.position;
                break;
            case 20:
                entity4.transform.position = SEntitySpawn21.position;
                break;
        }
        switch (ES5)
        {
            case 0:
                entity5.transform.position = SEntitySpawn1.position;
                break;
            case 1:
                entity5.transform.position = SEntitySpawn2.position;
                break;
            case 2:
                entity5.transform.position = SEntitySpawn3.position;
                break;
            case 3:
                entity5.transform.position = SEntitySpawn4.position;
                break;
            case 4:
                entity5.transform.position = SEntitySpawn5.position;
                break;
            case 5:
                entity5.transform.position = SEntitySpawn6.position;
                break;
            case 6:
                entity5.transform.position = SEntitySpawn7.position;
                break;
            case 7:
                entity5.transform.position = SEntitySpawn8.position;
                break;
            case 8:
                entity5.transform.position = SEntitySpawn9.position;
                break;
            case 9:
                entity5.transform.position = SEntitySpawn10.position;
                break;
            case 10:
                entity5.transform.position = SEntitySpawn11.position;
                break;
            case 11:
                entity5.transform.position = SEntitySpawn12.position;
                break;
            case 12:
                entity5.transform.position = SEntitySpawn13.position;
                break;
            case 13:
                entity5.transform.position = SEntitySpawn14.position;
                break;
            case 14:
                entity5.transform.position = SEntitySpawn15.position;
                break;
            case 15:
                entity5.transform.position = SEntitySpawn16.position;
                break;
            case 16:
                entity5.transform.position = SEntitySpawn17.position;
                break;
            case 17:
                entity5.transform.position = SEntitySpawn18.position;
                break;
            case 18:
                entity5.transform.position = SEntitySpawn19.position;
                break;
            case 19:
                entity5.transform.position = SEntitySpawn20.position;
                break;
            case 20:
                entity5.transform.position = SEntitySpawn21.position;
                break;
        }
        switch (ES6)
        {
            case 0:
                entity6.transform.position = SEntitySpawn1.position;
                break;
            case 1:
                entity6.transform.position = SEntitySpawn2.position;
                break;
            case 2:
                entity6.transform.position = SEntitySpawn3.position;
                break;
            case 3:
                entity6.transform.position = SEntitySpawn4.position;
                break;
            case 4:
                entity6.transform.position = SEntitySpawn5.position;
                break;
            case 5:
                entity6.transform.position = SEntitySpawn6.position;
                break;
            case 6:
                entity6.transform.position = SEntitySpawn7.position;
                break;
            case 7:
                entity6.transform.position = SEntitySpawn8.position;
                break;
            case 8:
                entity6.transform.position = SEntitySpawn9.position;
                break;
            case 9:
                entity6.transform.position = SEntitySpawn10.position;
                break;
            case 10:
                entity6.transform.position = SEntitySpawn11.position;
                break;
            case 11:
                entity6.transform.position = SEntitySpawn12.position;
                break;
            case 12:
                entity6.transform.position = SEntitySpawn13.position;
                break;
            case 13:
                entity6.transform.position = SEntitySpawn14.position;
                break;
            case 14:
                entity6.transform.position = SEntitySpawn15.position;
                break;
            case 15:
                entity6.transform.position = SEntitySpawn16.position;
                break;
            case 16:
                entity6.transform.position = SEntitySpawn17.position;
                break;
            case 17:
                entity6.transform.position = SEntitySpawn18.position;
                break;
            case 18:
                entity6.transform.position = SEntitySpawn19.position;
                break;
            case 19:
                entity6.transform.position = SEntitySpawn20.position;
                break;
            case 20:
                entity6.transform.position = SEntitySpawn21.position;
                break;
        }
    }

    public void RandomEntityGen()
    {
        if (!Networking.LocalPlayer.IsOwner(lidarScanner)) return;
        do
        {
            ES1 = Random.Range(0, 21);
            ES2 = Random.Range(0, 21);
        } while (ES1 == ES2);
        do
        {
            ES3 = Random.Range(0, 21);
        } while (ES3 == ES2 || ES3 == ES1);
        do
        {
            ES4 = Random.Range(0, 21);
        } while (ES4 == ES3 || ES4 == ES2 || ES4 == ES1);
        do
        {
            ES5 = Random.Range(0, 21);
        } while (ES5 == ES4 || ES5 == ES3 || ES5 == ES2 || ES5 == ES1);
        do
        {
            ES6 = Random.Range(0, 21);
        } while (ES6 == ES5 || ES6 == ES4 || ES6 == ES3 || ES6 == ES2 || ES6 == ES1);
        RequestSerialization();
    }

}

