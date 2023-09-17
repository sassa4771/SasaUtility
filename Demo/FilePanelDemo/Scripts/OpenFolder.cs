using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; //Input Field用に使う
using System.Windows.Forms; //OpenFileDialog用に使う

public class OpenFolder : MonoBehaviour
{
    public InputField input_field_path_;

    public void OpenExistFile()
    {

        OpenFileDialog open_file_dialog = new OpenFileDialog();

        //ダイアログを開く
        open_file_dialog.ShowDialog();

        //取得したファイル名をstringに代入する
        string file_name = open_file_dialog.FileName;

        Debug.Log(file_name);

    }
}
