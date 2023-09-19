using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if AVPRO_MOVIECAPTURE_WEBCAMTEXTURE_SUPPORT
using RenderHeads.Media.AVProMovieCapture;
#endif

public class csvSmapleManager : CSVManager
{
    public string[] header = { "id", "data1", "data2", "Time" };
    public float SaveTimer = 1.0f;
    private float CountTime = 0;
    private string date;
    private string filename;
    private float GameTimer = 0;

    private int data1 = 0;
    private int data2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        string path = UnityEngine.Application.persistentDataPath;
        SetDataPath(path);
        //開始時刻を取得
        DateTime dt = DateTime.Now;
        date = dt.Year.ToString() + "." + dt.Month.ToString() + "." + dt.Day.ToString() + "_" + dt.Hour.ToString() + "H" + dt.Minute.ToString() + "M" + dt.Second.ToString() + "S";
        filename = "csvSample_" + date;
        SetFileName(filename);
        SetHeader(header);
        Debug.Log("CSVファイルは「" + GetDataPath() + "」に保存されます。");
    }

    void Update()
    {
        CountTime += Time.deltaTime;
        GameTimer += Time.deltaTime;

        if (CountTime > SaveTimer)
        {
            string[] datas;
            try
            {
                datas = new string[] { GetNextId().ToString(), data1.ToString(), data2.ToString(), GameTimer.ToString("f3") };
            }
            catch (System.Exception e)
            {
                datas = new string[] { GetNextId().ToString(), "0", "0", GameTimer.ToString("f3") };
            }

            if (CheckExixtCSV())
            {
                AppendCSV(datas);
                Debug.Log(filename + "を新しく保存しました。");
            }
            else
            {
                OverWriteCSV(datas);
                Debug.Log(filename + "を上書き保存しました。");
            }

            data1++;
            data2 += 2;
            CountTime = 0;
        }
    }

#if AVPRO_MOVIECAPTURE_WEBCAMTEXTURE_SUPPORT
    /// <summary>
    /// アプリ終了時に呼び出される
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");

        Utils.ShowInExplorer(GetDataPath() + @"/");
    }
#endif
}
