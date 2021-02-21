using Unity.Entities;

public class DestroyAfterTimeSystem : SystemBase
{
    private EndFixedStepSimulationEntityCommandBufferSystem _commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        _commandBufferSystem = World
            .GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
    }

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

        var actualTime = Time.ElapsedTime;

        Entities.WithNone<Dead>().ForEach((in Entity entity, in DestroyAfterTime destroyAfterTime) =>
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as 'in', which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;

            if (actualTime - destroyAfterTime.BirthTime < destroyAfterTime.CountDown) return;

            commandBuffer.AddComponent<Dead>(entity);
        }).Schedule();

        _commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}