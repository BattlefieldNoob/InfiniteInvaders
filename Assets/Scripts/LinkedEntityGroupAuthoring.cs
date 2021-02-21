using Unity.Entities;
using UnityEngine;

public class LinkedEntityGroupAuthoring : MonoBehaviour,IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        conversionSystem.DeclareLinkedEntityGroup(gameObject);
    }
}
