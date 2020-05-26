using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    public Rigidbody rb;
    public Transform resourcePrefab;
    public MeteorSettings.MeteorType meteorType;
    public MeteorStats meteorStats;

    private float initScale;


    void Start()
    {
        
    }

    void Update()
    {
        CheckHealth();
    }

    void FixedUpdate()
    {

    }

    public void SetInitScale(float s)
    {
        initScale = s;
    }


    public void CheckHealth()
    {
        //If meteor health is below or equal to 0, it will be destroyed and resources will be spawned
        if (meteorStats.health <= 0)
        {
            if (resourcePrefab != null)
            {
                for (int i = 0; i < meteorStats.resourceCount; i++)
                {
                    Vector3 spacing = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                    spacing *= gameObject.transform.localScale.x;
                    Transform t = Instantiate(resourcePrefab, transform.position + spacing, Quaternion.identity);
                    t.localScale = Vector3.one;
                }
            }
            Destroy(gameObject);

        }
    }

    public void TakeDamage(float damage)
    {
        meteorStats.health -= damage;
        float s = Mathf.Clamp((meteorStats.health / meteorStats.maxHealth) * initScale, 0.6f, initScale);
        transform.localScale = new Vector3(s, s, s);
    }
}

[System.Serializable]
public struct MeteorStats
{
    public float health;
    public float maxHealth;
    public int resourceCount;

}
