using UnityEngine;
using UnityEditor;

public class DisableMouseInteraction
{
    public static void Execute()
    {
        GameObject gm = GameObject.Find("GameManager");
        if (gm != null)
        {
            var mi = gm.GetComponent<MouseInteracion>();
            if (mi != null)
            {
                mi.enabled = false;
                Debug.Log("Disabled MouseInteraction on GameManager");
            }
        }
    }
}
