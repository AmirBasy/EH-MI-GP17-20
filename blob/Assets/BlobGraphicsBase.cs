using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobGraphicsBase : MonoBehaviour
{
    public CreateBlob blob;
    public int pointNumber = 30;
    public float ray;



    protected Mesh mesh;
    protected Vector3[] vertices;
    protected Vector3[] directions;

    // Start is called before the first frame update
    protected void Create()
    {
        vertices = new Vector3[pointNumber + 1]; // add a point for center
        directions = new Vector3[pointNumber + 1]; // add a point for center
        Vector2[] uvs = new Vector2[pointNumber + 1];

        vertices[0] = Vector3.zero;
        directions[0] = Vector3.zero;
        uvs[0] = Vector2.one / 2f;

        for (int i = 0; i < pointNumber; i++)
        {
            directions[i + 1] = (Quaternion.Euler(0, 0, 360f / pointNumber * i)) * Vector3.up;
            vertices[i + 1] = directions[i + 1] * ray;
            uvs[i + 1] = (Vector2)vertices[i + 1].normalized / 2 + Vector2.one / 2;
        }

        List<int> indexs = new List<int>();
        for (int i = 0; i < pointNumber; i++)
        {
            if (i == pointNumber - 1)
            {
                indexs.Add(0);
                indexs.Add(i + 1);
                indexs.Add(1);
            }
            else
            {
                indexs.Add(0);
                indexs.Add(i + 1);
                indexs.Add(i + 2);
            }
        }

        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.SetIndices(indexs.ToArray(), MeshTopology.Triangles, 0);



    }

    private void OnDestroy()
    {
        if (mesh != null) Destroy(mesh);
    }
}
