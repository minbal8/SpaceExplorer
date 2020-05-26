using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOld
{
    public enum ItemType
    {
        Railgun,
        Snickers
    }

    public ItemType itemType;
    public int Amount;

    public void Use()
    {
        switch (itemType)
        {
            case ItemType.Railgun:
                // TODO shoot
                LineRenderer lineRenderer = GameObject.Find("Laser").GetComponent<LineRenderer>();
                Transform laserPoint1 = GameObject.Find("LaserPoint1").GetComponent<Transform>();
                Transform laserPoint2 = GameObject.Find("LaserPoint2").GetComponent<Transform>();
                Vector3[] positions = new Vector3[] { laserPoint1.position, laserPoint2.position };
                lineRenderer.enabled = true;

                RaycastHit hit;
                Ray ray = new Ray(laserPoint1.position , -(laserPoint1.position - laserPoint2.position));
                Debug.DrawRay(laserPoint1.position, -(laserPoint1.position -  laserPoint2.position));


                if (Physics.Raycast(ray, out hit, 8f))
                {
                    if (hit.collider.gameObject.tag == "Meteor")
                    {
                        Meteor meteor = hit.rigidbody.gameObject.GetComponent<Meteor>();
                        meteor.TakeDamage(Random.Range(10f, 15f) * Time.deltaTime);
                    }

                    lineRenderer.SetPosition(1, new Vector3(0, -hit.distance));

                    Transform ps = GameObject.Instantiate(lineRenderer.GetComponent<Laser>().hitParticles, hit.point - ray.direction.normalized * 0.2f, Quaternion.LookRotation(ray.direction, hit.normal));

                    GameObject.Destroy(ps.gameObject, ps.GetComponentInChildren<ParticleSystem>().main.duration);
                }
                else
                    lineRenderer.SetPosition(1, new Vector3(0, -Vector3.Distance(laserPoint1.position, laserPoint2.position)));
                


                break;
            case ItemType.Snickers:

                // TODO eat
                break;
        }
    }

    public void Cancel()
    {
        switch (itemType)
        {
            case ItemType.Railgun:
                LineRenderer lineRenderer = GameObject.Find("Laser").GetComponent<LineRenderer>();
                lineRenderer.enabled = false;

                break;

            case ItemType.Snickers:

                // TODO stop eating (needed for eating with progress)
                break;
        }
    }
}
