
using UdonSharp;
using System.Collections;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LiDAR : UdonSharpBehaviour
{

    public float speed = 40;
    public GameObject bullet;
    public Transform barrel;
    //public AudioSource audioSource;
    //public AudioClip audioClip;

    public override void OnPickupUseDown()
    {
        for (int i = 0; i < 100; i++)
        {
            if(i % 10 == 0) {
                //reset barrel x rotation
                //barrel.Rotate(-100.0f, 0.0f, 0.0f, Space.Self);
                barrel.Translate(0,0,-2);
                //proceed to next row
                barrel.Rotate(0.0f, 0.0f, -4.0f, Space.Self);
            }
            var spawnedBullet = VRCInstantiate(bullet);
            //barrel.Rotate(10.0f, 0.0f, 0.0f, Space.Self);
            barrel.Translate(0, 0, 0.2f);
            //set spawned bullet location to barrel location
            spawnedBullet.transform.position = new Vector3(barrel.position.x,
                barrel.position.y - 0.1f, barrel.position.z);
            //add velocity to bullet in direction of barrel pointing
            spawnedBullet.GetComponent<Rigidbody>().AddForce(barrel.up * speed);// = speed * barrel.forward;
                                                                                   //audioSource.PlayOneShot(audioClip);
        }
        //reset barrel x and z rotation at end of scan
        barrel.localRotation = Quaternion.Euler(0, 0, 16);
        barrel.localPosition = new Vector3(0, 0.556f, 16);
    }
}


