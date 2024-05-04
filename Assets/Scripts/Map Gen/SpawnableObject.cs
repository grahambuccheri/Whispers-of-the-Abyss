using UnityEngine;

namespace Map_Gen
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SpawnableObject")]
    public class SpawnableObject : ScriptableObject
    {
        public GameObject spawnable;
        
        public Vector3 spawnOffset = Vector3.zero;
        
        public Vector3 baseRotation = Vector3.zero;
        
        public int xzVariance = 0; // defines the variability in orientation in the X and Z axes of rotation. using in calling function to determine orientation.

        public void Spawn(Vector3 location, Vector3 orientation, GameObject parent)
        {
            if (!spawnable) { return; }

            var temp = Instantiate(spawnable, location + spawnOffset, Quaternion.Euler(orientation + baseRotation));
            temp.transform.parent = parent.transform;
        }
    }
}
