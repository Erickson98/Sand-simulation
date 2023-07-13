using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    public Transform edgeRoot;
    public LayerMask groundMask;

    List<Collector> collectors = new List<Collector>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < edgeRoot.transform.childCount; i++)
        {
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
        for (int i = 0; i < collectors.Count; i++)
        {
            var s = collectors[i];
            RaycastHit hit;
            if (Physics.Raycast(s.point.position, Vector3.up, out hit, 1, groundMask))
            {
                s.IsUnderground = true;
                if (hit.distance > s.amount)
                {
                    s.amount = hit.distance;
                }
                Debug.DrawLine(s.point.position, Vector3.up * hit.distance, Color.red);
            }
            else
            {
                s.IsUnderground = false;
                Debug.DrawRay(s.point.position, Vector3.up, Color.green);
            }
        }
    }
}
