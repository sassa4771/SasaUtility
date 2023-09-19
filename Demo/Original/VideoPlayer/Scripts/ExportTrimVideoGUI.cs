using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SasaUtility.Demo.Original
{
    public class ExportTrimVideoGUI : MonoBehaviour
    {
        VideoPlayerManager vpm;
        ExportTrimVideo exportTrim;

        // Start is called before the first frame update
        void Start()
        {
            vpm = GetComponent<VideoPlayerManager>();
            exportTrim = GetComponent<ExportTrimVideo>();

            Debug.Log(vpm.vp.url);
        }

        public void OnClickExportTrimVideo()
        {
            string outputVideoFolderPath = Application.persistentDataPath;
            exportTrim.StartCutVideo(vpm.vp.url, outputVideoFolderPath, vpm.startFrame, vpm.endFrame, vpm.vp.frameRate);
        }

    }
}