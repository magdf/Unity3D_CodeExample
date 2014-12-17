using System;

public static class ObjectExt
{
    public static bool In<T>(this T value, params T[] array)
    {
        int index = Array.IndexOf( array, value );
        return (index>-1);
    }
}
