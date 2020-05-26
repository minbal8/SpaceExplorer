using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSettings : MonoBehaviour
{
    public static ItemSettings instance = null;

    [Header("Ground Items")]
    public GameObject defaultGroundObject;      // Default object that is thrown on the ground
    public GameObject defaultGroundStackObject; // Default object that is thrown on the ground in stack
    public GameObject foodGroundStackObject;
    public GameObject resourceGroundStackObject;
    public GameObject weaponGroundStackObject;
    public GameObject equipmentGroundStackObject;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // Instance is set somewhere else
    }
}
