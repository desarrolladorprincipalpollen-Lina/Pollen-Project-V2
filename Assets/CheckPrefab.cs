using UnityEngine;
using UnityEditor;
using PollenModule;

public class CheckPrefab
{
    public static void Execute()
    {
        string path = "Assets/Modules/Game_1_Pollen/Assets/Prefabs/Working table/Pickables/Screwdrivers/Screwdriver_Single yellow.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab != null)
        {
            Debug.Log("Prefab found: " + prefab.name);
            if (prefab.GetComponent<DesktopGrabbable>() != null)
                Debug.Log("DesktopGrabbable present");
            else
                Debug.Log("DesktopGrabbable MISSING");

            if (prefab.GetComponent<Rigidbody>() != null)
                Debug.Log("Rigidbody present");
            else
                Debug.Log("Rigidbody MISSING");
                
            if (prefab.GetComponent<Collider>() != null)
                Debug.Log("Collider present");
            else
                Debug.Log("Collider MISSING");
        }
        else
        {
            Debug.LogError("Prefab not found at path: " + path);
        }
    }
}
