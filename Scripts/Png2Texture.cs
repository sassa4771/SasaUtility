using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace SasaUtility
{
    public static class Png2Texture
    {
        /// <summary>
        /// テクスチャに変換＆読み込み
        /// </summary>
        public static Texture2D PngToTexture2D(string path)
        {
            //読み込み
            byte[] bytes = File.ReadAllBytes(path);
            //画像をテクスチャに変換
            Texture2D loadTexture = new Texture2D(2, 2);
            loadTexture.LoadImage(bytes);

            return loadTexture;
        }

        public static Sprite PngToSprite(string path)
        {
            Texture2D loadTexture = PngToTexture2D(path);
            return Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.zero);
        }
    }
}