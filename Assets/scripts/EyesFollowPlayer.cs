
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EyesFollowPlayer : UdonSharpBehaviour
{
    public GameObject eye0;

    private void Update()
    {
        eye0.transform.LookAt(Networking.LocalPlayer.GetPosition());
    }
}
