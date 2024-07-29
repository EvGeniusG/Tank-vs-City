using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialRandomizer : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] MeshRenderer meshRenderer;

    void OnValidate(){
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Awake()
    {
        if(materials.Length > 0){
            meshRenderer.material = materials[Random.Range(0, materials.Length)];
        }
    }
}
