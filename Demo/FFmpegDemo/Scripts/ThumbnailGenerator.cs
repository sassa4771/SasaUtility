using UnityEngine;
using System.Diagnostics;
using System.IO;

public class ThumbnailGenerator : MonoBehaviour
{
    // �\�[�X�r�f�I�t�@�C���̃p�X
    public string sourceVideoPath;
    // ��������T���l�C���̃p�X
    public string thumbnailImagePath;
    // �T���l�C���̎��Ԉʒu�i�b�j
    public float thumbnailTime = 5f;

    void Start()
    {
        // ffmpeg.exe�̃p�X
        string ffmpegPath = Application.streamingAssetsPath + "/ffmpeg.exe";

        if (!File.Exists(ffmpegPath))
        {
            UnityEngine.Debug.LogError("StreamingAssets�t�H���_��ffmpeg.exe��z�u���Ă��������B");
            return;
        }


        // �T���l�C�������R�}���h
        string command = $"-ss {thumbnailTime} -i \"{sourceVideoPath}\" -vframes 1 -vf \"thumbnail\" -q:v 2 \"{thumbnailImagePath}\"";

        // �O���v���Z�X�Ƃ���ffmpeg.exe�����s
        Process process = new Process();
        process.StartInfo.FileName = ffmpegPath;
        process.StartInfo.Arguments = command;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        // �v���Z�X�̏I���܂őҋ@
        process.WaitForExit();

        // �v���Z�X�̏I����ɕK�v�ȏ�����ǉ�
        // ��: �T���l�C���̕\����ۑ��̊m�F�Ȃ�
        UnityEngine.Debug.Log("�T���l�C���̐������������܂����B");

        // �v���Z�X�����
        process.Close();
    }
}
