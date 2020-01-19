using Microsoft.Xna.Framework;
using System;

public static class NumberHelper
{
    public static Color Mix(this Color color, Color other)
    {
        var r = (color.R + other.R) / 2;
        var g = (color.G + other.G) / 2;
        var b = (color.B + other.B) / 2;
        var a = (color.A + other.A) / 2;
        return new Color(r, g, b);
    }

    //simple exponential cap
    public static float ExpCap(this float x, float yintercept, float plateau, float increase) => (float)(plateau - (plateau - yintercept) * Math.Exp(-increase * x));
}