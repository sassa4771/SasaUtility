using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SasaUtility
{
    public static class Texture2Png
    {

        /// <summary>
        /// RawImage??????????????????
        /// </summary>
        /// <param name="path">png???p?X</param>
        /// <param name="tex">Texture</param>
        public static void ConvertToPngAndSave(string path, RawImage _rawImage)
        {
            Debug.Log(path);
            //Texture2D??????
            //Texture2D Image2D = _RawImage.texture as Texture2D;
            //Png??????
            byte[] bytes = RawImageToPNG(_rawImage);
            //????
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Texture??????????????????
        /// </summary>
        /// <param name="path">png???p?X</param>
        /// <param name="tex">Texture</param>
        public static void ConvertToPngAndSave(string path, Texture tex)
        {
            Debug.Log(path);
            //Texture2D??????
            //Texture2D Image2D = _RawImage.texture as Texture2D;
            //Png??????
            byte[] bytes = TextureToPNG(tex);
            //????
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Texture2D??????????????????
        /// </summary>
        /// <param name="path">png???p?X</param>
        /// <param name="tex">Texture</param>
        public static void ConvertToPngAndSave(string path, Texture2D tex2d, bool debug = false)
        {
            if(debug)Debug.Log(path);
            //Texture2D??????
            //Texture2D Image2D = _RawImage.texture as Texture2D;
            //Png??????
            byte[] bytes = Texture2DToPNG(tex2d);
            //????
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// ?I???????p?X????????image?X?v???C?g???????E?\???????X?N???v?g
        /// </summary>
        /// <param name="path">png???p?X</param>
        /// <param name="image">UnityEngine.UI.Image</param>
        public static void ConvertToTextureAndLoad(string path, Image image)
        {
            //????????
            byte[] bytes = File.ReadAllBytes(path);
            //???????e?N?X?`????????
            Texture2D loadTexture = new Texture2D(2, 2);
            loadTexture.LoadImage(bytes);

            //?e?N?X?`?????X?v???C?g??????
            image.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.zero);
        }

        /// <summary>
        /// RawImage??PNG???G???R?[?h???????\?b?h
        /// </summary>
        /// <param name="_rawImage"></param>
        /// <returns></returns>
        private static byte[] RawImageToPNG(RawImage _rawImage)
        {
            byte[] bytes = TextureToPNG(_rawImage.texture);

            return bytes;
        }
        
        /// <summary>
        /// Texture??PNG???G???R?[?h???????\?b?h
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
        /// Texutre2D??PNG???G???R?[?h???????\?b?h
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