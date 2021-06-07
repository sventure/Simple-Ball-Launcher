using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3
{
    public float x, y, z;

    public V3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static V3 AddVectors(V3 a, V3 b)
    {
        V3 rv = new V3(a.x + b.x, a.y + b.y, a.z + b.z);

        return rv;
    }

    public static V3 SubVectors(V3 a, V3 b)
    {
        V3 rv = new V3(a.x - b.x, a.y - b.y, a.z - b.z);

        return rv;
    }

    public Vector3 toUnityVector()
    {
        Vector3 unityVector = new Vector3(x, y, z);

        return unityVector;
    }

    public float Length()
    {
        float mag = 0;
        mag = Mathf.Sqrt(x * x + y * y + z * z);
        return mag;
    }

    public float LengthSq()
    {
        float magsq = 0;
        magsq = x * x + y * y + z * z;
        return magsq;
    }

    public static V3 ScaleVector(V3 a, float scale)
    {
        V3 rv = new V3(0, 0, 0);
        rv.x = a.x * scale;
        rv.y = a.y * scale;
        rv.z = a.z * scale;
        return rv;
    }

    public static V3 DivideVector(V3 a, float div)
    {
        V3 rv = new V3(0, 0, 0);
        rv.x = a.x / div;
        rv.y = a.y / div;
        rv.z = a.z / div;
        return rv;
    }

    public V3 NormalizeVector()
    {
        V3 normVec = new V3(0, 0, 0);

        normVec = DivideVector(this, Length());

        return normVec;
    }

    public static float DotProduct(V3 a, V3 b, bool shouldNormalize)
    {
        float dot = 0;

        if (shouldNormalize)
        {
            V3 normA = a.NormalizeVector();
            V3 normB = b.NormalizeVector();

            dot = normA.x * normB.x + normA.y * normB.y + normA.z * normB.z;
            return dot;
        }
        else
        {
            dot = a.x * b.x + a.y * b.y + a.z * b.z;
            return dot;
        }
    }

    public static V3 operator *(V3 a, float b)
    {
        return ScaleVector(a, b);
    }

    public static V3 operator *(float a, V3 b)
    {
        return ScaleVector(b, a);
    }

    public static V3 operator +(V3 a, V3 b)
    {
        return AddVectors(a, b);
    }

    public static V3 operator -(V3 a, V3 b)
    {
        return SubVectors(a, b);
    }

    public static V3 operator -(V3 a)
    {
        return new V3(-a.x, -a.y, -a.z);
    }

    public static V3 operator /(V3 a, float b)
    {
        return DivideVector(a, b);
    }
}

public class MyMath
{
    public static float VecToRad(Vector2 a)
    {
        float rad = 0;

        rad = Mathf.Atan(a.y / a.x);

        return rad;
    }

    public static Vector2 RadToVec(float angle)
    {
        Vector2 rv = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        return rv;
    }

    public static V3 AngToDirRoll(float angle)
    {
        V3 rv = new V3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        return rv;
    }

    public static V3 AngToDir(float pitch, float yaw)
    {
        V3 rv = new V3(0, 0, 0);

        rv.z = Mathf.Cos(yaw) * Mathf.Cos(-pitch);
        rv.y = Mathf.Sin(-pitch);                                  //x and z are different
        rv.x = Mathf.Cos(-pitch) * Mathf.Sin(yaw);

        return rv;
    }

    public static V3 Cross(V3 a, V3 b)
    {
        V3 rv = new V3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);

        return rv;
    }

    public static Vector3 VecLerpUnity(Vector3 a, Vector3 b, float t)
    {
        return a * (1 - t) + b * t;
    }

    public static V3 VecLerpV3(V3 a, V3 b, float t)
    {
        V3 x = V3.ScaleVector(a, 1 - t);
        V3 y = V3.ScaleVector(b, t);

        return V3.AddVectors(x, y);
    }

    public static V3 RotateVertexAroundAxis(float angle, V3 Axis, V3 Vertex)
    {
        V3 rv = Vertex * Mathf.Cos(angle) +
                V3.DotProduct(Vertex, Axis, true) * Axis * (1.0f - Mathf.Cos(angle)) +
                MyMath.Cross(Axis, Vertex) * Mathf.Sin(angle);

        return rv;
    }

    public static V3 toMyVector(Vector3 unityVector)
    {
        V3 myVector = new V3(unityVector.x, unityVector.y, unityVector.z);
        return myVector;
    }
}

public class Mat4x4
{
    public float[,] value;

    public static Mat4x4 Identity
    {
        get
        {
            return new Mat4x4(
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1)
                );
        }

    }

    public Mat4x4(Vector4 col1, Vector4 col2, Vector4 col3, Vector4 col4)
    {
        value = new float[4, 4];

        value[0, 0] = col1.x;
        value[1, 0] = col1.y;
        value[2, 0] = col1.z;
        value[3, 0] = col1.w;

        value[0, 1] = col2.x;
        value[1, 1] = col2.y;
        value[2, 1] = col2.z;
        value[3, 1] = col2.w;

        value[0, 2] = col3.x;
        value[1, 2] = col3.y;
        value[2, 2] = col3.z;
        value[3, 2] = col3.w;

        value[0, 3] = col4.x;
        value[1, 3] = col4.y;
        value[2, 3] = col4.z;
        value[3, 3] = col4.w;
    }

    public Mat4x4(Vector3 col1, Vector3 col2, Vector3 col3, Vector3 col4)
    {
        value = new float[4, 4];

        value[0, 0] = col1.x;
        value[1, 0] = col1.y;
        value[2, 0] = col1.z;
        value[3, 0] = 0;

        value[0, 1] = col2.x;
        value[1, 1] = col2.y;
        value[2, 1] = col2.z;
        value[3, 1] = 0;

        value[0, 2] = col3.x;
        value[1, 2] = col3.y;
        value[2, 2] = col3.z;
        value[3, 2] = 0;

        value[0, 3] = col4.x;
        value[1, 3] = col4.y;
        value[2, 3] = col4.z;
        value[3, 3] = 1;
    }

    public Mat4x4(V3 col1, V3 col2, V3 col3, V3 col4)
    {
        value = new float[4, 4];

        value[0, 0] = col1.x;
        value[1, 0] = col1.y;
        value[2, 0] = col1.z;
        value[3, 0] = 0;

        value[0, 1] = col2.x;
        value[1, 1] = col2.y;
        value[2, 1] = col2.z;
        value[3, 1] = 0;

        value[0, 2] = col3.x;
        value[1, 2] = col3.y;
        value[2, 2] = col3.z;
        value[3, 2] = 0;

        value[0, 3] = col4.x;
        value[1, 3] = col4.y;
        value[2, 3] = col4.z;
        value[3, 3] = 1;
    }

    public static Vector4 operator *(Mat4x4 lhs, Vector4 rhs)
    {
        Vector4 rv = new Vector4();

        rv.x = lhs.value[0, 0] * rhs.x + lhs.value[0, 1] * rhs.y + lhs.value[0, 2] * rhs.z + lhs.value[0, 3] * rhs.w;
        rv.y = lhs.value[1, 0] * rhs.x + lhs.value[1, 1] * rhs.y + lhs.value[1, 2] * rhs.z + lhs.value[1, 3] * rhs.w;
        rv.z = lhs.value[2, 0] * rhs.x + lhs.value[2, 1] * rhs.y + lhs.value[2, 2] * rhs.z + lhs.value[2, 3] * rhs.w;
        rv.w = lhs.value[3, 0] * rhs.x + lhs.value[3, 1] * rhs.y + lhs.value[3, 2] * rhs.z + lhs.value[3, 3] * rhs.w;

        return rv;
    }


    public static Mat4x4 operator *(Mat4x4 lhs, Mat4x4 rhs)
    {
        Mat4x4 rv = Identity;

        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                rv.value[i, j] = lhs.value[i, 0] * rhs.value[0, j] + lhs.value[i, 1] * rhs.value[1, j] + lhs.value[i, 2] * rhs.value[2, j] + lhs.value[i, 3] * rhs.value[3, j];
            }
        }

        return rv;
    }

    public Vector4 GetRow(int i)
    {
        Mat4x4 rv = this;
        Vector4 row = new Vector4(rv.value[i, 0], rv.value[i, 1], rv.value[i, 2], rv.value[i, 3]);
        return row;
    }

    public Mat4x4 TransInv()
    {
        Mat4x4 rv = this;
        rv.value[0, 3] = -rv.value[0, 3];
        rv.value[1, 3] = -rv.value[1, 3];
        rv.value[2, 3] = -rv.value[2, 3];
        return rv;
    }

    public Mat4x4 RotInv()
    {
        Mat4x4 rv = this;
        return new Mat4x4(this.GetRow(0), this.GetRow(1), this.GetRow(2), this.GetRow(3));
    }

    public Mat4x4 ScaleInv()
    {
        Mat4x4 rv = this;
        rv.value[0, 0] = 1.0f / value[0, 0];
        rv.value[1, 1] = 1.0f / value[1, 1];
        rv.value[2, 2] = 1.0f / value[2, 2];
        return rv;
    }
}

public class Quat
{
    public float w;
    public V3 v;

    public Quat()
    {
        w = 0;
        v = new V3(0, 0, 0);
    }

    public Quat(float angle, V3 Axis)
    {
        float halfang = angle / 2;
        w = Mathf.Cos(halfang);
        v = Axis * Mathf.Sin(halfang);
    }

    public Quat(V3 pos)
    {
        w = 0;
        v = new V3(pos.x, pos.y, pos.z);
    }

    public static Quat operator *(Quat R, Quat S)
    {
        Quat rv = new Quat();

        rv.w = S.w * R.w - V3.DotProduct(S.v, R.v, false);
        rv.v = S.w * R.v + R.w * S.v + MyMath.Cross(R.v, S.v);

        return rv;
    }

    public V3 GetAxis()
    {
        return v;
    }

    public void SetAxis(V3 a)
    {
        v = a;
    }

    public Quat Inv()
    {
        Quat rv = new Quat();

        rv.w = w;
        rv.SetAxis(-GetAxis());

        return rv;
    }

    public Vector4 GetAxisAng()
    {
        Vector4 rv = new Vector4();

        float halfang = Mathf.Acos(w);
        rv.w = halfang * 2;

        rv.x = v.x / Mathf.Sin(halfang);
        rv.y = v.y / Mathf.Sin(halfang);
        rv.z = v.z / Mathf.Sin(halfang);

        return rv;
    }

    public static Quat SLERP(Quat q, Quat r, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);

        Quat d = r * q.Inv();
        Vector4 AxisAng = d.GetAxisAng();
        Quat dT = new Quat(AxisAng.w * t, new V3(AxisAng.x, AxisAng.y, AxisAng.z));

        return dT * q;
    }
}

public class MathLib : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
