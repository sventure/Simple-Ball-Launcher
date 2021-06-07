using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoundingSphere : MonoBehaviour
{
    public V3 CentrePoint;
    public float rad = 0.5f;

    public MyRigidBody Ball;
    public MyTransform myTransform;
    public GameManager gameManager;

    public Text DeathVictText;
    bool bounce = false;

    void Start()
    {
        myTransform = GetComponent<MyTransform>();
        DeathVictText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CentrePoint = new V3(myTransform.Position.x * 2f, myTransform.Position.y * 2f, myTransform.Position.z * 2f);
    }

    void FixedUpdate()
    {
        BoundingSphere[] allSpheresinScene = FindObjectsOfType<BoundingSphere>();
        AABB[] boxes = FindObjectsOfType<AABB>();

        foreach (BoundingSphere c in allSpheresinScene)
        {
            if (c != this)
            {
                if (Intersects(c))
                {
                    Debug.Log(gameObject.name + " is intersecting with " + c.name);
                }
            }
        }

        foreach (AABB box in boxes)
        {
            if (box != this)
            {
                if (AABBIntersect(box) && box.gameObject.name == "Plane")
                {
                    Debug.Log(this.gameObject.name + " intersects " + box.gameObject.name);

                    DeathVictText.enabled = true;
                    DeathVictText.text = "Death!";

                    myTransform.Position = new Vector3(myTransform.Position.x, 0, myTransform.Position.z);
                    Ball.Force = Vector3.zero;
                    Ball.Velocity = Vector3.zero;
                    Ball.Acceleration = Vector3.zero;

                    Time.timeScale = 0f;
                }
                else if (AABBIntersect(box) && box.gameObject.name == "Bounce")
                {
                    Debug.Log(this.gameObject.name + " intersects " + box.gameObject.name);
                    Ball.Force += new Vector3(20f, 20f, 0f);
                }
                else if (AABBIntersect(box) && box.gameObject.name == "End")
                {
                    Debug.Log(this.gameObject.name + " intersects " + box.gameObject.name);

                    DeathVictText.enabled = true;
                    DeathVictText.text = "Victory!";

                    myTransform.Position = new Vector3(myTransform.Position.x, 0, myTransform.Position.z);
                    Ball.Force = Vector3.zero;
                    Ball.Velocity = Vector3.zero;
                    Ball.Acceleration = Vector3.zero;

                    Time.timeScale = 0f;
                }
                else
                {
                    //bounce = false;
                }
            }
        }
    }

    public bool Intersects(BoundingSphere otherCircle)
    {
        V3 VectorToOther;
        VectorToOther = otherCircle.CentrePoint - CentrePoint;
        float CombinedRadiusSq = otherCircle.rad + rad;
        CombinedRadiusSq *= CombinedRadiusSq;

        return VectorToOther.LengthSq() <= CombinedRadiusSq;
    }

    public bool AABBIntersect(AABB otherBox)
    {
        float x = Mathf.Max(otherBox.Left, Mathf.Min(this.CentrePoint.x, otherBox.Right));
        float y = Mathf.Max(otherBox.Bottom, Mathf.Min(this.CentrePoint.y, otherBox.Top));
        float z = Mathf.Max(otherBox.Back, Mathf.Min(this.CentrePoint.z, otherBox.Front));

        float distance = (x - this.CentrePoint.x) * (x - this.CentrePoint.x) +
                                    (y - this.CentrePoint.y) * (y - this.CentrePoint.y) +
                                    (z - this.CentrePoint.z) * (z - this.CentrePoint.z);

        return distance < this.rad * this.rad;
    }
}
