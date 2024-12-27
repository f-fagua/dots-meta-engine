using Unity.Entities;
using Unity.Mathematics;

public struct Spawner : IComponentData
{
    public Entity Prefab;
    public float3 SpawnPosition;
    public float NextSpawnTime;
    public int Width;
    public int Height;
    public float Distance;
    public bool HasSpawned;
    public float PixelScale;
}
