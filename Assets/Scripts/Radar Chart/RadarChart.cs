using UnityEngine;
using UnityEngine.UI;

public class RadarChart : Graphic
{
    [SerializeField] private float radius = 100f; // Radius of the radar chart
    [SerializeField] private float lineWidth = 2f; // Width of the axis lines
    public float[] values; // Array of values for the 7 categories, set externally

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        // Check if values are provided and match the expected count
        if (values == null || values.Length != 7)
            return;

        int categoryCount = values.Length;
        float angleStep = 360f / categoryCount;
        Vector2 center = Vector2.zero;

        // Add center vertex for the data polygon
        vh.AddVert(center, color, Vector2.zero);

        // Add vertices for axes
        for (int i = 0; i < categoryCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 axisEnd = dir * radius;
            Vector2 perp = new Vector2(-dir.y, dir.x).normalized * (lineWidth / 2f);
            Vector2 axisLeft = axisEnd - perp;
            Vector2 axisRight = axisEnd + perp;

            vh.AddVert(axisLeft, Color.gray, Vector2.zero);
            vh.AddVert(axisRight, Color.gray, Vector2.zero);
        }

        // Add vertices for data points
        for (int i = 0; i < categoryCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float value = Mathf.Clamp01(values[i]); // Ensure value is between 0 and 1
            Vector2 dataPoint = dir * (value * radius);
            vh.AddVert(dataPoint, color, Vector2.zero);
        }

        // Add triangles for axes
        for (int i = 0; i < categoryCount; i++)
        {
            int leftIdx = 1 + 2 * i;
            int rightIdx = 2 + 2 * i;
            vh.AddTriangle(0, leftIdx, rightIdx);
        }

        // Add triangles for data polygon (triangle fan)
        int dataStart = 1 + 2 * categoryCount;
        for (int i = 0; i < categoryCount; i++)
        {
            int next = (i + 1) % categoryCount;
            vh.AddTriangle(0, dataStart + i, dataStart + next);
        }
    }
}