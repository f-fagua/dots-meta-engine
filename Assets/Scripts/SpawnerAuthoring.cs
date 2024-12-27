using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public int Width;
    public int Height;
    public float Distance;
    public float PixelScale;
}

class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new Spawner
        {
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            SpawnPosition = authoring.transform.position,
            NextSpawnTime = 0.0f,
            Width = authoring.Width,
            Height = authoring.Height,
            Distance = authoring.Distance,
            PixelScale = authoring.PixelScale
        });
    }
}