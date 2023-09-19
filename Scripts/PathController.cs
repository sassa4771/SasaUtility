using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace SasaUtility
{
    /// <summary>
    /// ?t?H???_?[???t?@?C???????????A?p?X?????????????????N???X
    /// </summary>
    public static class PathController
    {
        ///  /// <summary>
        /// ????????Application.persistentDataPath?????????????\?b?h
        /// ?t?H???_??????????????????????????
        /// </summary>
        /// <param name="folderName">?????????t?H???_??</param>
        /// <param name="ExtensionName">?g???q??</param>
        /// <returns>?????????p?X</returns>
        public static string GetSavePath(string folderName, string ExtensionName, bool persistent = true)
        {
            string directoryPath = Application.persistentDataPath + "/" + folderName + "/";
            
            //???S???p?X??????
            if (!persistent) directoryPath = folderName + "/";

                if (!Directory.Exists(directoryPath))
            {
                //??????????????????????????
                Directory.CreateDirectory(directoryPath);
                return directoryPath + GetDateTimeFileName() + "." + ExtensionName;
            }
            string SavedPath = directoryPath + GetDateTimeFileName() + "." + ExtensionName;

            return SavedPath;
        }

        /// <summary>
        /// ???t???t???????j?[?N???t?@?C???????????i?~???Z?J???h?????\???j
        /// ???F2023.05.28_14.08.01.614
        /// </summary>
        /// <returns>DateTime????????</returns>
        public static string GetDateTimeFileName()
        {
            DateTime TodayNow = DateTime.Now;
            string filename = TodayNow.Year.ToString() + "." + TodayNow.Month.ToString("D2") + "." + TodayNow.Day.ToString("D2") + "_" + TodayNow.Hour.ToString("D2") + "." + TodayNow.Minute.ToString("D2") + "." + TodayNow.Second.ToString("D2") + "." + TodayNow.Millisecond.ToString("D2");
            return filename;
        }

        /// <summary>
        /// sourceFolderPath????destinationFolderPath???t?H???_?[?????R?s?[???????\?b?h
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        /// <returns>?????????R?s?[?????p?X?????p</returns>
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
        /// ?I???????g???q???????t?@?C???????????????????\?b?h
        /// ?????V?????t?@?C???????p
        /// </summary>
        /// <param name="sourceFolderPath">?t?H???_?p?X</param>
        /// <param name="type">?g???q</param>
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
        /// ?????g???q???t?@?C?????????????????J?E???g???????\?b?h
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="extension">".mp4"</param>
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
        /// ?t?H???_???V???????????????\?b?h
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

            // ?t?H???_?????????????A???O?????X????
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