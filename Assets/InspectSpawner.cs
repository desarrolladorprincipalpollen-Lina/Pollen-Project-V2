using UnityEngine;
using PollenModule;
using System.Collections.Generic;

public class InspectSpawner
{
    public static void Execute()
    {
        GameObject workTable = GameObject.Find("Work table");
        if (workTable == null)
        {
            Debug.LogError("Work table not found");
            return;
        }

        RandomSpawner spawner = workTable.GetComponent<RandomSpawner>();
        if (spawner == null)
        {
            Debug.LogError("RandomSpawner not found on Work table");
            return;
        }

        // Use reflection to access private/serialized fields if necessary, 
        // but here we can try to access them if they are public or via SerializedObject if we were in Editor script.
        // Since this is runtime/editor script execution, we can access private fields via reflection.
        
        var type = typeof(RandomSpawner);
        var field = type.GetField("pickablesToSpawn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            List<GameObject> pickables = (List<GameObject>)field.GetValue(spawner);
            Debug.Log("Pickables found: " + pickables.Count);
            foreach (var p in pickables)
            {
                if (p != null)
                {
                    Debug.Log("Pickable: " + p.name + " | Path: " + UnityEditor.AssetDatabase.GetAssetPath(p));
                }
            }
        }
        else
        {
            Debug.LogError("Could not find pickablesToSpawn field");
        }
    }
}
