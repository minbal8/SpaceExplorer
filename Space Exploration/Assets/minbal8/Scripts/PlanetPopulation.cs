using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPopulation : MonoBehaviour
{
    public float planetRadius;
    private GameObject spawnCharacter;
    public int RandomSeed = 0;
    public List<SpawnItem> spawnItems;


    void Start()
    {
        Random.InitState(RandomSeed);

        foreach (var item in spawnItems)
        {             
            for (int i = 0; i < item.ammount; i++)
            {
                Vector3 spawnPosition = UnityEngine.Random.onUnitSphere * (planetRadius * 0.5f) + transform.position;
                GameObject newCharacter = Instantiate(item.spawnObject, spawnPosition, Quaternion.identity) as GameObject;
                newCharacter.transform.LookAt(transform.position);
                newCharacter.transform.Rotate(-90, 0, 0);                
                

                if (item.randomRotation)
                {
                    var rotation = newCharacter.transform.rotation.eulerAngles;
                    rotation.x = UnityEngine.Random.Range(0, 360);
                    rotation.y = UnityEngine.Random.Range(0, 360);
                    rotation.z = UnityEngine.Random.Range(0, 360);



                    newCharacter.transform.rotation = Quaternion.Euler(rotation);
                }
                if (item.randomScale)
                {
                    var size = UnityEngine.Random.Range(0.5f, 3f);
                    var scale = newCharacter.transform.localScale * size;
                    newCharacter.transform.localScale = scale;
                }
                newCharacter.transform.SetParent(transform);
            }
        }
    }

    [System.Serializable]
    public class SpawnItem
    {
        public int ammount;
        public GameObject spawnObject;
        public bool randomScale = false;
        public bool randomRotation = false;
    }
}
