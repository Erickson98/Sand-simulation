using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    public Transform edgeRoot;
    public LayerMask groundMask;

    List<Collector> collectors = new List<Collector>();
    Transform[] edgePoints;
    float[] sandAmount;
    // Start is called before the first frame update
    void Start()
    {
        edgePoints = new Transform[edgeRoot.transform.childCount];
        sandAmount = new float[edgePoints.Length];
        for (int i = 0; i < edgeRoot.transform.childCount; i++)
        {
            // edgePoints[i] = edgeRoot.GetChild(i);
            var c = new Collector();
            c.point = edgeRoot.GetChild(i);
            c.amount = 0f;
            c.IsUnderground = false;
            collectors.Add(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < edgePoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(edgePoints[i].position, Vector3.up, out hit, 1, groundMask))
            {
                Debug.Log(hit.distance);
                Debug.DrawLine(edgePoints[i].position, Vector3.up * hit.distance, Color.red);
                if (hit.distance > sandAmount[i])
                {
                    sandAmount[i] = hit.distance;
                }
            }
            else
            {

                Debug.DrawRay(edgePoints[i].position, Vector3.up, Color.green);
            }
        }
    }
}
