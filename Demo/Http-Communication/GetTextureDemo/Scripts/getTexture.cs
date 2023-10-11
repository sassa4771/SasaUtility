using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetTexture : MonoBehaviour
{
    private const string URI = "https://star2020.xsrv.jp/app_assessment/Demo/SampleImage.png";

    [SerializeField] private RawImage _image;

    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URI);

        // 画像を取得できるまで待つ
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // 取得した画像のテクスチャをRawImageのテクスチャに張り付ける
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            _image.texture = texture;

            // RawImageの大きさをテクスチャのサイズに合わせる
            _image.rectTransform.sizeDelta = new Vector2(texture.width, texture.height);
        }
    }
}
