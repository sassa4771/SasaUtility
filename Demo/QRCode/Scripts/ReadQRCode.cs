using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;
using ZXing;

public class ReadQRCode : MonoBehaviour
{
    private const string PERMISSION = Permission.Camera;

    public TextMeshProUGUI m_text;
    public RawImage m_rawImage;

    private WebCamTexture m_webCamTexture;

    private void Awake()
    {
        // カメラの使用許可リクエスト
        Permission.RequestUserPermission(PERMISSION);
    }

    private void Update()
    {
        // カメラの準備が出来ていない場合
        if (m_webCamTexture == null)
        {
            // カメラの使用が許可された場合
            if (Permission.HasUserAuthorizedPermission(PERMISSION))
            {
                var width = Screen.width;
                var height = Screen.height;

                m_webCamTexture = new WebCamTexture(width, height);

                // カメラの使用を開始
                m_webCamTexture.Play();

                // カメラが写している画像をゲーム画面に表示
                m_rawImage.texture = m_webCamTexture;

#if UNITY_IOS
                // カメラの映像を90度回転させる
                m_rawImage.transform.localEulerAngles = new Vector3(0, -180, 180);
#endif
            }
        }
        else
        {
            // カメラが写している QRコードからデータを取得し、ゲーム画面に表示
            m_text.text = Read(m_webCamTexture);
        }
    }

    private static string Read(WebCamTexture texture)
    {
        var reader = new BarcodeReader();
        var rawRGB = texture.GetPixels32();
        var width = texture.width;
        var height = texture.height;
        var result = reader.Decode(rawRGB, width, height);

        return result != null ? result.Text : "No Data!";
    }
}