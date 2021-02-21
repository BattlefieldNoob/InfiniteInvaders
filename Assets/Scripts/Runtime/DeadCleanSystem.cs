using Unity.Entities;

[UpdateAfter(typeof(DestroyOnCollisionSystem))]
[UpdateAfter(typeof(DestroyAfterTimeSystem))]
public class DeadCleanSystem : SystemBase
{
    private EntityQuery query;
    private BeginSimulationEntityCommandBufferSystem _commandBufferSystem;


    protected override void OnCreate()
    {
        base.OnCreate();
        _commandBufferSystem = World
            .GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

        query = GetEntityQuery(ComponentType.ReadOnly<Dead>());
    }


    protected override void OnUpdate()
    {
        var commandBuffer = _commandBufferSystem.CreateCommandBuffer();

        Entities
            .ForEach((in Entity entity, in Dead dead) => { commandBuffer.DestroyEntity(entity); }).Schedule();

        _commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}