using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensions
{
    /// <summary>
    /// 名前のみで検索する
    /// </summary>
    public static GameObject FindName(this Component componet,string name, bool isActiveOnly = true)
    {
        var children = componet.GetComponentsInChildren<Transform>(!isActiveOnly);
        foreach (var transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }
}
