using UnityEngine;
using UnityEditor;

namespace PollenHub.Editor
{
    public class MaterialFixer
    {
        [MenuItem("Pollen/Force Fix Pink Materials to URP (Auto-Fix)")]
        public static void FixMaterials()
        {
            // Search all materials in the Assets folder
            string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets" });
            Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
            
            if (urpLit == null) 
            {
                Debug.LogError("[Pollen Fixer] No se pudo encontrar el Shader de URP. ¿Estás seguro de que URP está instalado?");
                return;
            }

            int count = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                // Ignore TextMeshPro example materials as they use their own TMP shaders
                if (path.Contains("TextMesh") || path.Contains("TMP")) 
                    continue;

                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat != null && mat.shader != null)
                {
                    // If the shader is pure error (magenta) or an old legacy shader
                    if (mat.shader.name == "Hidden/InternalErrorShader" || 
                        mat.shader.name.StartsWith("Standard") ||
                        mat.shader.name.StartsWith("Legacy Shaders") ||
                        mat.shader.name.StartsWith("VR/") ||
                        (!mat.shader.name.Contains("Universal Render Pipeline") && 
                         !mat.shader.name.Contains("UI/")))
                    {
                        mat.shader = urpLit;
                        EditorUtility.SetDirty(mat);
                        count++;
                    }
                }
            }
            AssetDatabase.SaveAssets();
            
            if (count > 0)
            {
                Debug.Log($"[Pollen Fixer] ¡Éxito! Se forzó la actualización de {count} materiales viejos al nuevo formato URP Lit. Ya no deberían verse rosados.");
            }
            else
            {
                Debug.Log($"[Pollen Fixer] No se encontraron materiales rotos para arreglar (excluyendo textos).");
            }
        }
    }
}
