using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UI_RadarChart : MonoBehaviour {

    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture2D;
    [SerializeField] private List<TMP_Text> categoriesText;

    private Dictionary<string, int> categoriesElo;
    public Dictionary<string, int> CategoriesElo
    {
        get { return categoriesElo; }
        set { categoriesElo = value; }
    }

    private CanvasRenderer radarMeshCanvasRenderer;

    private void Awake() {
        radarMeshCanvasRenderer = GetComponent<CanvasRenderer>();
    }

    public void SetElo(Dictionary<string, int> CategoriesElo) {
        this.CategoriesElo = CategoriesElo;
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 * 5];

        float angleIncrement = 360f / 5;
        float radarChartSize = 217f;
        QuestionsCategories[] questionsCategories = (QuestionsCategories[])Enum.GetValues(typeof(QuestionsCategories));
        vertices[0] = Vector3.zero;
        uv[0] = Vector2.zero;

        for (int i = 1; i < vertices.Length; i++)
        {
            categoriesText[i - 1].text = questionsCategories[i].GetKey();
            int value;
            if (CategoriesElo.TryGetValue(questionsCategories[i - 1].GetKey(), out value))
            {
                value = CategoriesElo[questionsCategories[i - 1].GetKey()];
            }
            else
            {
                value = 0;
            }
            float normalizedValue =(float) value / EloSystemManager.EstimatedMaxElo;
            if (normalizedValue > 1)
            {
                normalizedValue = 1;
            }
            float angle = (i - 1) * -angleIncrement;
            vertices[i] = Quaternion.Euler(0, 0, angle) * (Vector3.up * radarChartSize * normalizedValue);
            uv[i] = Vector2.one;

            // Assign triangles
            triangles[(i - 1) * 3] = 0;
            triangles[(i - 1) * 3 + 1] = i;
            triangles[(i - 1) * 3 + 2] = (i == vertices.Length - 1) ? 1 : i + 1; // Wrap to the first vertex
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture2D);
    }






}

