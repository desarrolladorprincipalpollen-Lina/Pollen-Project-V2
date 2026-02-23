using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

namespace PollenHub.Editor
{
    public class XRFixer
    {
        [MenuItem("Pollen/Fix Deprecated XR Rig Components (Auto-Fix)")]
        public static void FixWarnings()
        {
            int count = 0;
            // Find all components matching XRRig by string to avoid strict compiler errors
            var allMonoBehaviours = Object.FindObjectsOfType<MonoBehaviour>(true);
            
            foreach (var mb in allMonoBehaviours)
            {
                if (mb == null) continue; // Skip missing scripts

                if (mb.GetType().Name == "XRRig")
                {
                    GameObject rigGo = mb.gameObject;
                    
                    // Destroy the old Rig FIRST to prevent component conflicts
                    GameObject.DestroyImmediate(mb, true);
                    
                    // Now safely add the new Origin
                    var origin = rigGo.AddComponent<Unity.XR.CoreUtils.XROrigin>();
                    
                    if (origin != null) 
                    {
                        origin.RequestedTrackingOriginMode = Unity.XR.CoreUtils.XROrigin.TrackingOriginMode.Floor;
                    }
                    else 
                    {
                        Debug.LogWarning($"[Pollen Fixer] Failed to add XROrigin to {rigGo.name}. You may need to add it manually.");
                    }

                    EditorUtility.SetDirty(rigGo);
                    count++;
                }
            }
            
            if (count > 0)
            {
                Debug.Log($"[Pollen Fixer] Successfully upgraded {count} XRRig(s) to XROrigin. Please verify your Camera references on the new XROrigin component in the Inspector.");
            }
            else
            {
                Debug.Log($"[Pollen Fixer] No obsolete XRRig components found in the current scene.");
            }
        }
    }
}
