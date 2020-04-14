using UnityEngine;
using UnityEditor;

public class Helper : ScriptableObject
{
    public static float Distance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }

    public static float GetXForce(float x1, float y1, float x2, float y2, float total)
    {
        float xMagnitude = x1 - x2;
        float yMagnitude = y1 - y2;

        if (yMagnitude == 0)
            return total;

        float xForce = (xMagnitude / Mathf.Abs(yMagnitude)) * total;

        if (xForce > total)
            xForce = total;
        if (xForce < total * -1.0f)
            xForce = total * -1.0f;

        return xForce;
    }

    public static float GetYForce(float x1, float y1, float x2, float y2, float total)
    {
        float xMagnitude = x1 - x2;
        float yMagnitude = y1 - y2;

        if (xMagnitude == 0)
            return total;

        float yForce = (yMagnitude / Mathf.Abs(xMagnitude)) * total;

        if (yForce > total)
            yForce = total;
        if (yForce < total * -1.0f)
            yForce = total * -1.0f;

        return yForce;
    }
}