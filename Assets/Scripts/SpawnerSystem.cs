using System.Diagnostics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Queries for all spawner components. uses RefRW because this system wants
        // to read from and write to the component. If the system only needed read-only
        // access, it would use refRO instead

        foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            if (!spawner.ValueRW.HasSpawned)
            {
                ProcessSpawner(ref state, spawner);
            }
        }
    }

    private void ProcessSpawner(ref SystemState state, RefRW<Spawner> spawner)
    {
        for (int row = 0; row < spawner.ValueRO.Height; row++)
        {
            for (int col = 0; col < spawner.ValueRO.Width; col++)
            {
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
        
                // LocalPosition.FromPosition returns a Transform initialized with the given position.
                var wOffset = (spawner.ValueRO.Width * spawner.ValueRO.Distance) / 2f;
                var hOffset = (spawner.ValueRO.Height * spawner.ValueRO.Distance) / 2f;

                var x = col * spawner.ValueRW.Distance - wOffset;
                var y = row * spawner.ValueRW.Distance - hOffset;
                
                var position = spawner.ValueRO.SpawnPosition + new float3(x, y, 0);
                
                state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPositionRotationScale(position, quaternion.identity, spawner.ValueRO.PixelScale));
            }            
        }

        spawner.ValueRW.HasSpawned = true;
    }
}
