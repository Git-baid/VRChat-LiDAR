
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RayGun : UdonSharpBehaviour
{
    public Camera lidarCam;
    public float castDensity = 0.01f;
    public int scanSpeed = 100;
    public GameObject lidarDot;
    private bool activeScan = false;
    private float scanTime;
    private int scanCount;
    private float targetCount;
    Vector2 rayPos;

    private void Update()
    {
        //if scan is running AND there is less dots than target
        if (activeScan && (scanCount < targetCount))
        {
            //scan speed (multiply by seconds for faster)
            if (scanCount < scanTime)
            {
                scanTime += scanSpeed * Time.deltaTime;
                for (int i = 0; i < targetCount*castDensity; i++)
                {
                    Ray ray = lidarCam.ViewportPointToRay(rayPos);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        var newDot = VRCInstantiate(lidarDot);
                        newDot.transform.position = hitInfo.point;
                    }
                    //if ray is not at end of row, move over one column
                    if (rayPos.x < 1.0f)
                    {
                        rayPos.x += castDensity;
                    }
                    scanCount++;
                }
                //if ray is at end of row, move down one column, reset row
                if (rayPos.x >= 1.0f)
                {
                    rayPos.x = 0.0f;
                    rayPos.y -= castDensity;
                }
            } else if(scanCount >= scanTime)
            {
                scanTime += scanSpeed * Time.deltaTime;
            }
        //if target dot count is reached
        }else if(activeScan && scanCount >= targetCount)
        {
            activeScan = false;
        }
    }
    public override void OnPickupUseDown()
    {
        if (!activeScan)
            CameraCast();
    }

    private void CameraCast()
    {
        rayPos = new Vector2(0.0f, 1.0f);
        targetCount = (1 / castDensity) * (1 / castDensity);
        Debug.Log(targetCount);
        scanCount = 1;
        scanTime = 0;
        activeScan = true;
    }

}
