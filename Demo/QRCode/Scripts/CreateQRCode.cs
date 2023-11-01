using UnityEngine;
using UnityEditor;
using ZXing;
using ZXing.QrCode;
using System.IO;

#if UNITY_EDITOR
public class CreateQRCode : EditorWindow
{
    [MenuItem("Tools/Create QRCode")]
    static void Init()
    {
        var window = EditorWindow.GetWindow<CreateQRCode>();
        window.Show();
    }

    string _content = "";

    void OnGUI()
    {
        // 保存するQRコードの画像ファイル名
        var path = Application.dataPath + "/QRCode.png";

        // テキストエリアから入力した文字を取得
        _content = GUILayout.TextArea(_content, GUILayout.Height(30f));

        EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(_content));

        if (GUILayout.Button("Save"))
        {
            // QR コードの画像の幅と高さ
            var width = 256;
            var height = 256;

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = width,
                    Height = height
                }
            };

            var format = TextureFormat.ARGB32;
            var texture = new Texture2D(width, height, format, false);
            var colors = writer.Write(_content);

            texture.SetPixels32(colors);
            texture.Apply();

            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var bytes = texture.EncodeToPNG();
                stream.Write(bytes, 0, bytes.Length);
            }

            AssetDatabase.Refresh();
            ShowNotification(new GUIContent("QRコードを作成しました。"));
        }

        EditorGUI.EndDisabledGroup();
    }
}
#endif