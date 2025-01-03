using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

class NewBakerScript : MonoBehaviour
{
    public Material Material;
    public Mesh Mesh;
}

public struct NewMaterial : IComponentData
{
    public bool hasChanged;
    public UnityObjectRef<Material> material;
    public UnityObjectRef<Mesh> mesh;
}

class NewBakerScriptBaker : Baker<NewBakerScript>
{
    public override void Bake(NewBakerScript authoring)
    {
        Entity e = GetEntity(authoring, TransformUsageFlags.Dynamic);

        AddComponent(e, new NewMaterial
        {
            material = authoring.Material,
            mesh = authoring.Mesh
        });
    }
}
