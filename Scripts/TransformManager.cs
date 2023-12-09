using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SasaUtility
{
  public class TransformManager
  {
    /// <summary>
    /// 子オブジェクトをすべて取得するメソッド
    /// </summary>
    /// <param name="parentObject">子オブジェクトを取得したい対象の親オブジェクト</param>
    /// <returns></returns>
    public List<GameObject> GetAreaGameObject(GameObject parentObject)
    {
      List<GameObject> AreaChild = new List<GameObject>();

      // 子オブジェクトを全て取得する
      foreach (Transform childTransform in parentObject.transform)
      {
        AreaChild.Add(childTransform.gameObject);
      }

      return AreaChild;
    }

    // 連番のオブジェクトをナンバリングしていくメソッド
    public void NumberingGameObjects(List<GameObject> targetObjects)
    {
      for (int i = 0; i < targetObjects.Count; i++)
      {
        targetObjects[i].name = targetObjects[i].name + "_" + i;
      }
    }
  }
}