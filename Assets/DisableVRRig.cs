using UnityEngine;
using UnityEditor;

public class DisableVRRig
{
    public static void Execute()
    {
        GameObject vrRig = GameObject.Find("VR Rig");
        if (vrRig != null)
        {
            vrRig.SetActive(false);
            Debug.Log("Disabled VR Rig");
        }
        else
        {
            Debug.Log("VR Rig not found (maybe already disabled or renamed)");
        }

        GameObject desktopCam = GameObject.Find("DesktopCamera");
        if (desktopCam != null)
        {
            desktopCam.SetActive(true);
            desktopCam.tag = "MainCamera";
            Debug.Log("Enabled DesktopCamera and tagged as MainCamera");
        }
    }
}
