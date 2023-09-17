using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class TestCamera : MonoBehaviour
{
    [SerializeField] private int m_width = 1920;
    [SerializeField] private int m_height = 1080;
    [SerializeField] private RawImage m_displayUI = null;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _stopButton;

    private WebCamTexture m_webCamTexture = null;

    private IEnumerator Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No Camera Device");
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.LogFormat("Not allow access to camera");
            yield break;
        }

        // とりあえず最初に取得されたデバイスを使ってテクスチャを作りますよ。
        WebCamDevice userCameraDevice = WebCamTexture.devices[0];
        m_webCamTexture = new WebCamTexture(userCameraDevice.name, m_width, m_height);

        m_displayUI.texture = m_webCamTexture;

        // さあ、撮影開始だ！
        m_webCamTexture.Play();

        //UniRxのサブスクライブ
        _startButton.OnPointerClickAsObservable().Subscribe(_ => OnPlay()).AddTo(this);
        _stopButton.OnPointerClickAsObservable().Subscribe(_ => OnStop()).AddTo(this);
    }

    /// <summary>
    /// カメラ再生開始
    /// </summary>
    public void OnPlay()
    {
        if (m_webCamTexture == null)
        {
            return;
        }

        if (m_webCamTexture.isPlaying)
        {
            return;
        }

        m_webCamTexture.Play();
    }

    /// <summary>
    /// カメラ再生停止
    /// </summary>
    public void OnStop()
    {
        if (m_webCamTexture == null)
        {
            return;
        }

        if (!m_webCamTexture.isPlaying)
        {
            return;
        }

        m_webCamTexture.Stop();
    }
}