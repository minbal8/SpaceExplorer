using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceBody
{
    public GameObject spaceObject;

    private MeteorStats meteor;
    //private Planet planet;

    public Vector3 position;
    public Vector3 scale;
    public Quaternion quaternion;
    

    public SpaceBody()
    {
        quaternion = Quaternion.identity;
        position = Vector3.zero;
        scale = Vector3.one;
        meteor = new MeteorStats();
    }

    public void UpdateObjectComponent()
    {
        if(spaceObject.gameObject.tag == "Meteor")
        {
            
        }
    }

    public void setMeteor(float _health, float _maxHealth, int _resourceCount)
    {
        meteor.health = _health;
        meteor.maxHealth = _maxHealth;
        meteor.resourceCount = _resourceCount;
    }

    public MeteorStats getMeteor()
    {
        return meteor;
    }



}



