using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SasaUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace SasaUtility.Demo
{
    public class VideoPlayerManager : MonoBehaviour
    {
        [SerializeField] private Button PlayButton;
        [SerializeField] private Button PauseButton;
        [SerializeField] private RawImage rawImage; //映像を表示するRawImage
        [SerializeField] private Slider slider;
        [SerializeField] private Slider visualslider;
        [SerializeField] private Slider TrimSliderStart;
        [SerializeField] private Slider TrimSliderEnd;
        [SerializeField] private GameObject Handle;
        public GameObject VisualHandle;
        public VideoPlayer vp;
        private int maxFrame = 0;

        private bool isTouching = false;
        private bool isMasterPause = false;

        private float startTime;
        public long startFrame;
        private float endTime;
        public long endFrame;
        private long curFrame;
        private bool loopPointReached = false;

        private void Awake()
        {
            vp = this.GetComponent<VideoPlayer>();
            Handle.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            VisualHandle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        // Start is called before the first frame update
        void Start()
        {
            vp.prepareCompleted += OnVideoPrepared;
            vp.Prepare();

            slider.onValueChanged.AddListener(OnSliderChange);

            PlayButton.OnPointerClickAsObservable().Subscribe(_ =>
            {
                isMasterPause = false;
                PlayButton.gameObject.SetActive(false);
                PauseButton.gameObject.SetActive(true);
                vp.Play();
            }).AddTo(this);

            PauseButton.OnPointerClickAsObservable().Subscribe(_ =>
            {
                isMasterPause = true;
                PlayButton.gameObject.SetActive(true);
                PauseButton.gameObject.SetActive(false);
                vp.Pause();
            }).AddTo(this);
        }

        private void Update()
        {
            curFrame = vp.frame;
            endFrame = (long)(TrimSliderEnd.value * maxFrame);
            startFrame = (long)(TrimSliderStart.value * maxFrame);
            startTime = (startFrame / vp.frameRate);
            endTime = (endFrame / vp.frameRate);

            if (!isTouching)
                visualslider.value = (float)vp.frame / maxFrame;

            if (curFrame < startFrame) vp.time = startTime;

            //終了時間の監視
            if (curFrame >= (endFrame - 1) && !loopPointReached) //endFrame-1にしているのは、最後まで行くと再生が止まってしまうため
            {
                Debug.Log("loopPointReached=true, endTime:" + endTime + ", startTime:" + startTime + ", frameRate:" + vp.frameRate);
                vp.Pause();

                //時間で変更するとvideoplayerの応答が早い
                vp.time = startTime;
                loopPointReached = true;
            }

            if (curFrame < (endFrame - 1) && loopPointReached)
            {
                Debug.Log("loopPointReached=false");
                loopPointReached = false;
                vp.Play();
            }
        }

        /// <summary>
        /// videoPlayerの準備ができたら呼び出されるメソッド
        /// </summary>
        /// <param name="source"></param>
        private void OnVideoPrepared(VideoPlayer source)
        {
            rawImage.texture = source.texture;
            source.Play();

            //動画の最大フレームを取得
            maxFrame = (int)vp.frameCount;
            Debug.Log("Max Frame: " + maxFrame);
        }

        public void OnClickPlay()
        {
            vp.Play();
        }

        public void OnClickPause()
        {
            vp.Pause();
        }

        public void OnClickStop()
        {
            vp.Stop();
        }

        public void OnSliderChange(float value)
        {
            vp.Pause();
            Handle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            VisualHandle.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            vp.frame = (long)(value * maxFrame);
            //Debug.Log(vp.frame + ", " + (long)(value * maxFrame));
            isTouching = true;
        }

        // 動画再生ハンドルがマウスクリックやタップが離された時
        public void OnPointerUp()
        {
            if (isTouching)
            {
                if (!isMasterPause) vp.Play();
                Handle.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                VisualHandle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                isTouching = false;
            }
            Debug.Log("OnPointerUp");
        }
    }
}
