using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SasaUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button PauseButton;
    [SerializeField] private RawImage rawImage; //映像を表示するRawImage
    [SerializeField] private Slider videoSlider;
    [SerializeField] private Slider visualslider;
    [SerializeField] private GameObject Handle;
    public GameObject VisualHandle;
    private VideoPlayer vp;
    private int maxFrame = 0;

    private bool isTouching = false;
    private bool isMasterPause = false;

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

        videoSlider.onValueChanged.AddListener(OnSliderChange);

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
        if (!isTouching)
            visualslider.value = (float)vp.frame / maxFrame;
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

    // マウスクリックやタップが離された時
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