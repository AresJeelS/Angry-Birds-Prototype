using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    public int NumClouds = 40;
    public GameObject CloudPrefab;
    public Vector3 CloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 CloudPosMax = new Vector3(150, 100, 10);
    public float CloudScaleMin = 1;
    public float CloudScaleMax = 3;
    public float CloudSpeedMult = 0.5f;

    private GameObject[] _cloudInstances;

    private void Awake()
    {
        _cloudInstances = new GameObject[NumClouds];
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;
        for (int i = 0; i < NumClouds; i++)
        {
            cloud = Instantiate(CloudPrefab);
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(CloudPosMin.x, CloudPosMax.x);
            cPos.y = Random.Range(CloudPosMin.y, CloudPosMax.y);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(CloudScaleMin, CloudScaleMax, scaleU);

            cPos.y = Mathf.Lerp(CloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor.transform);
            _cloudInstances[i] = cloud;

        }

    }
    private void Update()
    {
        foreach (GameObject cloud in _cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            cPos.x -= scaleVal * Time.deltaTime * CloudSpeedMult;

            if (cPos.x <= CloudPosMin.x)
            {
                cPos.x = CloudPosMax.x;
            }

            cloud.transform.position = cPos;

        }
    }

}
