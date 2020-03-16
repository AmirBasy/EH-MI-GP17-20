using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexCubeExample : MonoBehaviour
{
    public int columns=3;
    public int rows = 3;

    SkinnedMeshRenderer mr;
    MeshFilter mf;

    // setup the list needed for the mesh
    List<Vector3> vertex = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> indexs = new List<int>();

    Transform lowerBone, upperBone, topBone;


    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<SkinnedMeshRenderer>();

        BuildMesh();
    }

    void BuildMesh()
    {
        Vector3 rowDirection = Vector3.up;
        Vector3 colDirection = Vector3.right;
        Vector3 startPoint = Vector3.forward/2 - rowDirection/2 - colDirection/2;

        // build 4 face
        for (int rotation = 0; rotation <=270; rotation+=90)
        {
            BuildFace(startPoint, rowDirection, colDirection, Quaternion.AngleAxis(rotation, Vector3.up), false);
        }
        
        // top and down face
        BuildFace(startPoint, rowDirection, colDirection, Quaternion.AngleAxis(90, Vector3.right), false);
        BuildFace(startPoint, rowDirection, colDirection, Quaternion.AngleAxis(-90, Vector3.right), false);


        

        Mesh mesh = new Mesh();
        mesh.subMeshCount = 1;

        // setup the mesh with data
        mesh.SetVertices(vertex);
        mesh.SetUVs(0, uvs);
        mesh.SetIndices(indexs.ToArray(), MeshTopology.Quads, 0);

        CalculateNormals(mesh);

        mr.sharedMesh = mesh;

        BuildBones(mesh);

        BuildBlendShape(mesh);
    }

    void BuildBlendShape(Mesh mesh)
    {
        Vector3[] zeros = new Vector3[vertex.Count];
        Vector3[] offset = new Vector3[vertex.Count];
        Vector3[] other = new Vector3[vertex.Count];

        for (int i = 0; i < vertex.Count; i++)
        {
            offset[i] = (vertex[i].normalized * 1) - vertex[i];
            other[i] = ((vertex[i].normalized * 1) - vertex[i]) * vertex[i].y;
        }

        mesh.AddBlendShapeFrame("Spherizer", 1, offset, zeros, zeros);
        mesh.AddBlendShapeFrame("Y-Spherizer", 1, other, zeros, zeros);
    }

    void CalculateNormals(Mesh mesh)
    {
        Vector3[] normals = new Vector3[vertex.Count];

        for (int i = 0; i < vertex.Count; i++)
        {
            normals[i] = vertex[i].normalized;
        }

        mesh.normals = normals;
        
    }


    void BuildBones(Mesh mesh)
    {
        lowerBone = new GameObject("Lower").transform;
        lowerBone.parent = this.transform;
        lowerBone.localPosition = -Vector2.up / 2;
        lowerBone.localRotation = Quaternion.identity;

        upperBone = new GameObject("Upper").transform;
        upperBone.parent = this.transform;
        upperBone.localPosition = Vector2.up / 2;

        topBone = new GameObject("Hat").transform;
        topBone.parent = this.transform;
        topBone.localPosition = Vector2.right/2 + Vector2.up/2;
        topBone.localScale = Vector3.one;
        topBone.localRotation = Quaternion.identity;

        Matrix4x4[] bindBonePoses = new Matrix4x4[]
            {
            lowerBone.worldToLocalMatrix * transform.localToWorldMatrix
            ,upperBone.worldToLocalMatrix * transform.localToWorldMatrix
            ,topBone.worldToLocalMatrix *  transform.localToWorldMatrix
            };

        topBone.parent = upperBone;


        Transform[] bones = new Transform[] { lowerBone, upperBone, topBone };
        
        mesh.bindposes = bindBonePoses;
        mr.bones = bones;

        BoneWeight[] boneWeights = new BoneWeight[vertex.Count];
        for (int i = 0; i < vertex.Count; i++)
        {
            float y = vertex[i].y;

            float lowerDistance = Mathf.Abs(lowerBone.localPosition.y - y);
            float upperDistance = Mathf.Abs(upperBone.localPosition.y - y);

            Debug.LogFormat("{0} {1}", lowerDistance, upperDistance);

            boneWeights[i].boneIndex2 = 2;
            // is the top face?
            boneWeights[i].weight2 = 0;
            if ((i >= columns * rows * 5) && (i < columns * rows * 6)) boneWeights[i].weight2 = 1f;
            else
            {
                if (upperDistance > lowerDistance)
                {
                    boneWeights[i].boneIndex0 = 0;
                    boneWeights[i].weight0 = 1 - lowerDistance;

                    boneWeights[i].boneIndex1 = 1;
                    boneWeights[i].weight1 = 1 - upperDistance;

                    //boneWeights[i].boneIndex0 = 0;
                    //boneWeights[i].weight0 = 1 - upperDistance;

                }
                else
                {
                    boneWeights[i].boneIndex0 = 1;
                    boneWeights[i].weight0 = 1 - upperDistance;

                    boneWeights[i].boneIndex1 = 0;
                    boneWeights[i].weight1 = 1 - lowerDistance;

                    //boneWeights[i].boneIndex0 = 0;
                    //boneWeights[i].weight0 = 1 - lowerDistance;

                }
            }
            
            
            
        }

        mesh.boneWeights = boneWeights;

    }

    void BuildFace(Vector3 baseStartPoint,  Vector3 baseRowDirection, Vector3 baseColDirection, Quaternion rotation, bool normalOrder)
    {
        
        Vector3 startPoint = rotation * baseStartPoint;
        Vector3 rowDirection = rotation * baseRowDirection;
        Vector3 colDirection = rotation * baseColDirection;

        float normalizedRowStepSize = 1f / (rows - 1);
        float normalizedColStepSize = 1f / (columns - 1);

        int startIndex = vertex.Count;

        // create vertex
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                uvs.Add(new Vector2(1-col * normalizedColStepSize, row * normalizedRowStepSize));
                vertex.Add(startPoint + rowDirection * row * normalizedRowStepSize + colDirection * col * normalizedColStepSize);
            }
        }

        // create topology
        for (int col = 0; col < columns-1; col++)
        {
            int baseIndex = col * rows + startIndex;

            for (int row = 0; row < rows-1; row++)
            {
                if (normalOrder)
                {
                    indexs.Add(baseIndex + row);
                    indexs.Add(baseIndex + row + 1);
                    indexs.Add(baseIndex + row + 1 + rows);
                    indexs.Add(baseIndex + row + rows);
                }
                else
                {
                    indexs.Add(baseIndex + row + rows);
                    indexs.Add(baseIndex + row + 1 + rows);
                    indexs.Add(baseIndex + row + 1);
                    indexs.Add(baseIndex + row);
                }
            }
        }

    }

    public float velocity = 2;
    public float rotationvelocity = 90;

    public float animationForwardEffect = 30;
    public float animationRotationEffect = 45;

    public float animationForwardMoveEffect = 0.2f;

    public float stepAnimation = 20;
    public float stepAnimationCicle = 20;

    private void Update()
    {
        float rot = Input.GetAxis("Horizontal");
        float forw = Input.GetAxis("Vertical");

        upperBone.localEulerAngles = new Vector3(animationForwardEffect * forw, animationRotationEffect * rot, 0);
        upperBone.localPosition = Vector3.up/2f+ Vector3.forward * forw * animationForwardMoveEffect;

        lowerBone.localEulerAngles = Vector3.up * Mathf.Cos(Time.time * stepAnimationCicle) * stepAnimation * forw;

        this.transform.Translate(Vector3.forward * velocity * Time.deltaTime * forw, Space.Self);
        this.transform.Rotate(Vector3.up * rotationvelocity * Time.deltaTime * rot);

        mr.SetBlendShapeWeight(0, Mathf.Abs(forw));
        mr.SetBlendShapeWeight(1, Mathf.Abs(rot));
    }

}
