using UnityEngine;

public class HydrophoneStation : MonoBehaviour
{

    //TODO: Move engine RPM controll to an engine control script
    //TODO: Handle more precise interference
    //TODO: Implement randomized monster noise -> Grab from specific monster object
    public Transform hydrophoneTransform; // The position and rotation of the hydrophone
    public float maxDetectionDistance = 20000f; // Maximum distance for detection
    public float maxVolume = 0.5f; // Maximum volume for the sound
    public float throttleRate = 3f; // Throttle rate
    public float rotationAngle = 0f; //Rotation angle of the hydrophone
    public float volume = 0f;
    public float engineSpeed = 0f;

    public AudioSource monsterSoundSource; // AudioSource for the monster sounds
    public AudioClip[] monsterSounds; // Array of monster sound clips
    public AudioClip monsterSound; // Array of monster sound clips

    public AudioSource interferenceSoundSource; // AudioSource for interference sound
    public AudioClip interferenceSound; // Interference sound clip

    private Rigidbody submarineRigidbody; // Reference to the submarine's Rigidbody component

    void Start()
    {
        submarineRigidbody = this.GetComponent<Rigidbody>();
        hydrophoneTransform = GameObject.Find("Hydrophone").transform;
        interferenceSoundSource = GameObject.Find("EngineAudio").GetComponent<AudioSource>();
        monsterSoundSource = GameObject.Find("MonsterAudio").GetComponent<AudioSource>();

        // Play interference sound looped
        if (interferenceSoundSource != null && interferenceSound != null)
        {
            interferenceSoundSource.clip = interferenceSound;
            interferenceSoundSource.loop = true;
            interferenceSoundSource.volume = 0f;
            interferenceSoundSource.Play();
            //interferenceSoundSource.PlayOneShot(interferenceSound);
            Debug.Log("interferenceSoundSource is playing!");
        }
        else
        {
            Debug.Log("interferenceSoundSource Error");
        }
    }

    void Update()
    {

        if (hydrophoneTransform != null)
        {
            // Rotate the hydrophone based on input
            //float rotationInput = Input.GetAxis("HydrophoneRotation");
            

            GameObject[] monsterNodes = GameObject.FindGameObjectsWithTag("MonsterNode");
            float loudestVolume = 0f;

            foreach (GameObject node in monsterNodes)
            {
                Collider[] colliders = node.GetComponentsInChildren<Collider>();
                float closestDistance = Mathf.Infinity;
                Vector3 closestPoint = Vector3.zero;
                foreach (Collider collider in colliders)
                {
                    // Calculate the closest point on the collider to the hydrophone
                    Vector3 closestPointOnCollider = collider.ClosestPointOnBounds(hydrophoneTransform.position);

                    // Calculate the distance to this closest point
                    float distanceToCollider = Vector3.Distance(hydrophoneTransform.position, closestPointOnCollider);

                    // Update the closest distance and point if this is closer
                    if (distanceToCollider < closestDistance)
                    {
                        closestDistance = distanceToCollider;
                        closestPoint = closestPointOnCollider;
                    }
                }
                // Calculate direction to the monsterNode
                Vector3 directionToClosestPoint = closestPoint - hydrophoneTransform.position;

                // Calculate angle between the forward direction of the hydrophone and the direction to the closest point
                float angleToClosestPoint = Vector3.Angle(hydrophoneTransform.forward, directionToClosestPoint);

                // Check if the closest point is within detection range and within the listening angle
                if (closestDistance <= maxDetectionDistance  && angleToClosestPoint <= 60f)
                {
                    Debug.LogWarning("Hydrophone sees monster at " + closestDistance + " distance and angle :" + angleToClosestPoint);
                    // Calculate volume based on distance
                    volume = Mathf.Clamp01(1f - (angleToClosestPoint / 30)*(closestDistance / maxDetectionDistance)) * maxVolume;

                    // Keep track of the loudest volume
                    if (volume >= loudestVolume)
                    {
                        loudestVolume = volume;
                    }

                    // Play the monster's sound if it has an assigned AudioClip
                    if (monsterSoundSource != null)
                    {
                        if (!monsterSoundSource.isPlaying)
                        {
                            // Choose a random monster sound clip from the array
                            //int randomIndex = Random.Range(0, monsterSounds.Length);
                            //monsterSoundSource.clip = monsterSounds[randomIndex];
                            monsterSoundSource.clip = monsterSound;
                            monsterSoundSource.Play();
                            //monsterSoundSource.PlayOneShot(monsterSound);
                        }
                    }
                }
                //Monster is out of range
                else
                {
                    volume = 0;
                   
               
                    if (monsterSoundSource != null)
                    {
                        if (!monsterSoundSource.isPlaying)
                        {
                            // Choose a random monster sound clip from the array
                            //int randomIndex = Random.Range(0, monsterSounds.Length);
                            //monsterSoundSource.clip = monsterSounds[randomIndex];
                            //monsterSoundSource.Play();
                            monsterSoundSource.volume = 0;
                            monsterSoundSource.Stop();
                        }
                    }
                }
            }

            // Set the volume of the monster sound
            monsterSoundSource.volume = loudestVolume;

            // Set the volume of the interference sound based on engine RPM
            if (interferenceSoundSource != null && submarineRigidbody != null)
            {
                interferenceSoundSource.volume = engineSpeed * maxVolume;
                interferenceSoundSource.pitch = engineSpeed/5 +1;

            }
            else
            {
               ///   Debug.LogError("Submarine Missing or interferenceSoundSource not defined");
            }
        }
    }
}