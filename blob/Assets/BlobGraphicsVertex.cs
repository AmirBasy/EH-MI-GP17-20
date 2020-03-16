using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobGraphicsVertex : BlobGraphicsBase
{

    // Start is called before the first frame update
    void Start()
    {
        Create();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pointNumber; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.TransformDirection(directions[i + 1]), ray);

            if (hit.collider!=null)
            {
                vertices[i + 1] = this.transform.InverseTransformPoint(hit.point);
            }
            else vertices[i+1] = directions[i + 1] * ray;
        }

        mesh.vertices = vertices;
    }

}
