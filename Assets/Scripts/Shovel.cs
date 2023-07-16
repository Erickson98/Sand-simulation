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
using System.Collections;
using System.Collections.Generic;

public class Shovel : MonoBehaviour
{
    public Transform edgRoot;
    // Transform[] edgePoints;

    // float[] sandAmount;
    public LayerMask groundMask;
    public MeshFilter sandMesh;
    List<Collector> collectors = new List<Collector>();
    Vector3 sandMeshOffset;
    Collider shovelCollider;
    void Start()
    {
        // edgePoints = new Transform[edgRoot.transform.childCount];
        // sandAmount = new float[edgePoints.Length];

        for (int i = 0; i < edgRoot.transform.childCount; i++)
        {
            // edgePoints[i] = edgRoot.GetChild(i);
            var child = edgRoot.GetChild(i);
            Debug.Log(child.position);
            if (child.gameObject.activeSelf == false) continue;
            var c = new Collector();
            c.point = child;
            c.amount = Vector3.zero;
            c.IsUnderground = false;
            collectors.Add(c);
        }
        shovelCollider = GetComponent<Collider>();
        var mesh = new Mesh();
        mesh.MarkDynamic();
        sandMeshOffset = sandMesh.transform.localPosition;
        sandMesh.mesh = mesh;
    }
    void FixedUpdate()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var go = Instantiate(sandMesh.gameObject, sandMesh.transform.position, sandMesh.transform.rotation);

            var mc = go.AddComponent<MeshCollider>();
            mc.convex = true;
            Physics.IgnoreCollision(mc, shovelCollider);
            var rg = go.AddComponent<Rigidbody>();
            rg.AddForce(0, 7, 0, ForceMode.Impulse);
            for (int i = 0; i < collectors.Count; i++)
            {
                var s = collectors[i];
                s.IsUnderground = false;
                s.amount = Vector3.zero;
                collectors[i] = s;
            }
        }

        UpdateSandMesh();
    }

    void UpdateSandMesh()
    {
        // blucle para comprobar si los puntos estan bajo tierra
        for (int i = 0; i < collectors.Count; i++)
        {
            var s = collectors[i];
            RaycastHit hit;
            if (Physics.Raycast(s.point.position, Vector3.up, out hit, 1, groundMask))
            {
                s.IsUnderground = true;

                // Debug.DrawRay(s.point.position, Vector3.up * hit.distance, Color.red);
                if (hit.distance > s.amount.y)
                {
                    s.amount.y = hit.distance;
                }

            }
            else
            {
                // s.amount.y = 0;
                // s.IsUnderground = false;
                // Debug.DrawRay(collectors[i].point.position, Vector3.up, Color.green);
            }
            collectors[i] = s;
        }

        var verts = new List<Vector3>();
        var tris = new List<int>();
        var uv = new List<Vector2>();

        var topVert = new List<Vector3>();
        var bottomVerts = new List<Vector3>();
        int triCount = 0;
        var center = Vector3.zero;
        // Este bucle asigna los vertices con su profundidad para poderlo utilizar en la mesh
        for (int i = 0; i < collectors.Count; i++)
        {
            var s = collectors[i];

            if (s.IsUnderground == true)
            {
                center += collectors[i].point.position;
                var pos2 = transform.InverseTransformPoint(s.point.position) - sandMeshOffset;
                verts.Add(pos2);
                bottomVerts.Add(pos2);
                tris.Add(triCount++);
                uv.Add(pos2);

                var pos = transform.InverseTransformPoint(s.point.position + s.amount) - sandMeshOffset;
                verts.Add(pos);

                topVert.Add(pos);
                tris.Add(triCount++);
                uv.Add(pos);

                // tris.Add(tris.Count - 1);
                bool gotPair = false;
                for (i++; i < collectors.Count; ++i)
                {
                    var s2 = collectors[i];
                    if (s2.IsUnderground == true)
                    {
                        // tris.Add(tris.Count - 1);
                        var pos3 = transform.InverseTransformPoint(s2.point.position + s2.amount) - sandMeshOffset;
                        verts.Add(pos3);
                        topVert.Add(pos3);

                        tris.Add(triCount++);
                        uv.Add(pos3);

                        var pos4 = transform.InverseTransformPoint(s2.point.position) - sandMeshOffset;
                        verts.Add(pos4);
                        bottomVerts.Add(pos4);

                        tris.Add(triCount++);
                        uv.Add(pos4);


                        Debug.DrawRay(s.point.position, s.point.position + s.amount, i % 2 == 0 ? Color.red : Color.green);
                        Debug.DrawRay(s2.point.position, Vector3.up, i % 2 == 0 ? Color.red : Color.green);
                        // tris.Add(triCount++);
                        gotPair = true;

                        break;
                    }
                }
                if (gotPair == false)
                {
                    verts.RemoveAt(verts.Count - 1);
                    verts.RemoveAt(verts.Count - 1);
                    topVert.RemoveAt(topVert.Count - 1);
                    bottomVerts.RemoveAt(topVert.Count - 1);
                    uv.RemoveAt(uv.Count - 1);
                    uv.RemoveAt(uv.Count - 1);
                    // tris.RemoveAt(tris.Count - 1);
                    // tris.RemoveAt(tris.Count - 1);
                    tris.RemoveAt(tris.Count - 1);
                    tris.RemoveAt(tris.Count - 1);
                    triCount--;
                    triCount--;
                    // continue;
                }
                else
                {
                    i--;

                }
            }
        }

        center = center / (float)collectors.Count;
        sandMesh.transform.position = center;
        sandMeshOffset = sandMesh.transform.localPosition;

        //top mesh

        for (int i = 0; i < topVert.Count / 2; i += 2)
        {
            // if (i % 2 == 0) continue;
            // var v1 = transform.TransformPoint(topVert[i]);

            // var v2 = transform.TransformPoint(topVert[i + 1]);
            // var v3 = transform.TransformPoint(topVert[topVert.Count - 1 - i - 1]);
            // var v4 = transform.TransformPoint(topVert[topVert.Count - 1 - i]);

            verts.Add(topVert[topVert.Count - 1 - i]);
            uv.Add(verts[verts.Count - 1]);
            verts.Add(topVert[topVert.Count - 1 - i - 1]);
            uv.Add(verts[verts.Count - 1]);
            verts.Add(topVert[i + 1]);
            uv.Add(verts[verts.Count - 1]);
            verts.Add(topVert[i]);
            uv.Add(verts[verts.Count - 1]);

            tris.Add(triCount++);
            tris.Add(triCount++);
            tris.Add(triCount++);
            tris.Add(triCount++);
        }
        //bottom mesh
        for (int i = 0; i < bottomVerts.Count / 2; i += 2)
        {
            // // if (i % 2 == 0) continue;
            // var v1 = transform.TransformPoint(topVert[i]);
            // var v2 = transform.TransformPoint(topVert[i + 1]);
            // var v3 = transform.TransformPoint(topVert[topVert.Count - 1 - i - 1]);
            // var v4 = transform.TransformPoint(topVert[topVert.Count - 1 - i]);

            verts.Add(bottomVerts[i]);
            uv.Add(verts[verts.Count - 1]);

            verts.Add(bottomVerts[i + 1]);
            uv.Add(verts[verts.Count - 1]);

            verts.Add(bottomVerts[bottomVerts.Count - 1 - i - 1]);
            uv.Add(verts[verts.Count - 1]);

            verts.Add(bottomVerts[bottomVerts.Count - 1 - i]);
            uv.Add(verts[verts.Count - 1]);


            tris.Add(triCount++);
            tris.Add(triCount++);
            tris.Add(triCount++);
            tris.Add(triCount++);
        }

        if (verts.Count > 0)
        {
            verts.Add(topVert[0]);
            uv.Add(verts[verts.Count - 1]);

            verts.Add(bottomVerts[0]);
            uv.Add(verts[verts.Count - 1]);

            verts.Add(bottomVerts[bottomVerts.Count - 1]);
            uv.Add(verts[verts.Count - 1]);
            verts.Add(topVert[topVert.Count - 1]);
            uv.Add(verts[verts.Count - 1]);


            tris.Add(triCount++);
            tris.Add(triCount++);
            tris.Add(triCount++);
            tris.Add(triCount++);

        }


        var mesh = new Mesh();
        // var mesh = sandMesh.mesh;
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.uv = uv.ToArray();
        // mesh.triangles = tris.ToArray();
        mesh.SetIndices(tris.ToArray(), MeshTopology.Quads, 0);


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();


        sandMesh.mesh = mesh;
    }
}