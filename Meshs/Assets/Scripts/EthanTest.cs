using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanTest : MonoBehaviour
{
    public SkinnedMeshRenderer mr;

    Mesh mesh;
    Vector3[] zeros, normals;
    BoneWeight[] boneWeights;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        BuildMeshSphape();
    }

    void BuildMeshSphape()
    {
        mesh = Instantiate(mr.sharedMesh);


        mesh.ClearBlendShapes();
        // get the root bone
        Transform rootBone = mr.rootBone;

        

        // zero vertex for normals and tangent
        zeros = new Vector3[mesh.vertexCount];

        // cache info
        boneWeights = mesh.boneWeights;
        normals = mesh.normals;


        // track time
        startTime = Time.realtimeSinceStartup;

        // build shape for each bone
        for (int i = 0; i < mr.bones.Length; i++)
        {
            BuildShape(mesh,mr.bones[i],i);
        }


        mr.sharedMesh = mesh;
        Debug.LogFormat("Total Time:{0} for vertex:{1}", Time.realtimeSinceStartup - startTime, zeros.Length);
    }



    private void BuildShape(Mesh mesh, Transform target, int index)
    {
        Vector3[] offsets = new Vector3[mesh.vertexCount];


        bool hasWeight = false;

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector3 baseOffset = normals[i];
            float maxWeight = 0;
            // get max influence of vertex
            maxWeight = Mathf.Max(
                
                boneWeights[i].boneIndex0 == index ? boneWeights[i].weight0:0
                , boneWeights[i].boneIndex1 == index ? boneWeights[i].weight1:0 
                , boneWeights[i].boneIndex2 == index ? boneWeights[i].weight2:0
                , boneWeights[i].boneIndex3 == index ? boneWeights[i].weight3:0  
            
                );

            // track only bones with weight
            if (maxWeight > 0) hasWeight = true;

            // proportional offset
            offsets[i] = baseOffset * maxWeight;
        }

        // track only bones with weight
        if (hasWeight) mesh.AddBlendShapeFrame(target.name, 1, offsets, zeros, zeros);

        // track time
        Debug.LogFormat("Time for bone {0}: {1}", target.name, Time.realtimeSinceStartup - startTime);

    }

    private void OnDestroy()
    {
        if (mesh != null) Destroy(mesh);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
