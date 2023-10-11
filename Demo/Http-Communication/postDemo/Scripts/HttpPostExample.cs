using UnityEngine;
using System.Collections.Generic;

public class HttpPostExample : MonoBehaviour
{
    string RequestURL = "http://star2020.xsrv.jp/app_assessment/Demo/postDemo.php";
    void Start()
    {
        List<PostData> postDatas = new List<PostData>();
        postDatas.Add(new PostData("param1", 1));
        postDatas.Add(new PostData("param2", 2));

        HttpManager http = new HttpManager();
        StartCoroutine(http.PostData(postDatas, RequestURL, ResponseDebug));
    }

    public void ResponseDebug(string response)
    {
        HttpManager http = new HttpManager();
        Dictionary<string, string> responseData = http.convertJSON2Dictionary(response);

        foreach (KeyValuePair<string, string> kvp in responseData)
        {
            string key = kvp.Key;
            string value = kvp.Value;

            // データの処理
            Debug.Log("Key: " + key + ", Value: " + value);
        }
    }

}

/*
 * phpプログラム（postDemo.php）
<?php
// POSTデータを受け取る
$param1 = $_POST['param1'];
$param2 = $_POST['param2'];

// 受け取ったパラメータを表示
echo "param1: " . $param1 . "<br>";
echo "param2: " . $param2;
?>
 */