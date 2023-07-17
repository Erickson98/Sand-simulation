
using UnityEngine;

public struct Collector
{
    public Transform point;
    public Vector3 prevPos;
    public Vector3 amount; // cuanta cantidad de arena extraer
    public bool IsUnderground;
    public bool hasSand;
}
