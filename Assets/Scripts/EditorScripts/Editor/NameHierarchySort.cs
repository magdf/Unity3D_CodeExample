//source: http://unity3d.ru/distribution/viewtopic.php?f=105&t=25967

using UnityEditor;
using UnityEngine;

public class NameHierarchySort : BaseHierarchySort
{
    public override int Compare(GameObject lhs, GameObject rhs)
    {
        if (lhs == rhs)
            return 0;
        if (lhs == null)
            return -1;
        if (rhs == null)
            return 1;
        return EditorUtility.NaturalCompare(lhs.name, rhs.name);
    }
}