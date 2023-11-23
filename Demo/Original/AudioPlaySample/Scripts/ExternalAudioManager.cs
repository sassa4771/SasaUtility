using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using UnityEditor;

namespace AudioPlaySample
{
    public class ExternalAudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public List<string> AudioFileNames;

        private struct audioInfo
        {
            public string AudioPath;
            public bool play;
            public UnityAction action;
            public int FileNumber;
        }

        private void Awake()
        {
            string demoAudioPaht = "Assets/Plugins/SasaUtility/Demo/Original/AudioPlaySample/Audio/";
            string destinationFolderPath = UnityEngine.Application.persistentDataPath + "/Audio/";
            CopyFilesInFolder(demoAudioPaht, destinationFolderPath);
        }

        void Start()
        {
            //パーマネントディレクトリの場所
            Debug.Log("persistentDataPath：" + UnityEngine.Application.persistentDataPath + "の音声を再生します。");
        }

        public void GetAudio(int FileNumber, UnityAction action, bool play = true)
        {
            //AudioPathの設定
            string AudioPath = UnityEngine.Application.persistentDataPath + "/Audio/" + AudioFileNames[FileNumber];
            Debug.Log("音声ファイルのパス:" + AudioPath);

            audioInfo info = new audioInfo();
            info.AudioPath = AudioPath;
            info.play = play;
            info.action = action;
            info.FileNumber = FileNumber;

            StartCoroutine("GetAudioFromExternalFile", info);
        }

        /// <summary>
        /// オーディオを外部ファイルから取得して、オーディオクリップにセットし、再生する
        /// </summary>
        /// <param name="FileNumber">読み込んだAudioFileNamesの何番目の要素か選択する</param>
        /// <param name="play">再生するか否か</param>
        /// <returns></returns>
        private IEnumerator GetAudioFromExternalFile(audioInfo info)
        {

            // ファイルの有無を確認
            if (!System.IO.File.Exists(info.AudioPath))
            {
                Debug.LogError("File does NOT exist!! file path = " + info.AudioPath);
                yield break;
            }
            else
            {
                Debug.Log("ファイルは存在します。");
            }

            if (audioSource == null) yield break;

            //Get AudioFile through WWW
            using (WWW www = new WWW("file://" + info.AudioPath))
            {
                //ファイルの読み込み
                while (!www.isDone)
                    yield return null;

                AudioClip audioClip = www.GetAudioClip(false, true);
                if (audioClip.loadState != AudioDataLoadState.Loaded)
                {
                    //ここにロード失敗処理
                    Debug.Log("Failed to load AudioClip.");
                    yield break;
                }

                Debug.Log("Load Succcess : " + info.AudioPath);
                audioSource.clip = audioClip;
                if (info.play)
                {
                    audioSource.Play();
                    Debug.Log("再生時間 : " + audioSource.clip.length);
                    yield return new WaitForSeconds(audioSource.clip.length + 1.0f);
                    if(AudioFileNames.Count-1 > info.FileNumber)info.action?.Invoke();
                }
            }
        }

        /// <summary>
        /// デモ用に外部へ音声ファイルをコピーするメソッド
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        public void CopyFilesInFolder(string sourceFolderPath, string destinationFolderPath)
        {
            if (!Directory.Exists(sourceFolderPath))
            {
                Debug.LogError("コピー元のフォルダが見つかりません");
                return;
            }

            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath); // コピー先のフォルダが存在しない場合は作成する
                Debug.Log("新しくフォルダを作成しました。:" + destinationFolderPath);
            }

            string[] files = Directory.GetFiles(sourceFolderPath);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                if (File.Exists(destinationFilePath))
                {
                    Debug.LogWarning("同じ名前のファイルが既に存在します: " + fileName);
                    continue;
                }

                File.Copy(file, destinationFilePath);
                Debug.Log("ファイルがコピーされました: " + destinationFilePath);
            }

            Debug.Log("フォルダ内のファイルをすべてコピーしました");
        }
    }
}