using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour
{
    public Vector3 MinExtent;
    public Vector3 MaxExtent;

    public float Top { get { return MaxExtent.y + transform.position.y; } }
    public float Bottom { get { return MinExtent.y + transform.position.y; } }
    public float Right { get { return MaxExtent.x + transform.position.x; } }
    public float Left { get { return MinExtent.x + transform.position.x; } }
    public float Front { get { return MaxExtent.z + transform.position.z; } }
    public float Back { get { return MinExtent.z + transform.position.z; } }

    void Start()
    {
        //MinExtent.x = -transform.localScale.x / 2;
        //MinExtent.y = -transform.localScale.y / 2;
        //MinExtent.z = -transform.localScale.z / 2;

        //MaxExtent.x = transform.localScale.x / 2;
        //MaxExtent.y = transform.localScale.y / 2;
        //MaxExtent.z = transform.localScale.z / 2;
    }

    // Update is called once per frame
    void Update()
    {
        AABB[] boxes = FindObjectsOfType<AABB>();

        foreach (AABB box in boxes)
        {
            if (box != this)
            {
                if (Intersects(this, box))
                {
                    Debug.Log(this.gameObject.name + " intersects " + box.gameObject.name);
                }
            }
        }
    }

    public static bool Intersects(AABB Box1, AABB Box2)
    {
        return !(Box2.Left > Box1.Right
              || Box2.Right < Box1.Left
              || Box2.Top < Box1.Bottom
              || Box2.Bottom > Box1.Top
              || Box2.Back > Box1.Front
              || Box2.Front < Box1.Back);
    }
}
