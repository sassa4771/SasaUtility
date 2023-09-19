using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SasaUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace SasaUtility.Demo.Original
{
    public class VideoPlayerManager : MonoBehaviour
    {
        [SerializeField] private Button PlayButton;
        [SerializeField] private Button PauseButton;
        [SerializeField] private RawImage rawImage; //?f?????\??????RawImage
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

            //?I????????????
            if (curFrame >= (endFrame - 1) && !loopPointReached) //endFrame-1???????????????A?????????s???????????~????????????????
            {
                Debug.Log("loopPointReached=true, endTime:" + endTime + ", startTime:" + startTime + ", frameRate:" + vp.frameRate);
                vp.Pause();

                //?????????X??????videoplayer????????????
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
        /// videoPlayer?????????????????????o?????????\?b?h
        /// </summary>
        /// <param name="source"></param>
        private void OnVideoPrepared(VideoPlayer source)
        {
            rawImage.texture = source.texture;
            source.Play();

            //???????????t???[????????
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

        // ?????????n???h?????}?E?X?N???b?N???^?b?v????????????
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
