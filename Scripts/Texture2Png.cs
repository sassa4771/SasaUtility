using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SasaUtility
{
    public static class Texture2Png
    {

        /// <summary>
        /// RawImageを画像に変換＆保存
        /// </summary>
        /// <param name="path">pngのパス</param>
        /// <param name="tex">Texture</param>
        public static void ConvertToPngAndSave(string path, RawImage _rawImage)
        {
            Debug.Log(path);
            //Texture2Dに変換
            //Texture2D Image2D = _RawImage.texture as Texture2D;
            //Pngに変換
            byte[] bytes = RawImageToPNG(_rawImage);
            //保存
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Textureを画像に変換＆保存
        /// </summary>
        /// <param name="path">pngのパス</param>
        /// <param name="tex">Texture</param>
        public static void ConvertToPngAndSave(string path, Texture tex)
        {
            Debug.Log(path);
            //Texture2Dに変換
            //Texture2D Image2D = _RawImage.texture as Texture2D;
            //Pngに変換
            byte[] bytes = TextureToPNG(tex);
            //保存
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Texture2Dを画像に変換＆保存
        /// </summary>
        /// <param name="path">pngのパス</param>
        /// <param name="tex">Texture</param>
        public static void ConvertToPngAndSave(string path, Texture2D tex2d, bool debug = false)
        {
            if(debug)Debug.Log(path);
            //Texture2Dに変換
            //Texture2D Image2D = _RawImage.texture as Texture2D;
            //Pngに変換
            byte[] bytes = Texture2DToPNG(tex2d);
            //保存
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// 選択したパスの画像をimageスプライトに変換・表示するスクリプト
        /// </summary>
        /// <param name="path">pngのパス</param>
        /// <param name="image">UnityEngine.UI.Image</param>
        public static void ConvertToTextureAndLoad(string path, Image image)
        {
            //読み込み
            byte[] bytes = File.ReadAllBytes(path);
            //画像をテクスチャに変換
            Texture2D loadTexture = new Texture2D(2, 2);
            loadTexture.LoadImage(bytes);

            //テクスチャをスプライトに変換
            image.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.zero);
        }

        /// <summary>
        /// RawImageをPNGにエンコードするメソッド
        /// </summary>
        /// <param name="_rawImage"></param>
        /// <returns></returns>
        private static byte[] RawImageToPNG(RawImage _rawImage)
        {
            byte[] bytes = TextureToPNG(_rawImage.texture);

            return bytes;
        }
        
        /// <summary>
        /// TextureをPNGにエンコードするメソッド
        /// </summary>
        /// <param name="tex"></param>
        /// <returns></returns>
        private static byte[] TextureToPNG(Texture tex)
        {
            Texture2D tex2d = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            RenderTexture.active = (RenderTexture)tex;

            byte[] pngBytes = Texture2DToPNG(tex2d);
            
            RenderTexture.active = null;
            Texture2D.Destroy(tex2d);

            return pngBytes;
        }

        /// <summary>
        /// Texutre2DをPNGにエンコードするメソッド
        /// </summary>
        /// <param name="tex2D"></param>
        /// <returns></returns>
        private static byte[] Texture2DToPNG(Texture2D tex2d)
        {
            tex2d.ReadPixels(new Rect(0, 0, tex2d.width, tex2d.height), 0, 0, false);
            tex2d.Apply(false, false);

            byte[] pngBytes = tex2d.EncodeToPNG();
            return pngBytes;
        }
    }
}