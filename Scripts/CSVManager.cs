using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
//using Cysharp.Threading.Tasks;
using System.Threading.Tasks;


namespace SasaUtility
{
    public class CSVManager : MonoBehaviour
    {
        //呼び出す順番
        //0.SetDataPath()
        //1.SetFileName()
        //2.SetHeader()
        //3.CheckExixtCSV()
        //4.GetNextId()
        //5.OverWriteCSV()
        //6.AppendCSV()
        //7.ReadCSV()

        private List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト
        private string[] header = null;       //ヘッダーの中身を入れるリスト
        private string FileName = null;
        private string path = null;

        private void Awake()
        {
            path = UnityEngine.Application.persistentDataPath;
        }

        /// <summary>
        /// 保存先のパスを出力する
        /// </summary>
        /// <returns></returns>
        public string GetDataPath()
        {
            return path;
        }

        /// <summary>
        /// データパスを任意の場所にする場合に呼び出すメソッド
        /// </summary>
        /// <param name="datapath"></param>
        public void SetDataPath(string datapath)
        {
            path = datapath;
        }

        /// <summary>
        /// csvのファイル名を設定するメソッド
        /// </summary>
        /// <param name="filename"></param>
        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        /// <summary>
        /// csvのヘッダーを設定するメソッド
        /// </summary>
        /// <param name="header"></param>
        public void SetHeader(string[] header)
        {
            this.header = header;
        }

        /// <summary>
        /// 指定した名前のcsvが存在するかをチェックするメソッド
        /// </summary>
        /// <returns></returns>
        public bool CheckExixtCSV()
        {
            if (System.IO.File.Exists(path + @"/" + FileName + ".csv"))
            {
                //Debug.Log(FileName + "という名前のCSVファイルは存在します");
                return true;
            }
            else
            {
                Debug.Log(FileName + "という名前のCSVファイルは存在しません");
                return false;
            }
        }

        /// <summary>
        /// csvに書き込むidの次の番号を取得するメソッド
        /// </summary>
        /// <returns></returns>
        public int GetNextId()
        {
            if (System.IO.File.Exists(path + @"/" + FileName + ".csv"))
            {
                StreamReader reader = new StreamReader(path + @"/" + FileName + ".csv", System.Text.Encoding.UTF8);

                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    csvDatas.Add(line.Split(','));
                }

                reader.Close();

                return int.Parse(csvDatas[csvDatas.Count() - 1][0]) + 1;

            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// csvを上書きするメソッド
        /// </summary>
        /// <param name="data"></param>
        public bool OverWriteCSV(string[] datas)
        {
            StreamWriter sw = new StreamWriter(path + @"/" + FileName + ".csv", false, System.Text.Encoding.UTF8, bufferSize: 10000);

            // ヘッダー出力
            if (header != null)
            {
                string header_with_comma = string.Join(",", header);
                sw.WriteLine(header_with_comma);
            }
            else
            {
                //ヘッダーがnullの場合の処理
                Debug.LogError("ヘッダーに値が入っていません。（nullです。）");
                return false;
            }

            // データ出力
            if (datas != null)
            {
                string datas_with_comma = string.Join(",", datas);
                sw.WriteLine(datas_with_comma);
            }
            else
            {
                //データがnullの場合の処理
                Debug.LogError("データに値が入っていません。（nullです。）");
                return false;
            }

            //StreamWriterを閉じる
            sw.Close();

            return true;
        }

        /// <summary>
        /// CSVを上書きする非同期メソッド
        /// </summary>
        /// <param name="data"></param>
        //public async UniTask<bool> OverWriteCSVAsync(string[] datas)
        //{
        //    StreamWriter sw = new StreamWriter(path + @"/" + FileName + ".csv", false, System.Text.Encoding.UTF8, bufferSize: 10000);

        //    // ヘッダー出力
        //    if (header != null)
        //    {
        //        string header_with_comma = string.Join(",", header);
        //        await sw.WriteLineAsync(header_with_comma);
        //    }
        //    else
        //    {
        //        //ヘッダーがnullの場合の処理
        //        Debug.LogError("ヘッダーに値が入っていません。（nullです。）");
        //        sw.Close();
        //        return false;
        //    }

        //    // データ出力
        //    if (datas != null)
        //    {
        //        string datas_with_comma = string.Join(",", datas);
        //        await sw.WriteLineAsync(datas_with_comma);
        //    }
        //    else
        //    {
        //        //データがnullの場合の処理
        //        Debug.LogError("データに値が入っていません。（nullです。）");
        //        sw.Close();
        //        return false;
        //    }

        //    // StreamWriterを閉じる
        //    sw.Close();

        //    return true;
        //}

        /// <summary>
        /// WriteAllLinesAsyncを使用した書き込み方法
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public async Task WriteToFileAsync(string filePath, string[] lines)
        {
            try
            {
                await File.WriteAllLinesAsync(filePath, lines);
                Debug.Log("File writing completed successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// CSVにデータを追加するメソッド
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool AppendCSV(string[] datas)
        {
            StreamWriter sw = new StreamWriter(path + @"/" + FileName + ".csv", true, System.Text.Encoding.UTF8);

            // データ出力
            if (datas != null)
            {
                string datas_with_comma = string.Join(",", datas);
                sw.WriteLine(datas_with_comma);
            }
            else
            {
                //データがnullの場合の処理
                Debug.LogError("データに値が入っていません。（nullです。）");
                return false;
            }

            //StreamWriterを閉じる
            sw.Close();

            return true;
        }

        /// <summary>
        /// CSVを追加する非同期メソッド
        /// </summary>
        /// <param name="data"></param>
        //public async UniTask<bool> AppendCSVAsync(string[] datas)
        //{
        //    StreamWriter sw = new StreamWriter(path + @"/" + FileName + ".csv", true, System.Text.Encoding.UTF8, bufferSize: 10000);

        //    // データ出力
        //    if (datas != null)
        //    {
        //        string datas_with_comma = string.Join(",", datas);
        //        await sw.WriteLineAsync(datas_with_comma);
        //    }
        //    else
        //    {
        //        //データがnullの場合の処理
        //        Debug.LogError("データに値が入っていません。（nullです。）");
        //        return false;
        //    }

        //    //StreamWriterを閉じる
        //    sw.Close();

        //    return true;
        //}

        /// <summary>
        /// CSVのデータを読み取ってListにstring[]を入れて返す
        /// </summary>
        /// <returns></returns>
        public List<string[]> ReadCSV()
        {
            if (FileName == null)
            {
                Debug.LogError("ファイル名(FileNmae)が設定されていません。");
                return null;
            }

            if (System.IO.File.Exists(path + @"/" + FileName + ".csv"))
            {
                StreamReader reader = new StreamReader(path + @"/" + FileName + ".csv", System.Text.Encoding.UTF8);

                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    csvDatas.Add(line.Split(','));
                }

                reader.Close();

                return csvDatas;
            }
            else
            {
                return null;
            }
        }
    }
}