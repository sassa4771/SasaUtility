using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System;
#if AVPRO_MOVIECAPTURE_WEBCAMTEXTURE_SUPPORT
using RenderHeads.Media.AVProMovieCapture;
#endif

namespace SasaUtility.Demo.Original
{
    public class FileWriteExample : MonoBehaviour
    {
        private async void Start()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "example.txt");

            string[] lines = {
            "Line 1",
            "Line 2",
            "Line 3"
        };

            await WriteToFileAsync(filePath, lines);
        }

        public async Task WriteToFileAsync(string filePath, string[] lines)
        {
            try
            {
                await File.WriteAllLinesAsync(filePath, lines);
                Debug.Log("File writing completed successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }

#if AVPRO_MOVIECAPTURE_WEBCAMTEXTURE_SUPPORT
    /// <summary>
    /// アプリ終了時に呼び出される
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");

        string path = UnityEngine.Application.persistentDataPath;
        Utils.ShowInExplorer(path + @"/");
    }
#endif
    }
}