using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace SasaUtility.Demo
{
    public class ExportTrimVideo : MonoBehaviour
    {
        private string ffmpegPath;
        public string inputVideoPath = "path_to_your_input_video"; // Update this with your video's path
        public string outputVideoFolderPath = "path_to_your_input_video";
        private long startFrame = 5;
        private long endFrame = 50;
        private float frameRate = 30; // Update this with your video's framerate if different


        // Start is called before the first frame update
        void Start()
        {
            // Get the path to ffmpeg.exe
            ffmpegPath = Path.Combine(Application.streamingAssetsPath, "ffmpeg.exe");

            if (!File.Exists(ffmpegPath))
            {
                UnityEngine.Debug.LogError("StreamingAssetsフォルダにffmpeg.exeを配置してください。");
                return;
            }
        }

        public void StartCutVideo(string inputVideoPath, string outputVideoFolderPath, long startFrame, long endFrame, float frameRate)
        {
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.frameRate = frameRate;
            this.inputVideoPath = inputVideoPath;
            this.outputVideoFolderPath = outputVideoFolderPath;

            // Get the path to ffmpeg.exe
            ffmpegPath = Path.Combine(Application.streamingAssetsPath, "ffmpeg.exe");

            if (!File.Exists(ffmpegPath))
            {
                UnityEngine.Debug.LogError("StreamingAssetsフォルダにffmpeg.exeを配置してください。");
                return;
            }

            if (!File.Exists(inputVideoPath)) UnityEngine.Debug.LogError("選択した動画は存在しません");

            // Start the coroutine to cut the video
            StartCoroutine(CutVideo());
        }

        /// <summary>
        /// 開始と終了のフレームを選択して、動画を書き出すメソッド
        /// </summary>
        /// <param name="startFrame"></param>
        /// <param name="endFrame"></param>
        /// <param name="frameRate"></param>
        private IEnumerator CutVideo()
        {
            // Convert frames to time (seconds)
            float startTime = startFrame / (float)frameRate;
            float endTime = endFrame / (float)frameRate;
            float duration = endTime - startTime;

            //アウトプットのパス生成
            string outputVideoPath = Path.Combine(outputVideoFolderPath, "output" + startFrame.ToString() + "to" + endFrame.ToString() + ".mp4");

            // Create the command to cut the video
            string args = $"-ss {startTime} -i {inputVideoPath} -t {duration} -c copy {outputVideoPath}";

            // Start a new process to run the command
            ProcessStartInfo startInfo = new ProcessStartInfo(ffmpegPath, args);
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();

            // Wait for the process to finish
            while (!proc.HasExited)
            {
                yield return null;
            }

            System.Diagnostics.Process.Start(outputVideoFolderPath);
        }
    }
}