using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace SasaUtility
{
    /// <summary>
    /// フォルダーやファイルを作成し、パスを管理するためのクラス
    /// </summary>
    public static class PathController
    {
        ///  /// <summary>
        /// 保存先のApplication.persistentDataPathを取得するメソッド
        /// フォルダが存在しない場合は作成する
        /// </summary>
        /// <param name="folderName">区切りのフォルダ名</param>
        /// <param name="ExtensionName">拡張子名</param>
        /// <returns>保存先のパス</returns>
        public static string GetSavePath(string folderPath, string ExtensionName)
        {
            string directoryPath = folderPath + "/";

            if (!Directory.Exists(directoryPath))
            {
                //まだ存在してなかったら作成
                Directory.CreateDirectory(directoryPath);
                return directoryPath + GetDateTimeFileName() + "." + ExtensionName;
            }
            string SavedPath = directoryPath + GetDateTimeFileName() + "." + ExtensionName;

            return SavedPath;
        }

        /// <summary>
        /// 日付を付けたユニークなファイル名の作成
        /// 例：2023.05.28_14.08.01
        /// </summary>
        /// <returns>DateTimeの文字列</returns>
        public static string GetDateTimeFileName()
        {
            DateTime TodayNow = DateTime.Now;
            string filename = TodayNow.Year.ToString() + "." + TodayNow.Month.ToString("D2") + "." + TodayNow.Day.ToString("D2") + "_" + TodayNow.Hour.ToString("D2") + "." + TodayNow.Minute.ToString("D2") + "." + TodayNow.Second.ToString("D2") + "." + TodayNow.Millisecond.ToString("D2");
            return filename;
        }

        /// <summary>
        /// sourceFolderPathからdestinationFolderPathにフォルダーごとコピーするメソッド
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        /// <returns>成功時はコピー先のパスを返却</returns>
        public static string CopyDirectory(string sourceFolderPath, string destinationFolderPath)
        {
            string newFolderPath = CreateDirectory(sourceFolderPath, destinationFolderPath);

            string[] files = Directory.GetFiles(sourceFolderPath);
            Debug.Log(files[0]);

            foreach (string file in files)
            {
                Debug.Log(file);
                string fileName = Path.GetFileName(file);
                string newFilePath = Path.Combine(newFolderPath, fileName);
                File.Copy(file, newFilePath, true);
            }

            return newFolderPath;
        }

        public static string CopyOneFile(string sourceFolderPath, string destinationFolderPath, string type)
        {
            string[] files = Directory.GetFiles(sourceFolderPath);
            Debug.Log(files[0]);

            foreach (string file in files)
            {
                string extension = System.IO.Path.GetExtension(file);

                string getfile = GetOneFilePath(sourceFolderPath, type);
                if (getfile != null)
                {
                    Debug.Log(getfile);
                    string fileName = Path.GetFileName(file);
                    string newFilePath = Path.Combine(destinationFolderPath, fileName);
                    File.Copy(getfile, newFilePath, true);

                    return newFilePath;
                }
            }

            return null;
        }

        /// <summary>
        /// 選択した拡張子と同じファイルを一つ取得するメソッド
        /// </summary>
        /// <param name="sourceFolderPath">フォルダパス</param>
        /// <param name="type">拡張子</param>
        /// <returns></returns>
        public static string GetOneFilePath(string sourceFolderPath, string extension)
        {
            string[] files = Directory.GetFiles(sourceFolderPath);
            List<string> videoFilePath = new List<string>();

            foreach (string file in files)
            {
                string type = System.IO.Path.GetExtension(file);
                if (type == extension)
                {
                    Debug.Log(file);
                    string fileName = Path.GetFileName(file);
                    string newFilePath = Path.Combine(sourceFolderPath, fileName);
                    videoFilePath.Add(newFilePath);
                }
            }

            if (videoFilePath.Count > 0) return videoFilePath[videoFilePath.Count - 1];

            return null;
        }

        /// <summary>
        /// 選択した拡張子と同じファイルを一つ取得するメソッド
        /// </summary>
        /// <param name="sourceFolderPath">フォルダパス</param>
        /// <param name="type">拡張子</param>
        /// <returns></returns>
        public static int CountFileExtentino(string sourceFolderPath, string extension)
        {
            string[] files = Directory.GetFiles(sourceFolderPath);
            List<string> videoFilePath = new List<string>();

            foreach (string file in files)
            {
                string type = System.IO.Path.GetExtension(file);
                if (type == extension)
                {
                    Debug.Log(file);
                    string fileName = Path.GetFileName(file);
                    string newFilePath = Path.Combine(sourceFolderPath, fileName);
                    videoFilePath.Add(newFilePath);
                }
            }

            return videoFilePath.Count;
        }

        /// <summary>
        /// フォルダを新しく作成するメソッド
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        /// <returns></returns>
        public static string CreateDirectory(string sourceFolderPath, string destinationFolderPath)
        {
            if (!Directory.Exists(sourceFolderPath))
            {
                Debug.LogError("Source folder does not exist!");
                return null;
            }

            //Debug.Log(sourceFolderPath);
            string newFolderPath = destinationFolderPath + "/" + Path.GetFileName(sourceFolderPath);

            int count = 1;
            string tempPath = newFolderPath;

            // フォルダがある場合は、名前を変更する
            while (Directory.Exists(newFolderPath))
            {
                newFolderPath = tempPath + " (" + count + ")";
                count++;
            }

            Directory.CreateDirectory(newFolderPath);
            return newFolderPath;
        }
    }
}