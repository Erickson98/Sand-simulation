// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Shovel : MonoBehaviour
// {
//     public Transform edgeRoot;
//     public LayerMask groundMask;
//     // public MeshFilter sandMesh;
//     List<Collector> collectors = new List<Collector>();
//     // Start is called before the first frame update
//     void Start()
//     {
//         for (int i = 0; i < edgeRoot.transform.childCount; i++)
//         {
//             var child = edgeRoot.GetChild(i);
//             if (child.gameObject.activeSelf == false)
//             {
//                 continue;
//             }
//             var c = new Collector();
//             c.point = child;
//             c.amount = Vector3.zero;
//             c.IsUnderground = false;
//             collectors.Add(c);
//         }
//         var mesh = new Mesh();
//         // sandMesh.mesh = mesh;

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         for (int i = 0; i < collectors.Count; i++)
//         {
//             var s = collectors[i];
//             RaycastHit hit;
//             if (Physics.Raycast(s.point.position, Vector3.up, out hit, 1, groundMask))
//             {
//                 s.IsUnderground = true;
//                 if (hit.distance > s.amount.y)
//                 {
//                     s.amount.y = hit.distance;
//                 }
//                 Debug.DrawLine(s.point.position, Vector3.up * hit.distance, Color.red);
//             }
//             else
//             {
//                 s.IsUnderground = false;
//                 Debug.DrawRay(s.point.position, Vector3.up, Color.green);
//             }
//             collectors[i] = s;
//         }
//         //build mesh from edges
//         var verts = new List<Vector3>();
//         var tris = new List<int>();

//         int triCount = 0;
//         // this loop is about build the mesh.
//         for (int i = 0; i < collectors.Count; i++)
//         {
//             var s = collectors[i];
//             if (s.IsUnderground == true)
//             {

//                 verts.Add(s.point.position);

//                 verts.Add(s.point.position + s.amount);

//                 verts.Add(s.point.position + Vector3.forward);

//                 verts.Add(s.point.position + s.amount + Vector3.forward);

//                 tris.Add(triCount++);
//                 // tris.Add(tris.Count - 1);

//             }

//         }

//         Debug.Log(verts.Count + " " + tris.Count);
//         // var mesh = new Mesh();
//         // var mesh = sandMesh.mesh;
//         // mesh.Clear();
//         // mesh.vertices = verts.ToArray();
//         // // mesh.triangles = tris.ToArray();
//         // mesh.SetIndices(tris.ToArray(), MeshTopology.Quads, 0);

//         // mesh.RecalculateBounds();
//         // mesh.RecalculateNormals();
//         // mesh.RecalculateTangents();


//         // sandMesh.sharedMesh = mesh;
//     }
// }



using UnityEngine;
using System.Collections.Generic;

public class Shovel : MonoBehaviour
{
    public Transform edgRoot;
    // Transform[] edgePoints;

    // float[] sandAmount;
    public LayerMask groundMask;
    public MeshFilter sandMesh;
    List<Collector> collectors = new List<Collector>();
    void Start()
    {
        // edgePoints = new Transform[edgRoot.transform.childCount];
        // sandAmount = new float[edgePoints.Length];

        for (int i = 0; i < edgRoot.transform.childCount; i++)
        {
            // edgePoints[i] = edgRoot.GetChild(i);
            var c = new Collector();
            c.point = edgRoot.GetChild(i);
            c.amount = 0f;
            c.IsUnderground = false;
            collectors.Add(c);
        }
    }

    void Update()
    {


        for (int i = 0; i < collectors.Count; i++)
        {
            var s = collectors[i];
            RaycastHit hit;
            if (Physics.Raycast(s.point.position, Vector3.up, out hit, 1, groundMask))
            {
                s.IsUnderground = true;

                Debug.DrawRay(s.point.position, Vector3.up * hit.distance, Color.red);
                if (hit.distance > s.amount)
                {
                    s.amount = hit.distance;
                }

            }
            else
            {
                s.IsUnderground = false;
                Debug.DrawRay(collectors[i].point.position, Vector3.up, Color.green);
            }
        }

        var verts = new List<Vector3>();
        var tris = new List<int>();

        for (int i = 0; i < collectors.Count; i++)
        {
            var s = collectors[i];

            if (s.IsUnderground == true)
            {
                verts.Add(s.point.position);
                verts.Add(s.point.position + s.amount);
            }
        }

        var mesh = new Mesh();
        mesh.vertices = verts.ToArray();

        // mesh.triangles = tris.ToArray();
        mesh.SetIndices(tris.ToArray(), MeshTopology.Quads, 0);


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();


        sandMesh.sharedMesh = mesh;
    }
}