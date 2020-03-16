using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlobGraphicsBones : BlobGraphicsBase
{
    SkinnedMeshRenderer sr;
    BoneWeight[] weights;

    public float weightBase = 0.1f;

    Transform[] bones;

    (int intero,string pippo) what(string wow)
    {
        return (10,"what!");
    }

    // Start is called before the first frame update
    void Start()
    {
        var ret = what("oco").intero;
 
        sr = GetComponent<SkinnedMeshRenderer>();

        Create();

        weights = new BoneWeight[pointNumber + 1];

        weights[0].boneIndex0 = 0;
        weights[0].weight0 = 1;

        for (int i = 0; i < pointNumber; i++)
        {
            Vector3 where = this.transform.TransformPoint(directions[i + 1] * ray);

            // order by distance from this point
            var points = blob.points.OrderBy(a => (a.transform.position - where).sqrMagnitude).ToArray();

            weights[i+1].boneIndex0 = blob.points.IndexOf(points[0]);
            weights[i + 1].weight0 = 1-Mathf.Clamp01((points[0].transform.position - where).magnitude/ weightBase  )*0.8f;
            
            weights[i+1].boneIndex1 = blob.points.IndexOf(points[1]);
            weights[i+1].weight1 = 1-Mathf.Clamp01((points[1].transform.position - where).magnitude/ weightBase)*0.9f;

            weights[i+1].boneIndex2 = blob.points.IndexOf(points[2]);
            weights[i+1].weight2 = 1-Mathf.Clamp01((points[2].transform.position - where).magnitude / weightBase);
        }

        mesh.boneWeights = weights;

        bones = new Transform[blob.points.Count];
        Matrix4x4[] bindPoses = new Matrix4x4[blob.points.Count];


        for (int i = 0; i < blob.points.Count; i++)
        {
            GameObject go = new GameObject(i.ToString() + "bone");
            go.transform.SetParent(blob.transform);
            go.transform.position = blob.points[i].transform.position;
            go.transform.LookAt(blob.transform, Vector3.forward);
            bones[i] = go.transform;
            bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
        }

        mesh.bindposes = bindPoses;

        sr.bones = bones;
        sr.sharedMesh = mesh;
    }

    private void Update()
    {
        for (int i = 0; i < blob.points.Count; i++)
        {
            bones[i].transform.position = blob.points[i].transform.position;
            bones[i].transform.LookAt(blob.transform, Vector3.forward);

        }
    }

}
