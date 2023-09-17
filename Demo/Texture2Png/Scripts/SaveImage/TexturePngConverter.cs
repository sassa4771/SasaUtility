using System.IO;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using SasaUtility;

/// <summary>
/// テクスチャー ⇔ Png画像 の変換と保存と読み込み
/// </summary>
public class TexturePngConverter : Texture2Png
{
    [SerializeField] protected RawImage _RawImage;
    [SerializeField] private Button _saveButton;
    [SerializeField] protected GameObject ImagePrefab;
    [SerializeField] protected Transform ContainerPrefab;
    protected string SavedPath;
    protected const string IMAGE_SAVE_FOLDER = "Image";
    //[SerializeField] private Button _loadButton;

    private void Start()
    {
        //セーブ & ロード
        _saveButton.OnPointerClickAsObservable().Subscribe(_ => {
            ConvertToPngAndSave(PathController.GetSavePath(IMAGE_SAVE_FOLDER, "png"), _RawImage);
            }).AddTo(this);
    }

}