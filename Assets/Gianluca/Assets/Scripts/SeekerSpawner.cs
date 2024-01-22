using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerSpawner : MonoBehaviour
{
    public GameObject seekerPrefab; // Assign your seeker prefab in the inspector
    public Transform target; // Assign the common target in the inspector
    public int numberOfSeekers; // Set the number of seekers you want to spawn
    public float spawnRate = 0.1f; // Time between spawns

    private void Start()
    {
        StartCoroutine(SpawnSeekers());
    }

    private IEnumerator SpawnSeekers()
    {
        for (int i = 0; i < numberOfSeekers; i++)
        {
            // Instantiate a new seeker instance
            GameObject newSeeker = Instantiate(seekerPrefab, GetRandomSpawnPosition(), Quaternion.identity);

            // Assign the target to the new seeker
            newSeeker.GetComponent<UnitGP>().target = target;

            // Optionally, you can immediately start the pathfinding process
            newSeeker.GetComponent<UnitGP>().StartCoroutine("FollowPath");

            // Wait for a bit before spawning the next seeker
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 potentialPosition;
        bool positionFound = false;
        int maxAttempts = 100; // Prevent an infinite loop
        int attempts = 0;

        do
        {
            // The plane extends from -5 to +5 on the X and Z axes
            float xPosition = Random.Range(-5f, 5f);
            float zPosition = Random.Range(-5f, 5f);
            potentialPosition = new Vector3(xPosition, 0, zPosition); // Y is 0 because it's a flat plane

            // Check if this position collides with any unwalkable objects
            Collider[] colliders = Physics.OverlapBox(potentialPosition, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, LayerMask.GetMask("Unwalkable"));

            // If no colliders are found, it means this is a walkable position
            if (colliders.Length == 0)
            {
                positionFound = true;
            }

            attempts++;
        } while (!positionFound && attempts < maxAttempts);

        if (!positionFound)
        {
            Debug.LogError("Failed to find a walkable spawn position after " + maxAttempts + " attempts.");
            return Vector3.zero; // You may want to handle this more gracefully
        }

        return potentialPosition;
    }

}
