using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct iPoint
{
    public float x, y;

    public iPoint(float newX, float newY)
    {
        x = newX;
        y = newY;
    }
    //Vector3 v = new Vector3(1, 2, 3);
    //iPoint p = (iPoint)v;
    public static implicit operator Vector3(iPoint p)
    {
        return new Vector3(p.x, p.y, 0f);
    }//벡터 포인트를 iPoint로 대임
    
    public static implicit operator iPoint(Vector3 v)
    {
        return new iPoint(v.x, v.y);
    }//vector3 -> ipoint

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(iPoint p0, iPoint p1)
    {
        return (p0.x == p1.x && p0.y == p1.y);
    }

    public static bool operator !=(iPoint p0, iPoint p1)
    {
        return (p0.x != p1.x || p0.y != p1.y);
    }

    public static iPoint operator +(iPoint p0, iPoint p1)
    {
        iPoint p;
        p.x = p0.x + p1.x;
        p.y = p0.y + p1.y;

        return p;
    }
    public static iPoint operator -(iPoint p0, iPoint p1)
    {
        iPoint p;
        p.x = p0.x - p1.x;
        p.y = p0.y - p1.y;

        return p;
    }
    public static iPoint operator *(iPoint p0, float s)
    {
        iPoint p;
        p.x = p0.x * s;
        p.y = p0.y * s;

        return p;
    }
    public static iPoint operator /(iPoint p0, float s)
    {
        iPoint p;
        p.x = p0.x / s;
        p.y = p0.y / s;

        return p;
    }
}
