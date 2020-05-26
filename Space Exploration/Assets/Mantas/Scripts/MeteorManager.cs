using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{

    public float minDistance = 100.0f;
    public float maxDistance = 120.0f;
    public float minForce = 1f;
    public float maxForce = 2f;
    public int maxMeteors = 100;
    public float spawnDelay = 0.5f; // seconds
    private float initialSpawnDelay;
    private List<Meteor> meteors;
    public Transform meteorTransform;
    Dictionary<string, float> probabilities = new Dictionary<string, float>()
    {
        { "Meteor", 1f },
    };

    void Start()
    {
        meteors = new List<Meteor>();
        initialSpawnDelay = spawnDelay;
    }


    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SpawnMeteor();
        }
        if (spawnDelay <= 0f)
        {
            if (meteors.Count < maxMeteors && Random.Range(0f, 1f) <= probabilities["Meteor"])
            {
                SpawnMeteor();
            }

            spawnDelay = initialSpawnDelay;
        }

        spawnDelay -= Time.fixedDeltaTime;

    }

    public void SpawnMeteor()
    {
        float distance = Random.Range(minDistance, maxDistance);
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;

        Meteor meteor = Instantiate(meteorTransform, spawnPosition, Quaternion.identity, transform).GetComponent<Meteor>();
        
        //meteor.AddResource("Iron");
        //meteor.AddForce(Random.Range(minForce, maxForce));
        //meteor.AddTorque(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), Random.Range(10f, 50f) * meteor.rb.mass);
        //meteors.Add(meteor);
        Debug.Log("Spawning meteor");
    }

    public enum ResourceType
    {
        Iron,
        Coal,
        Gold
    }

}