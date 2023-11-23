using System.IO;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SasaUtility.Demo.Original
{
    /// <summary>
    /// テクスチャー ⇔ Png画像 の変換と保存と読み込み
    /// </summary>
    public class TexturePngConverter : MonoBehaviour
    {
        [SerializeField] protected RawImage _RawImage;
        [SerializeField] private Button _saveButton;
        protected string SavedPath;
        protected const string IMAGE_SAVE_FOLDER = "Image";
        //[SerializeField] private Button _loadButton;

        private void Start()
        {
            //セーブ & ロード
            _saveButton.OnPointerClickAsObservable().Subscribe(_ =>
            {
                Texture2Png.ConvertToPngAndSave(PathController.GetSavePath(Application.persistentDataPath + "/" + IMAGE_SAVE_FOLDER, "png"), _RawImage);
            }).AddTo(this);
        }

    }
}