using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlob : MonoBehaviour
{
    public int number=10;
    public float distance = 0.5f;
    public float colliderSize = 0.25f;
    public float frequency = 3;

    internal List<Rigidbody2D> points = new List<Rigidbody2D>();

    Rigidbody2D blob;

    public LayerMask partLayer;

    // Start is called before the first frame update
    void Awake()
    {

        var cCol = this.gameObject.AddComponent<CircleCollider2D>();
        cCol.radius = colliderSize;
        points.Add(blob=this.gameObject.AddComponent<Rigidbody2D>());
        blob.constraints = RigidbodyConstraints2D.FreezeRotation;

        for (int i = 0; i < number; i++)
        {
            GameObject go = new GameObject(i.ToString());
            go.layer = 2;// partLayer;
            go.transform.SetParent(this.transform);
            go.transform.position = this.transform.position + Quaternion.AngleAxis(360f / number * i, Vector3.forward) * Vector3.up * distance;
            var col = go.AddComponent<CircleCollider2D>();
            col.radius = colliderSize;
            var rb = go.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            points.Add(rb);

        }

        for (int i = 0; i < points.Count-1; i++)
        {
            for (int ib = i+1; ib < points.Count; ib++)
            {
                var a = points[i];
                var b = points[ib];
                var s = a.gameObject.AddComponent<SpringJoint2D>();
                s.autoConfigureDistance = false;
                s.distance = Vector3.Distance(a.transform.position, b.transform.position);
                s.connectedBody = b;
                s.enableCollision = true;
                s.frequency = frequency;
            }
        }

        for (int i = 1; i < points.Count; i++)
        {
            var a = points[0];
            var b = points[i];
            var md = a.gameObject.AddComponent<DistanceJoint2D>();
            md.connectedBody = b;
            md.maxDistanceOnly = true;
            md.distance = Vector3.Distance(a.gameObject.transform.position, b.gameObject.transform.position);
        }
    }

    public float velocity = 5;
    public float jump = 3;

    // Update is called once per frame
    void Update()
    {
        float x = blob.velocity.x;
        float y = blob.velocity.y;


        if (Input.GetKey(KeyCode.D)) x = velocity;// Mathf.Lerp(x,-1*velocity,Time.deltaTime*4);   
        if (Input.GetKey(KeyCode.A)) x = -velocity;// Mathf.Lerp(x, 1*velocity, Time.deltaTime * 4);

        blob.velocity = new Vector2(x, y);


        if (Input.GetKeyDown(KeyCode.W))
        {
            foreach (var item in points)
            {
                item.AddForce(Vector2.up * jump);
            }
        }

    }
}
