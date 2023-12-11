using UnityEngine;

[CreateAssetMenu(fileName = "SpawnTransformData", menuName = "Custom/SpawnTransformData", order = 1)]
public class SpawnTransformData : ScriptableObject
{
    [System.Serializable]
    public struct SpawnTransform
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    public SpawnTransform[] spawnTransforms; 
}