using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

partial struct NewISystemScript : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NewMaterial>();
    }

    public void OnUpdate(ref SystemState state)
    {
        //SystemAPI.GetSingletonBuffer<IBufferElementData>();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var cmb = new EntityCommandBuffer(Allocator.Temp);
        
        
            foreach (var (materialRef, entity) in SystemAPI.Query<NewMaterial>().WithEntityAccess())
            {
            
                cmb.SetSharedComponentManaged(entity, new RenderMeshArray(
                    new Material[] {materialRef.material.Value}, 
                    new Mesh[]{materialRef.mesh.Value}, 
                    null)
                );
            }

            cmb.Playback(state.EntityManager);
            cmb.Dispose();
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
