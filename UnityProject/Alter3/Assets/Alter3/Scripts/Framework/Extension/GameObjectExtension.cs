using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
/// <summary>
/// GameObject 拡張
/// </summary>
public static class GameObjectExtension
{

    /// <summary>
    /// child検索時に自分を除く
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponentsInChildren<T>().Where(c => gameObject != c.gameObject).ToArray();
    }

    public static GameObject FindInChildren(this GameObject gameObject, string name,bool includeInactive = false) 
    {
		var children = gameObject.GetComponentsInChildren<Transform>(includeInactive);
		foreach (var transform in children)
		{
			if (transform.name == name)
			{
				return transform.gameObject;
			}
		}
		return null;
	}
    /// <summary>
    /// Tagを設定する
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="tagName"></param>
    /// <param name="bChild"></param>
    public static void SetTag(this GameObject gameObject, string tagName, bool bChild = true)
    {
        if (gameObject == null)
        {
            return;
        }


        gameObject.tag = tagName;

        if (!bChild)
        {
            return;
        }

        //子のレイヤーにも設定する
        foreach (Transform childTransform in gameObject.transform)
        {
            SetTag(childTransform.gameObject, tagName, bChild);
        }

    }
    /// <summary>
    /// レイヤーを設定する
    /// </summary>
    /// <param name="bChild">子にもレイヤー設定を行う</param>
    public static void SetLayer(this GameObject gameObject, string layerName, bool bChild = true)
	{
		if (gameObject == null)
		{
			return;
		}

		int layerNo = LayerMask.NameToLayer(layerName);

		gameObject.layer = layerNo;

		if (!bChild)
		{
			return;
		}

		//子のレイヤーにも設定する
		foreach (Transform childTransform in gameObject.transform)
		{
			SetLayerNumber(childTransform.gameObject, layerNo, bChild);
		}
	}
	/// <summary>
	/// レイヤーの設定
	/// </summary>
	/// <param name="gameObject"></param>
	/// <param name="layerNumber"></param>
	/// <param name="bChild"></param>
	public static void SetLayerNumber(this GameObject gameObject, int layerNumber, bool bChild = true)
	{
		if (gameObject == null)
		{
			return;
		}

		gameObject.layer = layerNumber;

		if (!bChild)
		{
			return;
		}

		foreach (Transform childTransform in gameObject.transform)
		{
			SetLayerNumber(childTransform.gameObject, layerNumber, bChild);
		}

	}

	/// <summary>
	/// シャドーの設定をする
	/// </summary>
	/// <param name="gameObject"></param>
	/// <param name="cast">キャスト</param>
	/// <param name="receive">受ける</param>
	public static void SetShadowPrameter(this GameObject gameObject, UnityEngine.Rendering.ShadowCastingMode cast, bool receive)
	{
		if (gameObject == null)
		{
			return;
		}

		{
			var meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach (var mesh in meshRenderers)
			{
				mesh.shadowCastingMode = cast;
				mesh.receiveShadows = receive;
			}
		}

		{
			var meshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (var mesh in meshRenderers)
			{
				mesh.shadowCastingMode = cast;
				mesh.receiveShadows = receive;
			}
		}


	}

    public static GameObject InstantiateWithParentWithScale(this GameObject gameObject, Transform parent, Vector3 positionOffset, Vector3 rotationOffset,Vector3 localScale)
    {
        var obj = GameObject.Instantiate(gameObject, positionOffset, Quaternion.Euler(rotationOffset)) as GameObject;
//        obj.transform.localScale = localScale;
        obj.transform.localScale= localScale;
        obj.transform.SetParent(parent, false);

        return obj;

    }

    public static GameObject InstantiateWithParent(this GameObject gameObject, Transform parent,Vector3 positionOffset, Vector3 rotationOffset)
	{
        var obj = gameObject.InstantiateWithParentWithScale(parent, positionOffset, rotationOffset,Vector3.one);
        return obj;
        /*
        var obj = GameObject.Instantiate(gameObject, positionOffset, Quaternion.Euler(rotationOffset)) as GameObject;
        obj.transform.SetParent(parent, false);

		return obj;
        */
	}
    public static GameObject InstantiateWithParentObject(this GameObject gameObject, Transform parent, Vector3 positionOffset, Vector3 rotationOffset)
    {
        var obj = gameObject.InstantiateWithParent(parent, positionOffset, rotationOffset);
/*
//        var  obj = GameObject.Instantiate(gameObject, positionOffset, Quaternion.Euler(rotationOffset)) as GameObject;
//        obj.transform.SetParent(parent, false);
*/
        return obj;

    }


    /// <summary>
    /// 名前だけで検索する
    /// </summary>
    public static GameObject FindName( this GameObject gameObject, string name,bool isActiveOnly = true)
    {
        var children = gameObject.GetComponentsInChildren<Transform>(!isActiveOnly);
        foreach (var transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }


    private static bool Requires(Type obj, Type requirement)
    {
        //also check for m_Type1 and m_Type2 if required
        return Attribute.IsDefined(obj, typeof(RequireComponent)) &&
               Attribute.GetCustomAttributes(obj, typeof(RequireComponent)).OfType<RequireComponent>()
               .Any(rc => rc.m_Type0.IsAssignableFrom(requirement));
    }

    internal static bool CanDestroy(this GameObject go, Type t)
    {
        return !go.GetComponents<Component>().Any(c => Requires(c.GetType(), t));
    }


}
