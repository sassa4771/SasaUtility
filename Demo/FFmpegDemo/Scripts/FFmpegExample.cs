using UnityEngine;
using System.Diagnostics;
using System.IO;

public class FFmpegExample : MonoBehaviour
{
    // 変換元のビデオファイルパス
    public string sourceVideoPath;
    // 変換後のビデオファイルパス
    public string outputVideoPath;

    void Start()
    {
        // ffmpeg.exeのパス
        string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

        if (!File.Exists(ffmpegPath))
        {
            UnityEngine.Debug.LogError("StreamingAssetsフォルダにffmpeg.exeを配置してください。");
            return;
        }

        // 外部プロセスとしてffmpeg.exeを実行
        Process process = new Process();
        process.StartInfo.FileName = ffmpegPath;
        process.StartInfo.Arguments = "-i " + sourceVideoPath + " " + outputVideoPath;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        // プロセスの終了まで待機
        process.WaitForExit();

        // プロセスの終了後に必要な処理を追加
        // 例: 完了メッセージの表示や変換後のビデオの再生など
        UnityEngine.Debug.Log("ビデオ変換が完了しました。");

        // プロセスを解放
        process.Close();
    }
}
