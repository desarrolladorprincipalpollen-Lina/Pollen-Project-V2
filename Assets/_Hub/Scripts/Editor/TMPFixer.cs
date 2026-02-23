using UnityEngine;
using UnityEditor;
using TMPro;

namespace PollenHub.Editor
{
    public class TMPFixer
    {
        [MenuItem("Pollen/Fix TMP CanvasRenderer Warnings (Auto-Fix)")]
        public static void FixWarnings()
        {
            int count = 0;
            // Find all TextMeshPro (3D Text) objects in the currently loaded scenes
            TextMeshPro[] tmps = Object.FindObjectsOfType<TextMeshPro>(true);
            
            foreach (var tmp in tmps)
            {
                CanvasRenderer cr = tmp.GetComponent<CanvasRenderer>();
                if (cr != null)
                {
                    // Destroy the obsolete CanvasRenderer component
                    GameObject.DestroyImmediate(cr, true);
                    EditorUtility.SetDirty(tmp.gameObject);
                    count++;
                }
            }
            
            if (count > 0)
            {
                Debug.Log($"[Pollen Fixer] Successfully removed CanvasRenderer from {count} TextMeshPro objects. Console warnings should be gone!");
            }
            else
            {
                Debug.Log($"[Pollen Fixer] No obsolete CanvasRenderers found in the current scene.");
            }
        }
    }
}
