using UnityEngine;
using System.Diagnostics;
using System.IO;

public class FFmpegExample : MonoBehaviour
{
    // �ϊ����̃r�f�I�t�@�C���p�X
    public string sourceVideoPath;
    // �ϊ���̃r�f�I�t�@�C���p�X
    public string outputVideoPath;

    void Start()
    {
        // ffmpeg.exe�̃p�X
        string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

        if (!File.Exists(ffmpegPath))
        {
            UnityEngine.Debug.LogError("StreamingAssets�t�H���_��ffmpeg.exe��z�u���Ă��������B");
            return;
        }

        // �O���v���Z�X�Ƃ���ffmpeg.exe�����s
        Process process = new Process();
        process.StartInfo.FileName = ffmpegPath;
        process.StartInfo.Arguments = "-i " + sourceVideoPath + " " + outputVideoPath;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        // �v���Z�X�̏I���܂őҋ@
        process.WaitForExit();

        // �v���Z�X�̏I����ɕK�v�ȏ�����ǉ�
        // ��: �������b�Z�[�W�̕\����ϊ���̃r�f�I�̍Đ��Ȃ�
        UnityEngine.Debug.Log("�r�f�I�ϊ����������܂����B");

        // �v���Z�X�����
        process.Close();
    }
}
