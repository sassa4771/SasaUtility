using System;
using System.IO;
using UnityEngine;

namespace SasaUtility
{
    /// <summary>
    /// �t�H���_�[��t�@�C�����쐬���A�p�X���Ǘ����邽�߂̃N���X
    /// </summary>
    public static class PathController
    {
        ///  /// <summary>
        /// �ۑ����Application.persistentDataPath���擾���郁�\�b�h
        /// </summary>
        /// <param name="folderName">��؂�̃t�H���_��</param>
        /// <param name="ExtensionName">�g���q��</param>
        /// <returns>�ۑ���̃p�X</returns>
        public static string GetSavePath(string folderName, string ExtensionName)
        {
            string directoryPath = Application.persistentDataPath + "/" + folderName + "/";

            if (!Directory.Exists(directoryPath))
            {
                //�܂����݂��ĂȂ�������쐬
                Directory.CreateDirectory(directoryPath);
                return directoryPath + GetDateTimeFileName() + "." + ExtensionName;
            }
            string SavedPath = directoryPath + GetDateTimeFileName() + "." + ExtensionName;

            return SavedPath;
        }

        /// <summary>
        /// ���t��t�������j�[�N�ȃt�@�C�����̍쐬
        /// </summary>
        /// <returns>DateTime�̕�����</returns>
        public static string GetDateTimeFileName()
        {
            DateTime TodayNow = DateTime.Now;
            string filename = TodayNow.Year.ToString() + "." + TodayNow.Month.ToString("D2") + "." + TodayNow.Day.ToString("D2") + "_" + TodayNow.Hour.ToString("D2") + "." + TodayNow.Minute.ToString("D2") + "." + TodayNow.Second.ToString("D2");
            return filename;
        }
    }
}