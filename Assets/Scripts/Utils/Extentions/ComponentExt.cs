using System.Collections.Generic;
using UnityEngine;

public static class ComponentExt
{
    public static I GetInterfaceComponent<I>(this Component self) where I : class
    {
        return self.GetComponent(typeof(I)) as I;
    }

    public static I[] GetInterfaceComponents<I>(this Component self) where I : class
    {
        var components = self.GetComponents(typeof(I));
        I[] Icomponents = new I[components.Length];
        components.CopyTo(Icomponents, 0);
        return Icomponents;
    }

    public static T GetSafeComponent<T>(this Component self) where T : Component
    {
        T component = self.GetComponent<T>();
        if (component == null)
            Debug.LogError("Component of type " + typeof(T) + " not found", self);
        return component;
    }

    public static T[] GetComponentsInDirectChildrens<T>(this Component self) where T : Component
    {
        var transform = self.transform;
        List<T> components = new List<T>();
        foreach (Transform tr in transform)
        {
            components.AddRange(tr.GetComponents<T>());
        }
        return components.ToArray();
    }
}