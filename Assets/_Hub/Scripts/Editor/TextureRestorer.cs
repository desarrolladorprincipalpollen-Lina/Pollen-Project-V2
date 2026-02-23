using UnityEngine;
using UnityEditor;

namespace PollenHub.Editor
{
    public class TextureRestorer
    {
        [MenuItem("Pollen/Restore Textures and Skybox (Auto-Fix)")]
        public static void FixTextures()
        {
            string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets" });
            int recoveredTextures = 0;
            bool skyboxFixed = false;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                
                if (mat == null) continue;

                // Fix Skybox
                if (mat.name.ToLower().Contains("skybox") || path.ToLower().Contains("skybox"))
                {
                    Shader panoShader = Shader.Find("Skybox/Panoramic");
                    if (panoShader != null && mat.shader != panoShader)
                    {
                        // Save the old texture before changing shader
                        Texture tex = null;
                        if (mat.HasProperty("_MainTex")) tex = mat.GetTexture("_MainTex");
                        else if (mat.HasProperty("_BaseMap")) tex = mat.GetTexture("_BaseMap");
                        
                        mat.shader = panoShader;
                        
                        if (tex != null)
                        {
                            mat.SetTexture("_MainTex", tex);
                        }
                        EditorUtility.SetDirty(mat);
                        skyboxFixed = true;
                    }
                }
                // Fix standard URP Lit textures
                else if (mat.shader != null && mat.shader.name.Contains("Universal Render Pipeline"))
                {
                    // URP uses _BaseMap. Standard used _MainTex.
                    // If the URP material has no BaseMap but has MainTex hidden in properties, we copy it over.
                    // Unity sometimes keeps the serialized "_MainTex" property even if the new shader doesn't use it cleanly.
                    if (mat.HasProperty("_BaseMap") && mat.GetTexture("_BaseMap") == null)
                    {
                        // Some legacy textures might still be accessible via the old name
                        if (mat.HasProperty("_MainTex"))
                        {
                            Texture oldTex = mat.GetTexture("_MainTex");
                            if (oldTex != null)
                            {
                                mat.SetTexture("_BaseMap", oldTex);
                                EditorUtility.SetDirty(mat);
                                recoveredTextures++;
                            }
                        }
                    }
                }
            }
            AssetDatabase.SaveAssets();
            Debug.Log($"[Pollen Fixer] ¡Listo! Restauradas {recoveredTextures} texturas de paredes/objetos y {(skyboxFixed ? "1 Skybox arreglado" : "cielo revisado")}.");
        }
    }
}
