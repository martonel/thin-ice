using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class IceBreakSystem : MonoBehaviour
{
    public static IceBreakSystem Instance;

    [Header("Visual")]
    public Material iceMaterial;
    public Color iceColor = new Color(0.6f, 0.9f, 1f, 1.0f);
    public Color iceColor2 = new Color(0.6f, 0.9f, 1f, 1.0f);

    public int pixelsPerUnit = 100;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateBrokenIce(List<Vector2> shape)
    {

        Vector2[] frozen = shape.ToArray(); // 🔒 LEFAGYASZTÁS
        // Belső (kitöltés)
        Sprite fillSprite = CreateSpriteFromPolygon(frozen, out Vector2 pivot);

        // Külső (perem) – enyhén nagyobb
        Sprite edgeSprite = CreateSpriteFromPolygon(
            ExpandPolygonDirectional(
                frozen,
                0.08f,        // max vastagság
                Vector2.up    // "fény iránya"
            ),
            out _
        );

        GameObject root = new GameObject("BrokenIce");
        root.tag = "IceRoot";

        // EDGE
        GameObject edge = new GameObject("Edge");
        edge.transform.SetParent(root.transform, false);
        var edgeSR = edge.AddComponent<SpriteRenderer>();
        edge.AddComponent<LakeFroze>();
        edgeSR.sprite = edgeSprite;
        edgeSR.material = new Material(iceMaterial); // ÚJ PÉLDÁNY LÉTREHOZÁSA
        edgeSR.color = new Color(0.5f, 0.8f, 1f, 0.8f);
        edgeSR.sortingLayerName = "Ice";
        edgeSR.sortingOrder = 1;
        edgeSR.drawMode = SpriteDrawMode.Simple;
        edge.gameObject.layer = 4;
        // FILL
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(root.transform, false);
        var fillSR = fill.AddComponent<SpriteRenderer>();
        fillSR.sprite = fillSprite;
        fillSR.material = new Material(iceMaterial);
        fillSR.color = new Color(0.7f, 0.95f, 1f, 0.5f);
        fillSR.sortingLayerName = "Ice";
        fillSR.sortingOrder = 2;
        fill.AddComponent<LakeFroze>();
        fill.gameObject.layer = 4;


        PolygonCollider2D polygonCollider2D = fill.AddComponent<PolygonCollider2D>();
        polygonCollider2D.isTrigger = true;
        fill.AddComponent<WaterFallTrigger>();

        fillSR.drawMode = SpriteDrawMode.Simple;


        Vector3 p = root.transform.position;
        p.x = Mathf.Round(p.x * pixelsPerUnit) / pixelsPerUnit;
        p.y = Mathf.Round(p.y * pixelsPerUnit) / pixelsPerUnit;
        root.transform.position = p;
    }

    Sprite CreateSpriteFromPolygon(Vector2[] shape, out Vector2 pivot)
    {
        Texture2D tex = CreateTextureFromPolygon(shape, out pivot);

        return Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            pivot,
            pixelsPerUnit,
            0,                        // extrude mértéke
            SpriteMeshType.FullRect   // <--- EZ A KULCS: Tight helyett FullRect
        );
    }

    List<Vector2> ExpandPolygon(List<Vector2> poly, float amount)
    {
        Vector2 center = Vector2.zero;
        foreach (var p in poly) center += p;
        center /= poly.Count;

        List<Vector2> expanded = new List<Vector2>();
        foreach (var p in poly)
        {
            Vector2 dir = (p - center).normalized;
            expanded.Add(p + dir * amount);
        }

        return expanded;
    }

    Vector2[] ExpandPolygonDirectional(
    Vector2[] poly,
    float maxThickness,
    Vector2 lightDir // pl. Vector2.up
)
    {
        Vector2 center = Vector2.zero;
        foreach (var p in poly) center += p;
        center /= poly.Length;

        lightDir.Normalize();

        List<Vector2> expanded = new List<Vector2>();

        foreach (var p in poly)
        {
            Vector2 dirFromCenter = (p - center).normalized;

            // Mennyire "felfelé" néz ez a pont
            float facing = Vector2.Dot(dirFromCenter, lightDir);
            facing = Mathf.Clamp01((facing + 1f) * 0.5f);

            float thickness = maxThickness * facing;

            expanded.Add(p + dirFromCenter * thickness);
        }

        return expanded.ToArray();
    }




    Texture2D CreateTextureFromPolygon(Vector2[] poly, out Vector2 pivot)
    {
        Vector2 min = poly[0];
        Vector2 max = poly[0];

        foreach (var p in poly)
        {
            min = Vector2.Min(min, p);
            max = Vector2.Max(max, p);
        }

        min = new Vector2(
            Mathf.Floor(min.x * pixelsPerUnit) / pixelsPerUnit,
            Mathf.Floor(min.y * pixelsPerUnit) / pixelsPerUnit
        );

        max = new Vector2(
            Mathf.Ceil(max.x * pixelsPerUnit) / pixelsPerUnit,
            Mathf.Ceil(max.y * pixelsPerUnit) / pixelsPerUnit
        );

        int width = Mathf.RoundToInt((max.x - min.x) * pixelsPerUnit);
        int height = Mathf.RoundToInt((max.y - min.y) * pixelsPerUnit);

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        float pivotX = Mathf.Round(-min.x * pixelsPerUnit);
        float pivotY = Mathf.Round(-min.y * pixelsPerUnit);

        pivot = new Vector2(pivotX / width, pivotY / height);

        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        for (int y = 0; y < height; y++)
        {
            float worldY = min.y + y / (float)pixelsPerUnit;
            List<float> intersections = new();

            for (int i = 0; i < poly.Length - 1; i++)
            {
                Vector2 a = poly[i];
                Vector2 b = poly[i + 1];

                if ((a.y <= worldY && b.y > worldY) ||
                    (b.y <= worldY && a.y > worldY))
                {
                    float t = (worldY - a.y) / (b.y - a.y);
                    intersections.Add(Mathf.Lerp(a.x, b.x, t));
                }
            }

            intersections.Sort();

            for (int i = 0; i < intersections.Count; i += 2)
            {
                int x0 = Mathf.RoundToInt((intersections[i] - min.x) * pixelsPerUnit);
                int x1 = Mathf.RoundToInt((intersections[i + 1] - min.x) * pixelsPerUnit);

                for (int x = x0; x <= x1; x++)
                {
                    int idx = y * width + x;
                    if (idx >= 0 && idx < pixels.Length)
                        pixels[idx] = Color.white;
                }
            }
        }

        tex.SetPixels(pixels);
        tex.Apply(false, false); // mipmap OFF, readonly

        return tex;
    }
}