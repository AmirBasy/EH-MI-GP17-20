using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobExample : MonoBehaviour
{
    public MeshFilter filter;
    public MeshRenderer render;
    Vector3[] originalPoints;
    Vector3[] positions;

    // Start is called before the first frame update
    void Start()
    {
        originalPoints = filter.sharedMesh.vertices;
        positions = filter.sharedMesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        for (int i = 0; i < positions.Length; i++)
        {
            Ray ray = new Ray(this.transform.position, originalPoints[i]);
            float distance = 1;
            if (Physics.Raycast(ray,out hit,1))
            {
                distance = (hit.point - this.transform.position).magnitude;
            }

            positions[i] = originalPoints[i] + originalPoints[i].normalized * distance;



        }

        filter.mesh.vertices = positions;
    }
}
