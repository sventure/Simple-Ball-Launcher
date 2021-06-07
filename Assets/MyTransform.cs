using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransform : MonoBehaviour
{
    public Vector3 Position = Vector3.zero;
    public Vector3 Rotation = Vector3.zero;
    public Vector3 Scale = new Vector3(1, 1, 1);
    Vector3[] ModelSpaceVerts;

    public Quat k;

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter MF = GetComponent<MeshFilter>();

        ModelSpaceVerts = MF.mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Position;
        transform.rotation = Quaternion.Euler(Rotation);
        transform.localScale = Scale;

        MeshFilter MF = GetComponent<MeshFilter>();

        Vector3[] TransformedVerts = new Vector3[ModelSpaceVerts.Length];

        //Quat rotY = new Quat(Rotation.y, new V3(0, -1, 0));
        //V3 yrot = MyMath.toMyVector(Rotation);
        //Quat yk = new Quat(yrot);
        //Quat newyK = rotY * yk * rotY.Inv();
        //V3 newyP = newyK.GetAxis();

        //Quat rotx = new Quat(Rotation.x, new V3(1, 0, 0));
        //V3 xrot = MyMath.toMyVector(Rotation);
        //Quat xk = new Quat(xrot);
        //Quat newxK = rotx * xk * rotx.Inv();
        //V3 newxP = newxK.GetAxis();

        //Quat rotz = new Quat(Rotation.z, new V3(0, 0, 1));
        //V3 zrot = MyMath.toMyVector(Rotation);
        //Quat zk = new Quat(zrot);
        //Quat newzK = rotz * zk * rotz.Inv();
        //V3 newzP = newzK.GetAxis();

        //Quat RotInY = new Quat(Rotation.y, new V3(0, -1, 0));
        //Quat RotInX = new Quat(Rotation.x, new V3(1, 0, 0));
        //Quat RotInZ = new Quat(Rotation.z, new V3(0, 0, 1));
        //V3 rot = MyMath.toMyVector(Rotation);
        //Quat rotQuat = new Quat(rot);
        //k = RotInZ * rotQuat * RotInZ.Inv();
        //k *= RotInY * rotQuat * RotInY.Inv();
        //k *= RotInX * rotQuat * RotInX.Inv();

        float x = Rotation.x;
        float y = Rotation.y;
        float z = Rotation.z;

        //Mat4x4 quatRot = new Mat4x4(new Vector3(1.0f - 2.0f * (y * y + z * z), 2.0f * (x * y - z * w), 2.0f * (x * z + y * w)),
        //                            new Vector3(2.0f * (x * y + z * w), 1.0f - 2.0f * (x * x + z * z), 2.0f * (y * z - x * w)),
        //                            new Vector3(2.0f * (x * z - y * w), 2.0f * (y * z + x * w), 1.0f - 2.0f * (x * x + y * y)),
        //                            Vector3.zero);

        //Mat4x4 quatRotTransposed = new Mat4x4(quatRot.GetRow(0), quatRot.GetRow(1), quatRot.GetRow(2), quatRot.GetRow(3));

        Mat4x4 S = new Mat4x4(new Vector3(Scale.x, 0, 0),
                                new Vector3(0, Scale.y, 0),
                                new Vector3(0, 0, Scale.z),
                                Vector3.zero);

        Mat4x4 T = new Mat4x4(new Vector3(1, 0, 0),
                                     new Vector3(0, 1, 0),
                                     new Vector3(0, 0, 1),
                                     Position);

        Mat4x4 rollMat = new Mat4x4(new Vector3(Mathf.Cos(z * 0.0174533f), Mathf.Sin(z * 0.0174533f), 0),
                                     new Vector3(-Mathf.Sin(z * 0.0174533f), Mathf.Cos(z * 0.0174533f), 0),
                                     new Vector3(0, 0, 1),
                                     Vector3.zero);

        Mat4x4 pitchMat = new Mat4x4(new Vector3(1, 0, 0),
                                     new Vector3(0, Mathf.Cos(x * 0.0174533f), Mathf.Sin(x * 0.0174533f)),
                                     new Vector3(0, -Mathf.Sin(x * 0.0174533f), Mathf.Cos(x * 0.0174533f)),
                                     Vector3.zero);

        Mat4x4 yawMat = new Mat4x4(new Vector3(Mathf.Cos(y * 0.0174533f), 0, -Mathf.Sin(y * 0.0174533f)),
                                     new Vector3(0, 1, 0),
                                     new Vector3(Mathf.Sin(y * 0.0174533f), 0, Mathf.Cos(y * 0.0174533f)),
                                     Vector3.zero);

        Mat4x4 R = yawMat * (pitchMat * rollMat);
        Mat4x4 M = T * (R * S);

        for (int i = 0; i < TransformedVerts.Length; i++)
        {
            TransformedVerts[i] = M * new Vector4(ModelSpaceVerts[i].x, ModelSpaceVerts[i].y, ModelSpaceVerts[i].z, 1);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Mat4x4 Minv = S.ScaleInv() * (R.RotInv() * T.TransInv());

            for (int i = 0; i < TransformedVerts.Length; i++)
            {
                TransformedVerts[i] = Minv * new Vector4(TransformedVerts[i].x, TransformedVerts[i].y, TransformedVerts[i].z, 1);
            }
        }

        //MF.mesh.vertices = ScaledVerts;
        MF.mesh.vertices = TransformedVerts;

        MF.mesh.RecalculateNormals();
        MF.mesh.RecalculateBounds();

        //Mat4x4 Tinv = T.TransInv();
        //string s = "";

        //for(int i = 0; i <= 3;  i++)
        //{
        //    for (int j = 0; j <= 3; j++)
        //    {
        //        s += Tinv.value[i, j].ToString() + " ";
        //    }

        //    s += "\n";
        //}

        //Debug.Log(s);

        //AABB theBox = new AABB(new Vector3(0, 0, 0), new Vector3(3, 3, 3));

        //Vector3 LineStart = new Vector3(-2, -2, -2);
        //Vector3 LineEnd = new Vector3(3, 4, 5);

        //Vector3 IntersectionPt;

        //if(AABB.LineIntersection(theBox,LineStart,LineEnd, out IntersectionPt))
        //{
        //    Debug.Log("Intersecting at " + IntersectionPt);
        //}
        //else
        //{
        //    Debug.Log("no");
        //}

    }
}
