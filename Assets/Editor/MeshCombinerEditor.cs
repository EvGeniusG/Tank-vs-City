using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MeshCombinerEditor : EditorWindow
{
    [MenuItem("Tools/Mesh Combiner")]
    public static void ShowWindow()
    {
        GetWindow<MeshCombinerEditor>("Mesh Combiner");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Combine Selected Meshes"))
        {
            CombineSelectedMeshes();
        }
    }

    private void CombineSelectedMeshes()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected for combining.");
            return;
        }

        Dictionary<Material, List<CombineInstance>> combinesByMaterial = new Dictionary<Material, List<CombineInstance>>();

        foreach (GameObject obj in selectedObjects)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            Renderer renderer = obj.GetComponent<Renderer>();

            if (meshFilter == null || renderer == null)
                continue;

            Material[] materials = renderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (!combinesByMaterial.ContainsKey(materials[i]))
                    combinesByMaterial.Add(materials[i], new List<CombineInstance>());

                CombineInstance combineInstance = new CombineInstance
                {
                    mesh = meshFilter.sharedMesh,
                    subMeshIndex = i,
                    transform = meshFilter.transform.localToWorldMatrix
                };
                combinesByMaterial[materials[i]].Add(combineInstance);
            }
        }

        GameObject combinedObject = new GameObject("CombinedMesh");
        combinedObject.transform.position = Vector3.zero;

        MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
        MeshRenderer combinedRenderer = combinedObject.AddComponent<MeshRenderer>();

        List<Material> combinedMaterials = new List<Material>();
        List<CombineInstance> finalCombines = new List<CombineInstance>();

        foreach (var kvp in combinesByMaterial)
        {
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(kvp.Value.ToArray(), true, true);
            finalCombines.Add(new CombineInstance { mesh = mesh, transform = Matrix4x4.identity });
            combinedMaterials.Add(kvp.Key);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(finalCombines.ToArray(), false, false);
        combinedMeshFilter.sharedMesh = combinedMesh;
        combinedRenderer.materials = combinedMaterials.ToArray();

        // Save the combined mesh as an asset
        SaveMeshAsset(combinedMesh, "CombinedMesh");

        foreach (GameObject obj in selectedObjects)
        {
            DestroyImmediate(obj);
        }
    }

    private void SaveMeshAsset(Mesh mesh, string name)
    {
        string path = "Assets/CombinedMeshes";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets", "CombinedMeshes");
        }

        string assetPath = Path.Combine(path, name + ".asset");
        AssetDatabase.CreateAsset(mesh, assetPath);
        AssetDatabase.SaveAssets();
    }
}
