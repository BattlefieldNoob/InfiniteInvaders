using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

public class DestroyOnCollisionSystem : SystemBase
{
    
    private EndSimulationEntityCommandBufferSystem _commandBufferSystem;
    
    
    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        _commandBufferSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        _physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    public struct DestroyOnCollision : ICollisionEventsJob
    {
        public EntityCommandBuffer _commandBuffer;

        public void Execute(CollisionEvent collisionEvent)
        {
            _commandBuffer.DestroyEntity(collisionEvent.EntityA);
            _commandBuffer.DestroyEntity(collisionEvent.EntityB);
        }
    }

    private BuildPhysicsWorld _physicsWorld;
    private StepPhysicsWorld _stepPhysicsWorld;
    
    
    
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.


        var commandBuffer = _commandBufferSystem.CreateCommandBuffer();

        var a = new DestroyOnCollision()
        {
            _commandBuffer = commandBuffer
        };


        var handle=a.Schedule(_stepPhysicsWorld.Simulation, ref _physicsWorld.PhysicsWorld, Dependency);
        
        _commandBufferSystem.AddJobHandleForProducer(handle);
        
    }
}
