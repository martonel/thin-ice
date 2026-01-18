using System.Collections.Generic;
using UnityEngine;


struct IntersectionResult
{
    public bool hit;
    public Vector2 point;
}


[RequireComponent(typeof(LineRenderer))]
public class PlayerIceTrail : MonoBehaviour
{
    [Header("Trail settings")]
    public float pointSpacing = 0.25f;
    public float maxTrailLength = 4f;
    public float trailShrinkSpeed = 1.5f;
    public float minMoveThreshold = 0.01f;

    [Header("Intersection debug")]
    public bool debugDraw = false;

    private List<Vector2> points = new List<Vector2>();
    private float currentTrailLength = 0f;
    private Vector2 lastFramePosition;

    private LineRenderer line;


    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.useWorldSpace = true;
        line.alignment = LineAlignment.View;
    }

    void Start()
    {
        Vector2 startPos = transform.position;
        points.Add(startPos);
        lastFramePosition = startPos;

        line.positionCount = 1;
        line.SetPosition(0, startPos);
    }

    void Update()
    {
        if (points.Count == 0) return; // biztonsági ellenőrzés

        Vector2 currentPos = transform.position;
        float movedDist = Vector2.Distance(currentPos, lastFramePosition);
        lastFramePosition = currentPos;

        bool isMoving = movedDist > minMoveThreshold;

        if (isMoving)
        {
            if (points.Count == 0 || Vector2.Distance(currentPos, points[points.Count - 1]) >= pointSpacing)
            {
                AddPoint(currentPos);
                TrimTrailFromStart();

            }
        }
        else
        {
            ShrinkTrail(Time.unscaledDeltaTime * trailShrinkSpeed);
        }
    }



    void AddPoint(Vector2 newPoint)
    {
        if (points.Count > 0)
        {
            Vector2 lastPoint = points[points.Count - 1];

            // önmetszés ellenőrzés
            if (CheckSelfIntersection(newPoint))
            {

                IceBreak();
                //return;
            }

            float segmentLength = Vector2.Distance(lastPoint, newPoint);
            currentTrailLength += segmentLength;
        }

        points.Add(newPoint);
        TrimTrailToMaxLength();
        UpdateLineRenderer();
    }

    void TrimTrailToMaxLength()
    {
        while (currentTrailLength > maxTrailLength && points.Count > 1)
        {
            RemoveOldestSegment();
        }
    }

    void ShrinkTrail(float amount)
    {
        float remaining = amount;

        while (remaining > 0f && points.Count > 1)
        {
            float segLength = Vector2.Distance(points[0], points[1]);

            if (segLength <= remaining)
            {
                remaining -= segLength;
                currentTrailLength -= segLength;
                points.RemoveAt(0);
            }
            else
            {
                Vector2 dir = (points[1] - points[0]).normalized;
                points[0] += dir * remaining;
                currentTrailLength -= remaining;
                remaining = 0f;
            }
        }

        // ha csak 1 pont maradt
        if (points.Count == 1)
        {
            currentTrailLength = 0f;
        }

        UpdateLineRenderer();
    }

    void RemoveOldestSegment()
    {
        if (points.Count < 2) return; // mindig legalább 2 pont kell

        float segLength = Vector2.Distance(points[0], points[1]);
        currentTrailLength -= segLength;
        points.RemoveAt(0);
    }

    void UpdateLineRenderer()
    {
        if (points.Count == 0)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }

    void RemoveClosedSection(int startIndex, Vector2 intersection)
    {
        // Töröljük a hurkot
        points.RemoveRange(startIndex + 1, points.Count - startIndex - 1);

        // Az utolsó pont legyen a metszéspont
        points.Add(intersection);

        // LineRenderer frissítés
        line.positionCount = points.Count;
        line.SetPositions(points.ConvertAll(p => (Vector3)p).ToArray());
    }

    bool TryGetIntersectionPoint(
    Vector2 a1, Vector2 a2,
    Vector2 b1, Vector2 b2,
    out Vector2 intersection)
    {
        intersection = Vector2.zero;

        float d = (a2.x - a1.x) * (b2.y - b1.y) -
                  (a2.y - a1.y) * (b2.x - b1.x);

        if (Mathf.Abs(d) < 0.0001f)
            return false;

        float u = ((b1.x - a1.x) * (b2.y - b1.y) -
                   (b1.y - a1.y) * (b2.x - b1.x)) / d;

        float v = ((b1.x - a1.x) * (a2.y - a1.y) -
                   (b1.y - a1.y) * (a2.x - a1.x)) / d;

        if (u > 0f && u < 1f && v > 0f && v < 1f)
        {
            intersection = a1 + u * (a2 - a1);
            return true;
        }

        return false;
    }


    // önmetszés
    bool CheckSelfIntersection(Vector2 newPoint)
    {
        if (points.Count < 4) return false;

        Vector2 lastPoint = points[points.Count - 1];

        for (int i = 0; i < points.Count - 3; i++)
        {
            if (TryGetIntersectionPoint(
                points[i], points[i + 1],
                lastPoint, newPoint,
                out Vector2 hit))
            {
                // 1. LOOP KIVÁGÁSA (lefagyasztva)
                List<Vector2> closedShape = ExtractClosedShape(i, hit);
                IceBreakSystem.Instance.CreateBrokenIce(
                    new List<Vector2>(closedShape) // 🔒 teljes leválasztás
                );

                // 2. TRAIL MEGTISZTÍTÁSA
                //RemoveClosedSection(i, hit);

                return true;
            }
        }

        return false;
    }



    void ResetTrail(Vector2 startPoint)
    {
        points.Clear();
        points.Add(startPoint);

        line.positionCount = 1;
        line.SetPosition(0, startPoint);
    }



    List<Vector2> ExtractClosedShape(int startIndex, Vector2 intersection)
    {
        List<Vector2> shape = new List<Vector2>();

        shape.Add(intersection);

        for (int i = startIndex + 1; i < points.Count; i++)
            shape.Add(points[i]); // EZ OK, mert Vector2 value type

        shape.Add(intersection);

        return new List<Vector2>(shape); // 💥 KRITIKUS
    }




    bool LinesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        float d = (a2.x - a1.x) * (b2.y - b1.y) - (a2.y - a1.y) * (b2.x - b1.x);
        if (Mathf.Abs(d) < 0.0001f) return false;

        float u = ((b1.x - a1.x) * (b2.y - b1.y) - (b1.y - a1.y) * (b2.x - b1.x)) / d;
        float v = ((b1.x - a1.x) * (a2.y - a1.y) - (b1.y - a1.y) * (a2.x - a1.x)) / d;

        return (u > 0f && u < 1f && v > 0f && v < 1f);
    }

    void IceBreak()
    {
        Debug.Log("🧊 ICE BROKE!");
        //ClearTrail();

        // ide teheted később:
        // - animáció
        // - player leesés
        // - jég tile reset
    }

    public void ClearTrail()
    {
        points.Clear();
        currentTrailLength = 0f;
        line.positionCount = 0;
    }

    public List<Vector2> GetPoints()
    {
        return points;
    }
    float GetTrailLength()
    {
        float length = 0f;
        for (int i = 1; i < points.Count; i++)
            length += Vector2.Distance(points[i - 1], points[i]);
        return length;
    }

    void TrimTrailFromStart()
    {
        float totalLength = GetTrailLength();

        while (totalLength > maxTrailLength && points.Count > 2)
        {
            float segmentLength = Vector2.Distance(points[0], points[1]);

            points.RemoveAt(0);
            totalLength -= segmentLength;
        }
    }
}

//----------------------------------------------------------------------------------
//----------------------------------------------------------------------------------
//----------------------------------------------------------------------------------
//----------------------------------------------------------------------------------

public static class Triangulator
{
    public static int[] Triangulate(Vector2[] points)
    {
        List<int> indices = new List<int>();

        int n = points.Length;
        if (n < 3) return indices.ToArray();

        int[] V = new int[n];
        if (Area(points) > 0)
            for (int v = 0; v < n; v++) V[v] = v;
        else
            for (int v = 0; v < n; v++) V[v] = (n - 1) - v;

        int nv = n;
        int count = 2 * nv;

        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0) break;

            int u = v;
            if (nv <= u) u = 0;
            v = u + 1;
            if (nv <= v) v = 0;
            int w = v + 1;
            if (nv <= w) w = 0;

            if (Snip(points, u, v, w, nv, V))
            {
                int a = V[u], b = V[v], c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);

                for (int s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];

                nv--;
                count = 2 * nv;
            }
        }

        return indices.ToArray();
    }

    static float Area(Vector2[] points)
    {
        float A = 0;
        for (int p = points.Length - 1, q = 0; q < points.Length; p = q++)
            A += points[p].x * points[q].y - points[q].x * points[p].y;
        return A * 0.5f;
    }

    static bool Snip(Vector2[] points, int u, int v, int w, int n, int[] V)
    {
        Vector2 A = points[V[u]];
        Vector2 B = points[V[v]];
        Vector2 C = points[V[w]];

        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) -
                             ((B.y - A.y) * (C.x - A.x))))
            return false;

        for (int p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w)) continue;
            Vector2 P = points[V[p]];
            if (InsideTriangle(A, B, C, P)) return false;
        }

        return true;
    }

    static bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax = C.x - B.x, ay = C.y - B.y;
        float bx = A.x - C.x, by = A.y - C.y;
        float cx = B.x - A.x, cy = B.y - A.y;
        float apx = P.x - A.x, apy = P.y - A.y;
        float bpx = P.x - B.x, bpy = P.y - B.y;
        float cpx = P.x - C.x, cpy = P.y - C.y;

        float aCROSSbp = ax * bpy - ay * bpx;
        float cCROSSap = cx * apy - cy * apx;
        float bCROSScp = bx * cpy - by * cpx;

        return ((aCROSSbp >= 0f) && (bCROSScp >= 0f) && (cCROSSap >= 0f));
    }
}

