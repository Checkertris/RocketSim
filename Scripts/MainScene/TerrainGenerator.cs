using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    public  int vSize;
    public  int uSize;
    public float noiseScale;
    public float horzScale;
    public float vertScale;

    private float[] U;
    private float[] V;
    private float[] T;


  void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        GenerateUVT();
        CreateShape();
        UpdateMesh();

    }

void Update()
{
        GenerateUVT();
        CreateShape();
        UpdateMesh();
}


    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

    }

 void GenerateUVT()
    {
        U = new float[(vSize + 1) * (uSize + 1)];
        V = new float[(vSize + 1) * (uSize + 1)];
        T = new float[(vSize + 1) * (uSize + 1)];

        //float Zscale = 2 * Mathf.PI / uSize;
       // float Xscale = Mathf.PI / vSize;

        for (int i = 0, z = 0; z <= uSize; z++)
        {
            for (int x = 0; x <= vSize; x++)
            {
                //float theta = z * Zscale;
              //  float phi = x * Xscale;

              //  U[i] = CalculatePerlinNoise(z,x) * Mathf.Sin(theta) * Mathf.Sin(phi);
              //  V[i] = CalculatePerlinNoise(z,x) * Mathf.Cos(theta) * Mathf.Sin(phi);
              //  T[i] = Mathf.Cos(phi) - 1;
                
                //Original
                U[i] = z - (0.5f*uSize);
                V[i] = x - (0.5f*vSize);
                T[i] = CalculatePerlinNoise(U[i], V[i]) * (Mathf.Pow(U[i], 4) + Mathf.Pow(V[i], 4));

                //Rescale
                U[i] = horzScale * U[i];
                V[i] = horzScale * V[i];
                T[i] = vertScale * T[i];

                i++;
            }
        }
    }

    float CalculatePerlinNoise (float x, float z)
    {
        float xPos = x/noiseScale;
        float zPos = z/noiseScale;
        float returnValue = Mathf.PerlinNoise(xPos,zPos);

        return returnValue;

    }



     void CreateShape()
    {
        vertices = new Vector3[(vSize + 1) * (uSize + 1)];

        for (int i = 0, z = 0; z <= uSize; z++)
        {
            for (int x = 0; x <= vSize; x++)
            {
                vertices[i] = new Vector3(V[i], T[i], U[i]);
                i++;
            }

        }

        triangles = new int[vSize * uSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < uSize; z++)
        {

            for (int x = 0; x < uSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + vSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + vSize + 1;
                triangles[tris + 5] = vert + vSize + 2;

                vert++;
                tris += 6;

            }

            vert++;

        }

    }

}
