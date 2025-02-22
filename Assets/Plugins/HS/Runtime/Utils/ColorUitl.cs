using System;
using UnityEngine;

public static class ColorUtil
{
    public static Vector3 RGBToHSV(Color rgbColor)
    {
        float r = rgbColor.r;
        float g = rgbColor.g;
        float b = rgbColor.b;

        float max = Mathf.Max(r, Mathf.Max(g, b));
        float min = Mathf.Min(r, Mathf.Min(g, b));

        float hue = 0;
        if (max == min)
        {
            hue = 0;
        }
        else if (max == r)
        {
            hue = (60 * (g - b) / (max - min) + 360) % 360;
        }
        else if (max == g)
        {
            hue = (60 * (b - r) / (max - min) + 120) % 360;
        }
        else if (max == b)
        {
            hue = (60 * (r - g) / (max - min) + 240) % 360;
        }

        float saturation = max == 0 ? 0 : 1 - min / max;
        float value = max;

        return new Vector3(hue, saturation, value);
    }

    public static Color HSVToRGB(Vector3 hsvColor)
    {
        float h = hsvColor.x;
        float s = hsvColor.y;
        float v = hsvColor.z;

        float c = v * s;
        float x = c * (1 - Mathf.Abs((h / 60) % 2 - 1));
        float m = v - c;

        float r, g, b;
        if (0 <= h && h < 60)
        {
            r = c;
            g = x;
            b = 0;
        }
        else if (60 <= h && h < 120)
        {
            r = x;
            g = c;
            b = 0;
        }
        else if (120 <= h && h < 180)
        {
            r = 0;
            g = c;
            b = x;
        }
        else if (180 <= h && h < 240)
        {
            r = 0;
            g = x;
            b = c;
        }
        else if (240 <= h && h < 300)
        {
            r = x;
            g = 0;
            b = c;
        }
        else
        {
            r = c;
            g = 0;
            b = x;
        }

        r = (r + m);
        g = (g + m);
        b = (b + m);

        return new Color(r, g, b);
    }


    public static Color RGBToColor(string strColor)
    {
        var color = Convert.ToUInt32(strColor, 16);
        return new Color((color >> 24 & 0xFF) / 255f, (color >> 16 & 0xFF) / 255f,
            (color >> 8 & 0xFF) / 255f, (color & 0xFF) / 255f);
    }

    public static string ColorToRGB(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int a = Mathf.RoundToInt(color.a * 255);

        uint rgba = (uint)(r << 24 | g << 16 | b << 8 | a);
        return rgba.ToString("X8");
    }
}