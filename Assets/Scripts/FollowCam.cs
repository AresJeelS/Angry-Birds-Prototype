using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in Inspector")]
    public float Easing = 0.05f;
    public Vector2 MinXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float CamZ;

    private void Awake()
    {
        CamZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
      //  if (POI == null)
      //  {
            // return;
      //  }

      //  Vector3 destination = POI.transform.position;

        Vector3 destination;
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;

            if (POI.CompareTag("Projectile"))
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                }
            }
        }
        destination = Vector3.Lerp(transform.position, destination, Easing);
        destination.z = CamZ;
        transform.position = destination;
        destination.x = Mathf.Max(MinXY.x, destination.x);
        destination.y = Mathf.Max(MinXY.x, destination.y);
        Camera.main.orthographicSize = destination.y + 10;
    }

}
