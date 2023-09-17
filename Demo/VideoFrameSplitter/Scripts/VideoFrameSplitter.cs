using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

namespace SasaUtility.Demo
{
    public class VideoFrameSplitter : MonoBehaviour
    {
        [SerializeField] private bool ShowImage = false;
        [SerializeField] private int splitImage = 32;
        [SerializeField] private RawImage rawImage;

        private VideoPlayer vp;
        private int maxFrame = 0;
        private bool seekDone = true;
        protected const string IMAGE_SAVE_FOLDER = "Image";

        void Start()
        {
            vp = this.GetComponent<VideoPlayer>();
            vp.prepareCompleted += OnVideoPrepared;
            vp.Prepare();
            vp.seekCompleted += seekCompleted;
        }

        void seekCompleted(VideoPlayer par)
        {
            StartCoroutine(WaitToUpdateRenderTextureBeforeEndingSeek());
        }

        IEnumerator WaitToUpdateRenderTextureBeforeEndingSeek()
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("Do Seek True");
            seekDone = true;
        }

        private void OnVideoPrepared(VideoPlayer source)
        {
            //動画の最大フレームを取得
            maxFrame = (int)vp.frameCount;
            Debug.Log("Max Frame: " + maxFrame);

            if (ShowImage) rawImage.texture = source.texture;
            source.Play();
            source.Pause();
            StartCoroutine(VideoSplite());
        }

        /// <summary>
        /// videoPlayerの動画をsplitImageの回数で分割して、pngを保存する
        /// </summary>
        /// <returns></returns>
        IEnumerator VideoSplite()
        {
            string[] savePath = new string[splitImage];
            int count = 0;
            int frameCount = 0;
            int previosFrame = 0;

            //Debug.Log(maxFrame / 32);
            while(count < splitImage) {
                if (count==0 || File.Exists(savePath[count-1]))
                {
                    if (seekDone)
                    {
                        frameCount += maxFrame / splitImage;
                        vp.frame = frameCount;
                        // You should pause while you seek for better stability
                        vp.Pause();
                        seekDone = false;

                        if (vp.frame == previosFrame) vp.frame = frameCount + 1;
                         yield return new WaitForSeconds(1);

                        savePath[count] = SaveVideoPlayerFrame();
                        Debug.Log("frameCount: " + frameCount + ", frame: " + vp.frame + ", previous: " + previosFrame);
                        count++;
                        previosFrame = frameCount;
                    }
                }
                yield return null;
            }
        }

        /// <summary>
        /// 現在のvideoPlayerのFrameをpngとして保存するメソッド
        /// </summary>
        /// <returns>保存先のパスを返却</returns>
        string SaveVideoPlayerFrame()
        {
            Texture2D tex = new Texture2D((int)vp.texture.width, (int)vp.texture.height, TextureFormat.RGB24, false);
            RenderTexture.active = (RenderTexture)vp.texture;
            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            tex.Apply();

            string savePath = PathController.GetSavePath(IMAGE_SAVE_FOLDER, "png");
            Texture2Png.ConvertToPngAndSave(savePath, tex);

            Debug.Log(vp.frame);

            Destroy(tex);

            return savePath;
        }
    }
}