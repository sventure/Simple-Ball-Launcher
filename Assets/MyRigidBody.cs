using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidBody : MonoBehaviour
{
    public Vector3 Force;
    public Vector3 Acceleration;
    public Vector3 Velocity;

    public V3 F;
    public V3 Acc;
    public V3 Vel;
    public float mass = 1;

    public MyTransform myTransform;

    //public MyTransform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        F = new V3(Force.x, Force.y, Force.z);
        Acc = new V3(Acceleration.x, Acceleration.y, Acceleration.z);
        Vel = new V3(Velocity.x, Velocity.y, Velocity.z);

        myTransform = this.GetComponent<MyTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        F = new V3(Force.x, Force.y, Force.z);
        Acc = new V3(Acceleration.x, Acceleration.y, Acceleration.z);

        Acc = F / mass;

        Vel += Acc * Time.deltaTime;

        myTransform.Position += Vel.toUnityVector() * Time.deltaTime;

    }

    public V3 GetMomentumAtPoint(V3 point)
    {
        V3 momentum = new V3(0, 0, 0);

        momentum = mass * Vel;

        return momentum;
    }

    public void ImpartMomentum(MyRigidBody otherBody, V3 ContactPoint)
    {
        V3 otherMomentum = otherBody.GetMomentumAtPoint(ContactPoint);
        V3 thisMomentum = GetMomentumAtPoint(ContactPoint);

        V3 SummedMomentum = thisMomentum + otherMomentum;

        ApplyMomentum(SummedMomentum);
        otherBody.ApplyMomentum(SummedMomentum);
    }

    public void ApplyMomentum(V3 momentum)
    {
        Vel = momentum / mass;
    }
}


