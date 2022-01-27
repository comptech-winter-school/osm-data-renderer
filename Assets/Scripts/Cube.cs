using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // Тут пилится куб
    [SerializeField] float side = 1.0f;
    [SerializeField] Color cubeColor;
    [SerializeField] Vector3 initialPosition;

    /*  Ниже создаётся массив хранящий координаты вершин куба
     *  Почему вершин 24, а не 8? Это сделано чтобы избежать проблем с расчётом освещения
     */
    private Vector3[] cubeVertices = new Vector3[24];
    private int[] cubeTriangles =
    {
        0, 1, 2,    // bottom
        0, 2, 3,

        4, 5, 6,    // front
        4, 6, 7,

        8, 9, 10,   // left
        8, 10, 11,

        12, 13, 14, // right
        12, 14, 15,

        16, 17, 18, // back
        16, 18, 19,

        20, 21, 22, // top
        20, 22, 23,
    };

    private Mesh cube;

    private void initCubeVertices(Vector3[] verts, float cubeSide)
    {
        if (verts.Length != 24)
        {
            Debug.LogWarning("initCubeVertices() got incorrect vertex array");
        }

        // bottom
        verts[0].x = 0.0f; verts[0].y = 0.0f; verts[0].z = cubeSide;
        verts[1].x = 0.0f; verts[1].y = 0.0f; verts[1].z = 0.0f;
        verts[2].x = cubeSide; verts[2].y = 0.0f; verts[2].z = 0.0f;
        verts[3].x = cubeSide; verts[3].y = 0.0f; verts[3].z = cubeSide;

        // front
        verts[4].x = 0.0f; verts[4].y = 0.0f;   verts[4].z = 0.0f;
        verts[5].x = 0.0f; verts[5].y = cubeSide; verts[5].z = 0.0f;
        verts[6].x = cubeSide; verts[6].y = cubeSide; verts[6].z = 0.0f;
        verts[7].x = cubeSide; verts[7].y = 0.0f; verts[7].z = 0.0f;

        // left
        verts[8].x = 0.0f; verts[8].y = 0.0f; verts[8].z = cubeSide;
        verts[9].x = 0.0f; verts[9].y = cubeSide; verts[9].z = cubeSide;
        verts[10].x = 0.0f; verts[10].y = cubeSide; verts[10].z = 0.0f;
        verts[11].x = 0.0f; verts[11].y = 0.0f; verts[11].z = 0.0f;

        // right
        verts[12].x = cubeSide; verts[12].y = 0.0f; verts[12].z = 0.0f;
        verts[13].x = cubeSide; verts[13].y = cubeSide; verts[13].z = 0.0f;
        verts[14].x = cubeSide; verts[14].y = cubeSide; verts[14].z = cubeSide;
        verts[15].x = cubeSide; verts[15].y = 0.0f; verts[15].z = cubeSide;

        // top
        verts[16].x = 0.0f; verts[16].y = cubeSide; verts[16].z = 0.0f;
        verts[17].x = 0.0f; verts[17].y = cubeSide; verts[17].z = cubeSide;
        verts[18].x = cubeSide; verts[18].y = cubeSide; verts[18].z = cubeSide;
        verts[19].x = cubeSide; verts[19].y = cubeSide; verts[19].z = 0.0f;

        // back
        verts[20].x = cubeSide; verts[20].y = 0.0f; verts[20].z = cubeSide;
        verts[21].x = cubeSide; verts[21].y = cubeSide; verts[21].z = cubeSide;
        verts[22].x = 0.0f; verts[22].y = cubeSide; verts[22].z = cubeSide;
        verts[23].x = 0.0f; verts[23].y = 0.0f; verts[23].z = cubeSide;
    }

    Vector3[] TranslateVerices(Vector3[] vertices, Vector3 translate)
    {
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] += translate;
        return vertices;
    }

    // Start is called before the first frame update
    void Start()
    {
        initCubeVertices(cubeVertices, side);
        cubeVertices = TranslateVerices(cubeVertices, transform.position);

        cube = new Mesh();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        meshRenderer.sharedMaterial.SetColor("_Color", cubeColor);
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        cube.vertices = cubeVertices;
        cube.triangles = cubeTriangles;
        cube.RecalculateNormals();
        cube.RecalculateBounds();

        meshFilter.mesh = cube;

        transform.position = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }


}
