using UnityEngine;
using UnityEditor;
using PollenModule;
using System.Collections.Generic;
using System.IO;

public class FixGameSetup
{
    public static void Execute()
    {
        FixCamera();
        FixPrefabs();
    }

    static void FixCamera()
    {
        GameObject camObj = GameObject.Find("DesktopCamera");
        if (camObj == null)
        {
            Debug.LogError("DesktopCamera not found in scene.");
            return;
        }

        var fpsController = camObj.GetComponent<SimpleFPSController>();
        if (fpsController != null)
        {
            Object.DestroyImmediate(fpsController);
            Debug.Log("Removed SimpleFPSController from DesktopCamera.");
        }

        var pcController = camObj.GetComponent<PCControllers1>();
        if (pcController == null)
        {
            pcController = camObj.AddComponent<PCControllers1>();
            Debug.Log("Added PCControllers1 to DesktopCamera.");
        }
        
        // Ensure Camera component is there (it should be)
        if (camObj.GetComponent<Camera>() == null)
        {
            camObj.AddComponent<Camera>();
        }
    }

    static void FixPrefabs()
    {
        string rootPath = "Assets/Modules/Game_1_Pollen/Assets/Prefabs/Working table/Pickables";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { rootPath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);

            bool modified = false;

            // Add DesktopGrabbable
            if (prefabRoot.GetComponent<DesktopGrabbable>() == null)
            {
                prefabRoot.AddComponent<DesktopGrabbable>();
                Debug.Log($"Added DesktopGrabbable to {prefabRoot.name}");
                modified = true;
            }

            // Add Rigidbody
            if (prefabRoot.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = prefabRoot.AddComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                Debug.Log($"Added Rigidbody to {prefabRoot.name}");
                modified = true;
            }

            // Add Collider if missing
            if (prefabRoot.GetComponent<Collider>() == null)
            {
                // Try to add a BoxCollider that fits the mesh
                BoxCollider bc = prefabRoot.AddComponent<BoxCollider>();
                
                // Attempt to fit collider to mesh if a MeshFilter exists
                MeshFilter mf = prefabRoot.GetComponentInChildren<MeshFilter>();
                if (mf != null && mf.sharedMesh != null)
                {
                    // This is a simple approximation. 
                    // Ideally we'd transform bounds to local space of root, but for simple props this often works or defaults are okay.
                    // If the mesh is on a child, the collider on root might need adjustment.
                    // For now, let's just add it. Unity usually defaults to a 1x1x1 box.
                    // A better approach for complex hierarchies is to add the collider to the object with the mesh, 
                    // but DesktopGrabbable expects to be on the root with the RB.
                    // Let's leave the default BoxCollider, it's better than nothing.
                }
                
                Debug.Log($"Added BoxCollider to {prefabRoot.name}");
                modified = true;
            }

            if (modified)
            {
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            }

            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
        Debug.Log("Finished fixing prefabs.");
    }
}
