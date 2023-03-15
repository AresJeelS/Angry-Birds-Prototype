using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot _s;
    [Header("Set im Inspector")]
    public GameObject PrefabProjectile;
    public float VelocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject LaunchPoint;
    public Vector3 LaunchPos;
    public GameObject Projectile;
    public bool AimingMode;

    private Rigidbody _projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (_s == null)
            {
                return Vector3.zero;
            }
            return _s.LaunchPos;
        }
    }
    private void Awake()
    {
        _s = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        LaunchPoint = launchPointTrans.gameObject;
        LaunchPoint.SetActive(false);
        LaunchPos = launchPointTrans.position;

    }
    private void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        LaunchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        LaunchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        AimingMode = true;
        Projectile = Instantiate(PrefabProjectile);
        Projectile.transform.position = LaunchPos;
        Projectile.GetComponent<Rigidbody>().isKinematic = true;
        _projectileRigidbody = Projectile.GetComponent<Rigidbody>();
        _projectileRigidbody.isKinematic = true;
    }
    private void Update()
    {
        if (!AimingMode)
        {
            return;
        }
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - LaunchPos;
        float maxMagnitude = GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = LaunchPos + mouseDelta;
        Projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            AimingMode = false;
            _projectileRigidbody.isKinematic = false;
            _projectileRigidbody.velocity = -mouseDelta * VelocityMult;
            FollowCam.POI = Projectile;
            Projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.POI = Projectile;
        }
    }
}
