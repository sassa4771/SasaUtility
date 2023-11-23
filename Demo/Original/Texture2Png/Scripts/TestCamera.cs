using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;
using TMPro;

namespace SasaUtility.Demo.Original
{
    public class TestCamera : MonoBehaviour
    {
        [SerializeField] private int m_width = 1920;
        [SerializeField] private int m_height = 1080;
        [SerializeField] private RawImage m_displayUI = null;
        [SerializeField] private TMP_Dropdown cameraDropdown; // Webカメラを選択するためのDropdown
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

            // WebカメラをDropdownに追加する
            cameraDropdown.ClearOptions();
            List<string> cameraOptions = new List<string>();
            foreach (var device in WebCamTexture.devices)
            {
                cameraOptions.Add(device.name);
            }
            cameraDropdown.AddOptions(cameraOptions);

            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Debug.LogFormat("Not allow access to camera");
                yield break;
            }

            // 最初に取得されたデバイスを使ってテクスチャを作成
            string selectedDeviceName = cameraOptions[0];
            m_webCamTexture = new WebCamTexture(selectedDeviceName, m_width, m_height);

            m_displayUI.texture = m_webCamTexture;

            // カメラ再生開始ボタンのSubscribe
            _startButton.OnPointerClickAsObservable().Subscribe(_ => OnPlay()).AddTo(this);
            // カメラ再生停止ボタンのSubscribe
            _stopButton.OnPointerClickAsObservable().Subscribe(_ => OnStop()).AddTo(this);

            OnPlay();
        }

        /// <summary>
        /// カメラ再生開始
        /// </summary>
        public void OnPlay()
        {
            if (m_webCamTexture == null || m_webCamTexture.isPlaying)
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
            if (m_webCamTexture == null || !m_webCamTexture.isPlaying)
            {
                return;
            }

            m_webCamTexture.Stop();
        }

        /// <summary>
        /// Dropdownで選択されたカメラを設定
        /// </summary>
        public void SetCamera()
        {
            string selectedDeviceName = cameraDropdown.options[cameraDropdown.value].text;
            m_webCamTexture.Stop();
            m_webCamTexture.deviceName = selectedDeviceName;
            m_webCamTexture.Play();
        }
    }
}
