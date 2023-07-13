using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    public Transform edgeRoot;
    public LayerMask groundMask;
    public MeshFilter sandMesh;
    List<Collector> collectors = new List<Collector>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < edgeRoot.transform.childCount; i++)
        {
            var c = new Collector();
            c.point = edgeRoot.GetChild(i);
            c.amount = Vector3.zero;
            c.IsUnderground = false;
            collectors.Add(c);
        }
        var mesh = new Mesh();
        sandMesh.mesh = mesh;

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
                if (hit.distance > s.amount.y)
                {
                    s.amount.y = hit.distance;
                }
                Debug.DrawLine(s.point.position, Vector3.up * hit.distance, Color.red);
            }
            else
            {
                s.IsUnderground = false;
                Debug.DrawRay(s.point.position, Vector3.up, Color.green);
            }
            collectors[i] = s;
        }

        // var verts = new List<Vector3>();
        // var tris = new List<int>();

        // // this loop is about build the mesh.
        // for (int i = 0; i < collectors.Count; i++)
        // {
        //     var s = collectors[i];
        //     if (s.IsUnderground)
        //     {

        //         verts.Add(s.point.position);
        //         tris.Add(tris.Count - 1);
        //         verts.Add(s.point.position + s.amount);
        //         tris.Add(tris.Count - 1);

        //     }

        // }
        // // var mesh = new Mesh();
        // var mesh = sandMesh.mesh;

        // mesh.vertices = verts.ToArray();
        // // mesh.triangles = tris.ToArray();
        // mesh.SetIndices(tris.ToArray(), MeshTopology.Quads, 0);

        // mesh.RecalculateBounds();
        // mesh.RecalculateNormals();
        // mesh.RecalculateTangents();


        sandMesh.sharedMesh = mesh;
    }
}
