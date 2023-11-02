using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ImageSample
{
    public class ExternalImageManager
    {
        private GameObject ParentObject;
        private string ImagePath;
        private float width = 600;
        private float height = 600;

        void Start()
        {
            //パーマネントディレクトリの場所
            Debug.Log("persistentDataPath：" + UnityEngine.Application.persistentDataPath + "の画像を表示します。");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="FolderName">フォルダー名</param>
        /// <param name="ImageFileName">ファイル名（拡張子含む）</param>
        /// <param name="ParentObject">親オブジェクト</param>
        /// <param name="width">画像の幅</param>
        /// <param name="height">画像の高さ</param>
        public ExternalImageManager(string FolderName, string ImageFileName, GameObject ParentObject, float width = 600, float height = 600)
        {
            this.ParentObject = ParentObject;
            this.width = width;
            this.height = height;

            //ImagePathの設定
            this.ImagePath = UnityEngine.Application.persistentDataPath + "/" + FolderName + "/" + ImageFileName;
        }

        public void InstanceImageObject()
        {
            //新しくGameObjectを作成
            GameObject ImageObject = CreateUIImage(ParentObject);
            Image image = ImageObject.GetComponent<Image>();

            //外部フォルダから画像ファイルを取得
            image.sprite = readImageFile2Sprite(ImagePath);
            //画像の大きさを本来の大きさにする
            image.SetNativeSize();
            image.rectTransform.sizeDelta = new Vector2(width, height);

        }

        //外部ファイルの画像をbynaryデータで読み取り、Texture2Dを返す
        private Texture2D readImageFile2Texture2D(string filePath)
        {
            byte[] byteData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            texture.LoadImage(byteData);

            return texture;
        }

        private Sprite readImageFile2Sprite(string filePath)
        {
            Texture2D texture = readImageFile2Texture2D(filePath);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        private GameObject CreateUIImage(GameObject ParentObject)
        {

            GameObject CreateObject = new GameObject("CreateObject");

            // 親子関係
            CreateObject.transform.SetParent(ParentObject.transform, false);

            // 並べ替え
            CreateObject.transform.SetSiblingIndex(ParentObject.transform.GetSiblingIndex());

            // 位置・回転・スケール・アンカーなど
            RectTransform baseTransform = ParentObject.transform as RectTransform;
            RectTransform rectTransform = CreateObject.AddComponent<RectTransform>();
            //rectTransform.anchorMax = baseTransform.anchorMax;
            //rectTransform.anchorMin = baseTransform.anchorMin;
            //rectTransform.anchoredPosition = baseTransform.anchoredPosition;
            //rectTransform.sizeDelta = baseTransform.sizeDelta;
            //rectTransform.localScale = baseTransform.localScale;
            //rectTransform.localPosition = baseTransform.localPosition;
            //rectTransform.localRotation = baseTransform.localRotation;

            // Imageコンポーネントの追加
            CreateObject.AddComponent<Image>();

            return CreateObject;
        }

    }
}