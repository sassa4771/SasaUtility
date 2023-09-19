using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace SasaUtility.Demo.Original
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
                UnityEngine.Debug.LogError("StreamingAssets?t?H???_??ffmpeg.exe???z?u?????????????B");
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
                UnityEngine.Debug.LogError("StreamingAssets?t?H???_??ffmpeg.exe???z?u?????????????B");
                return;
            }

            if (!File.Exists(inputVideoPath)) UnityEngine.Debug.LogError("?I????????????????????????");

            // Start the coroutine to cut the video
            StartCoroutine(CutVideo());
        }

        /// <summary>
        /// ?J?n???I?????t???[?????I???????A???????????o?????\?b?h
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

            //?A?E?g?v?b?g???p?X????
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