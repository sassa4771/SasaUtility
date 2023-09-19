using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine;
using Newtonsoft.Json;

public class HttpManager
{
    public IEnumerator PostData(List<PostData> postDatas, string RequestURL, UnityAction<string> action = null)
    {
        // 送信するデータを作成
        UnityEngine.WWWForm form = new UnityEngine.WWWForm();
        foreach(var data in postDatas)
        {
            form.AddField(data.ColumnName, data.InsertData.ToString());
        }

        // POSTリクエストを作成
        UnityWebRequest www = UnityWebRequest.Post(RequestURL, form);

        // リクエストを送信し、レスポンスを待機
        yield return www.SendWebRequest();


        // レスポンスの処理
        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.Log("エラー: " + www.error);
        }
        else
        {
            UnityEngine.Debug.Log("成功: " + www.downloadHandler.text);
            if (action != null) action.Invoke(www.downloadHandler.text);
        }
    }

    public Dictionary<string, string> convertJSON2Dictionary(string JsonText)
    {

        Dictionary<string, string> responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonText);
        return responseData;
    }
}

public class PostData
{
    public string ColumnName { get; set; }
    public object InsertData { get; set; }

    public PostData(string columnName, object insertData)
    {
        ColumnName = columnName;
        InsertData = insertData;
    }
}