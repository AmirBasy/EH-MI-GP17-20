using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTestCircle : MonoBehaviour
{
    MeshRenderer mr;
    MeshFilter mf;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();

        BuildMesh();
    }

    public int step;
    public float raggio = 4;
    public float lenght = 3;

    

    void BuildMesh()
    {
        // setup the list needed for the mesh
        List<Vector3> vertex = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> indexs = new List<int>();

        // get the up point, will rotate it
        Vector3 point = Vector3.up * raggio;
        Quaternion rotation = Quaternion.AngleAxis(360f / step, Vector3.forward);

        // center point (uv at center)
        vertex.Add(Vector3.zero);
        uvs.Add(Vector2.one / 2);
        // rotate it for steps number
        for (int i = 0; i < step; i++)
        {
            vertex.Add(point);

            // convert the point in planar mapping
            Vector2 uv = (Vector2)((point / raggio / 2f));
            uv.x = -uv.x;
            uv+= (Vector2.one / 2);


            Debug.Log(uv);
            uvs.Add(uv);
            point = rotation * point;
        }

        // build new mesh as triangle strip
        Mesh mesh = new Mesh();
        mesh.subMeshCount = 1;
        for (int i = 1; i < step; i++)
        {
            indexs.Add(0);
            indexs.Add(i);
            indexs.Add(i + 1);
        }
        indexs.Add(0);
        indexs.Add(step);
        indexs.Add(1);

        // setup the mesh with data
        mesh.SetVertices(vertex);
        mesh.SetUVs(0,uvs);
        mesh.SetIndices(indexs.ToArray(), MeshTopology.Triangles, 0);


        // EXAMPLE OF TOPOLOGY TRIANGLE
        /*
         * 
         * 
        indexs.Add(0);
        indexs.Add(1);
        indexs.Add(2);

        indexs.Add(0);
        indexs.Add(2);
        indexs.Add(3);

        mesh.SetIndices(indexs.ToArray(), MeshTopology.Triangles, 0);
        */

        // EXAMPLE OF TOPOLOGY LINES
        /*
        indexs.Add(0);
        indexs.Add(1);

        indexs.Add(1);
        indexs.Add(2);

        indexs.Add(2);
        indexs.Add(3);

        indexs.Add(3);
        indexs.Add(0);

        mesh.SetIndices(indexs.ToArray(), MeshTopology.Lines, 0);
        */

        // EXAMPLE OF TOPOLOGY LINE STRIP
        /*
        indexs.Add(0);
        indexs.Add(1);
        indexs.Add(2);
        indexs.Add(3);
        indexs.Add(0);

        mesh.SetIndices(indexs.ToArray(), MeshTopology.LineStrip, 0);
        */

        // EXAMPLE OF TOPOLOGY QUAD
        /*
        indexs.Add(0);
        indexs.Add(1);
        indexs.Add(2);
        indexs.Add(3);
        mesh.SetIndices(indexs.ToArray(), MeshTopology.Quads, 0);
        */

        // EXAMPLE OF TOPOLOGY POINTS
        /*
        indexs.Add(0);
        indexs.Add(1);
        indexs.Add(2);
        indexs.Add(3);
        mesh.SetIndices(indexs.ToArray(), MeshTopology.Points, 0);
        */


        mf.sharedMesh = mesh;
    }

    private void OnDestroy()
    {
        Destroy(mf.sharedMesh);
    }
}
