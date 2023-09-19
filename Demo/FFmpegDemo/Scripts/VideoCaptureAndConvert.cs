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
            UnityEngine.Debug.LogError("StreamingAssets�t�H���_��ffmpeg.exe��z�u���Ă��������B");
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
                UnityEngine.Debug.LogError("StreamingAssets�t�H���_��ffmpeg.exe��z�u���Ă��������B");
                return;
            }

            StartCoroutine(CaptureAndConvertVideo());
        }
    }

    IEnumerator CaptureAndConvertVideo()
    {
        capturing = true;

        // �t���[�����[�g
        int frameRate = 12;

        // �ϊ�����r�f�I�̒����i�b�j
        float videoLength = 10f;

        // �L���v�`���ƕϊ��̃t���[����
        int totalFrames = (int)(frameRate * videoLength);

        // �ꎞ�I�ȃt���[���ۑ��p�̃f�B���N�g�����쐬
        string tempDirPath = Application.temporaryCachePath + "/temp_frames";
        System.IO.Directory.CreateDirectory(tempDirPath);

        // �L���v�`���ƃt���[���ۑ�
        for (int i = 0; i < totalFrames; i++)
        {
            Texture2D frame = new Texture2D(webCamTexture.width, webCamTexture.height);
            frame.SetPixels(webCamTexture.GetPixels());
            frame.Apply();

            // �t���[�����ꎞ�I��PNG�t�@�C���Ƃ��ĕۑ�
            string tempImagePath = $"{tempDirPath}/frame_{i.ToString("D5")}.jpg";
            System.IO.File.WriteAllBytes(tempImagePath, frame.EncodeToJPG());

            yield return new WaitForEndOfFrame();
        }

        // ffmpeg.exe�̃p�X
        string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

        // �O���v���Z�X�Ƃ���ffmpeg.exe�����s���ăr�f�I�ϊ�
        Process process = new Process();
        process.StartInfo.FileName = ffmpegPath;
        process.StartInfo.Arguments = $"-y -framerate {frameRate} -i \"{tempDirPath}/frame_%05d.jpg\" -c:v libx264 -pix_fmt yuv420p \"{outputVideoPath}\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        // �v���Z�X�̏I���܂őҋ@
        process.WaitForExit();

        // �v���Z�X�̏I����ɕK�v�ȏ�����ǉ�
        // ��: �ϊ��������b�Z�[�W�̕\����ۑ��̊m�F�Ȃ�
        UnityEngine.Debug.Log("�r�f�I�ϊ����������܂����B");

        // �v���Z�X�����
        process.Close();

        // �ꎞ�I�ȃt���[���ۑ��p�̃f�B���N�g�����폜
        System.IO.Directory.Delete(tempDirPath, true);

        capturing = false;
    }
}
