using System;
using System.IO;
using UnityEngine;

namespace SasaUtility
{
    /// <summary>
    /// フォルダーやファイルを作成し、パスを管理するためのクラス
    /// </summary>
    public static class PathController
    {
        ///  /// <summary>
        /// 保存先のApplication.persistentDataPathを取得するメソッド
        /// </summary>
        /// <param name="folderName">区切りのフォルダ名</param>
        /// <param name="ExtensionName">拡張子名</param>
        /// <returns>保存先のパス</returns>
        public static string GetSavePath(string folderName, string ExtensionName)
        {
            string directoryPath = Application.persistentDataPath + "/" + folderName + "/";

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
        /// </summary>
        /// <returns>DateTimeの文字列</returns>
        public static string GetDateTimeFileName()
        {
            DateTime TodayNow = DateTime.Now;
            string filename = TodayNow.Year.ToString() + "." + TodayNow.Month.ToString("D2") + "." + TodayNow.Day.ToString("D2") + "_" + TodayNow.Hour.ToString("D2") + "." + TodayNow.Minute.ToString("D2") + "." + TodayNow.Second.ToString("D2");
            return filename;
        }
    }
}