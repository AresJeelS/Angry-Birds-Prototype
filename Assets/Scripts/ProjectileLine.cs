using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;
    [Header("Set in Inspector")]
    public float MinDist = 0.1f;

    private LineRenderer _line;
    private GameObject _poi;
    private List<Vector3> _points;

    private void Awake()
    {
        S = this;
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _points = new List<Vector3>();
    }
    public GameObject POI
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                _line.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear()
    {
        _poi = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if (_points.Count > 0 && (pt - LastPoint).magnitude < MinDist)
        {
            return;
        }
        if (_points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            _points.Add(pt + launchPosDiff);
            _points.Add(pt);
            _line.positionCount = 2;
            _line.SetPosition(0, _points[0]);
            _line.SetPosition(1, _points[1]);
            _line.enabled = true;
        }
        else
        {
            _points.Add(pt);
            _line.positionCount = _points.Count;
            _line.SetPosition(_points.Count - 1, LastPoint);
            _line.enabled = true;
        }
    }

    public Vector3 LastPoint
    {
        get
        {
            if (_points == null)
            {
                return (Vector3.zero);
            }
            return (_points[_points.Count - 1]);
        }
    }
    private void FixedUpdate()
    {
        if (POI == null)
        {
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    POI = FollowCam.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        AddPoint();

        if (FollowCam.POI == null)
        {
            POI = null;
        }
    }
}

