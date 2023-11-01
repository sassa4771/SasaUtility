using UnityEngine;
using System.IO;
using SasaUtility;

namespace SasaUtility.Demo
{
    public class ScreenshotManager : MonoBehaviour
    {
        public Camera targetCamera; // カメラオブジェクトを指定する変数

        void Update()
        {
            // スペースキーが押された時にCameraオブジェクトから映像を取得
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CaptureScreenshot();
            }
        }

        [ContextMenu("CaptureScreenshot")]
        void CaptureScreenshot()
        {
            // カメラのスクリーンショットを撮影する
            RenderTexture renderTexture = targetCamera.targetTexture;
            print($"Camera Name: {targetCamera.name}, targetTexture: {targetCamera.targetTexture.name}");
            print($"width: {renderTexture.width}, height: {renderTexture.height}");

            Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;
            screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            //targetCamera.targetTexture = null;
            RenderTexture.active = null;

            // フォルダを作成する
            string dirName = Path.Combine(Application.dataPath, "ScreenShot");
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
                Debug.Log("Success to create ScreenShot directory!");
            }

            // スクリーンショットをPNGファイルとして保存する
            byte[] bytes = screenshot.EncodeToPNG();
            string filePath = Path.Combine(dirName, PathController.GetDateTimeFileName() + "_Image.png"); // 保存先のファイルパス
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Screenshot saved to: " + filePath);
            System.Diagnostics.Process.Start(filePath);
        }
    }
}