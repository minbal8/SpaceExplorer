using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSettings : MonoBehaviour
{
    public static MeteorSettings instance = null;
    public List<GameObject> meteorPrefabs;

    public int minMeteorCount = 0;
    public int maxMeteorCount = 3;

    // Lowest probability first, next one is equal previous minus current
    Dictionary<MeteorType, float> probabilities = new Dictionary<MeteorType, float>()
    {
        { MeteorType.Gold, 0.01f },    
        { MeteorType.Coal, 0.15f },
        { MeteorType.Iron, 0.25f },
        { MeteorType.Rock, 1f },
    };


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // Instance exists somewhere else
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject PickMeteorByProbability()
    {
        float ran = Random.Range(0f, 1f);

        foreach(var probability in probabilities)
        {
            if(ran <= probability.Value)
            {
                return meteorPrefabs[(int)(probability.Key)];
            }
        }

        return meteorPrefabs[(int)(MeteorType.Rock)];
    }

    public enum MeteorType {
        Rock,
        Iron,
        Coal,
        Gold,
    }
}
