using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.IO;

public class VideoCaptureAndConvert : MonoBehaviour
{
    public int webcamIndex = 0;
    public string outputVideoFileName = "output.mp4";
    public string outputVideoPath;

    private WebCamTexture webCamTexture;
    private bool capturing = false;

    void Start()
    {
        string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

        if (!File.Exists(ffmpegPath))
        {
            UnityEngine.Debug.LogError("StreamingAssetsフォルダにffmpeg.exeを配置してください。");
            return;
        }

        outputVideoPath = Application.streamingAssetsPath + "/" + outputVideoFileName;

        UnityEngine.Debug.Log(outputVideoPath);

        WebCamDevice[] devices = WebCamTexture.devices;
        if (webcamIndex < devices.Length)
        {
            webCamTexture = new WebCamTexture(devices[webcamIndex].name);
            webCamTexture.Play();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !capturing)
        {
            string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

            if (!File.Exists(ffmpegPath))
            {
                UnityEngine.Debug.LogError("StreamingAssetsフォルダにffmpeg.exeを配置してください。");
                return;
            }

            StartCoroutine(CaptureAndConvertVideo());
        }
    }

    IEnumerator CaptureAndConvertVideo()
    {
        capturing = true;

        // フレームレート
        int frameRate = 12;

        // 変換するビデオの長さ（秒）
        float videoLength = 10f;

        // キャプチャと変換のフレーム数
        int totalFrames = (int)(frameRate * videoLength);

        // 一時的なフレーム保存用のディレクトリを作成
        string tempDirPath = Application.temporaryCachePath + "/temp_frames";
        System.IO.Directory.CreateDirectory(tempDirPath);

        // キャプチャとフレーム保存
        for (int i = 0; i < totalFrames; i++)
        {
            Texture2D frame = new Texture2D(webCamTexture.width, webCamTexture.height);
            frame.SetPixels(webCamTexture.GetPixels());
            frame.Apply();

            // フレームを一時的なPNGファイルとして保存
            string tempImagePath = $"{tempDirPath}/frame_{i.ToString("D5")}.jpg";
            System.IO.File.WriteAllBytes(tempImagePath, frame.EncodeToJPG());

            yield return new WaitForEndOfFrame();
        }

        // ffmpeg.exeのパス
        string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

        // 外部プロセスとしてffmpeg.exeを実行してビデオ変換
        Process process = new Process();
        process.StartInfo.FileName = ffmpegPath;
        process.StartInfo.Arguments = $"-y -framerate {frameRate} -i \"{tempDirPath}/frame_%05d.jpg\" -c:v libx264 -pix_fmt yuv420p \"{outputVideoPath}\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        // プロセスの終了まで待機
        process.WaitForExit();

        // プロセスの終了後に必要な処理を追加
        // 例: 変換完了メッセージの表示や保存の確認など
        UnityEngine.Debug.Log("ビデオ変換が完了しました。");

        // プロセスを解放
        process.Close();

        // 一時的なフレーム保存用のディレクトリを削除
        System.IO.Directory.Delete(tempDirPath, true);

        capturing = false;
    }
}
