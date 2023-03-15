using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject CloudSphere;
    public int NumSphereMin = 6;
    public int NumSphereMax = 10;
    public Vector3 SphereOffSetScale = new Vector3(5, 2, 1);
    public Vector2 SphereScaleRangeX = new Vector3(4, 8);
    public Vector2 SphereScaleRangeY = new Vector3(3, 4);
    public Vector2 SphereScaleRangeZ = new Vector3(2, 4);
    public float ScaleYMin = 2f;

    private List<GameObject> _spheres;

    private void Start()
    {
        _spheres= new List<GameObject>();

        int num = Random.Range(NumSphereMin,NumSphereMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate(CloudSphere);
            _spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(transform);

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= SphereOffSetScale.x;
            offset.y *= SphereOffSetScale.y;
            offset.z *= SphereOffSetScale.z;
            spTrans.localPosition = offset;

            Vector3 scale = Vector3.one;
            scale.x = Random.Range (SphereScaleRangeX.x, SphereScaleRangeX.y);
            scale.y = Random.Range (SphereScaleRangeY.x, SphereScaleRangeY.y);
            scale.z = Random.Range (SphereScaleRangeZ.x, SphereScaleRangeZ.y);

            scale.y *= 1 - (Mathf.Abs(offset.x) / SphereOffSetScale.x);
            scale.y = Mathf.Max(scale.y, ScaleYMin);

            spTrans.localScale = scale;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    public void Restart()
    {
        foreach (GameObject sp in _spheres) 
        {
            Destroy(sp);
        }
        Start();
    }
}
