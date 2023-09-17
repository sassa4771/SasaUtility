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

        /// <summary>
        /// sourceFolderPath����destinationFolderPath�Ƀt�H���_�[���ƃR�s�[���郁�\�b�h
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        /// <returns>�������̓R�s�[��̃p�X��ԋp</returns>
        public static string CopyDirectory(string sourceFolderPath, string destinationFolderPath)
        {
            if (!Directory.Exists(sourceFolderPath))
            {
                Debug.LogError("Source folder does not exist!");
                return null;
            }

            Debug.Log(sourceFolderPath);
            string newFolderPath = destinationFolderPath + "/" + Path.GetFileName(sourceFolderPath);
            
            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
            }

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
    }
}