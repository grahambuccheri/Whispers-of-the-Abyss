using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public delegate void ScoreEvent(int score);

    public static event ScoreEvent OnCollection;

    public float radius = 5f;
    public int score = 42;
    void Start()
    {
        // create sphere collider which is the collection radius
        SphereCollider myCollider = gameObject.AddComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = radius;
    }

    void OnTriggerEnter(Collider other)
    {
        // if the submarine collided with our trigger, send out a collection event and destroy self.
        if (other.CompareTag("Submarine"))
        {
            OnCollection?.Invoke(score);
            Destroy(gameObject);
        }
    }
}
