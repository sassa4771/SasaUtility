using UnityEngine;
using System.Diagnostics;
using System.IO;

public class ThumbnailGenerator : MonoBehaviour
{
    // ソースビデオファイルのパス
    public string sourceVideoPath;
    // 生成するサムネイルのパス
    public string thumbnailImagePath;
    // サムネイルの時間位置（秒）
    public float thumbnailTime = 5f;

    void Start()
    {
        // ffmpeg.exeのパス
        string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

        if (!File.Exists(ffmpegPath))
        {
            UnityEngine.Debug.LogError("StreamingAssetsフォルダにffmpeg.exeを配置してください。");
            return;
        }


        // サムネイル生成コマンド
        string command = $"-ss {thumbnailTime} -i \"{sourceVideoPath}\" -vframes 1 -vf \"thumbnail\" -q:v 2 \"{thumbnailImagePath}\"";

        // 外部プロセスとしてffmpeg.exeを実行
        Process process = new Process();
        process.StartInfo.FileName = ffmpegPath;
        process.StartInfo.Arguments = command;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        // プロセスの終了まで待機
        process.WaitForExit();

        // プロセスの終了後に必要な処理を追加
        // 例: サムネイルの表示や保存の確認など
        UnityEngine.Debug.Log("サムネイルの生成が完了しました。");

        // プロセスを解放
        process.Close();
    }
}
