
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using RenderHeads.Media.AVProMovieCapture;
using TMPro;
using System;
using SasaUtility.Demo.Original;

namespace SasaUtility.AVPro
{
    /// <summary>
    /// Web?J?????????f???????????A?h???b?v?_?E?????X?g???\?????????\?b?h
    /// ?I??????????????????
    /// </summary>
    public class WebCamCapture : MonoBehaviour
    {
        public static WebCamCapture instance { get { return Instance; } }
        private static WebCamCapture Instance;

        [SerializeField] int _webcamResolutionWidth = 1920;
        [SerializeField] int _webcamResolutionHeight = 1080;
        [SerializeField] int _webcamFrameRate = 30;
        [SerializeField] RawImage rawImage;
        private TMP_Dropdown WebCamDropdown;
        [SerializeField] CaptureFromWebCamTexture capture;
        // State
        private int _selectedWebcamIndex = -1;
        private string currentDevice = null;
        private WebCamTexture webtex;

        private string directoryPath;

        public bool isCapturing { get { return capture.IsCapturing(); } }

        public string MAIN_SAVE_FOLDER = "Movie";

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            WebCamDropdown = this.GetComponent<TMP_Dropdown>();
            capture.CompletedFileWritingAction += OnCompleteFinalFileWriting;
        }

#if AVPRO_MOVIECAPTURE_WEBCAMTEXTURE_SUPPORT

        private IEnumerator Start()
        {
            Application.targetFrameRate = 60;

            // Make sure we're authorised for using the camera. On iOS the OS will forcibly
            // close the application if authorisation has not been granted. Make sure the
            // "Camera Usage Description" field has been filled in the player settings.
            // This needs to be done first otherwise no cameras will be reported.
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            }

            // Make sure we're authorised for using the microphone. On iOS the OS will forcibly
            // close the application if authorisation has not been granted. Make sure the
            // "Microphone Usage Description" field has been filled in the player settings.
            // if (_capture.AudioCaptureSource == AudioCaptureSource.Microphone)
            {
                if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
                {
                    yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
                }
                if (Application.HasUserAuthorization(UserAuthorization.Microphone))
                {
                    // On iOS modified the audio session to allow recording from the microphone.
                    NativePlugin.SetMicrophoneRecordingHint(true);
                }
            }

            ShowDropDownList();
        }

        private void ShowDropDownList()
        {
            // Create instance data per webcam
            int numCams = WebCamTexture.devices.Length;

            TMP_Dropdown.OptionDataList options = new TMP_Dropdown.OptionDataList();
            for (int i = 0; i < numCams; i++)
            {
                Debug.Log("Device: " + WebCamTexture.devices[i].name);
                options.options.Add(new TMP_Dropdown.OptionData(WebCamTexture.devices[i].name));
            }
            // Clear the existing options in the dropdown
            WebCamDropdown.ClearOptions();
            // Assign the options list to the dropdown
            WebCamDropdown.options = options.options;

            if (numCams > 0)
            {
                SelectWebcam(0);
            }
        }

        private void SelectWebcam(int index)
        {

            // Stop any currently
            if (_selectedWebcamIndex >= 0)
            {
                StopWebcam();
                _selectedWebcamIndex = -1;
            }

            if (index >= 0)
            {
                _selectedWebcamIndex = index;
                currentDevice = WebCamTexture.devices[_selectedWebcamIndex].name;
                StartWebcam(currentDevice);
            }
        }

        private void StartWebcam(string deviceName)
        {
            Debug.LogFormat("_webcamResolutionWidth: {0}, _webcamResolutionHeight: {1}, _webcamFrameRate: {2}", _webcamResolutionWidth, _webcamResolutionHeight, _webcamFrameRate);
            webtex = new WebCamTexture(deviceName);
            webtex.Play();

            //???????\??
            this.rawImage.texture = webtex;
            this.rawImage.rectTransform.sizeDelta = new Vector2(1978, 1978 * webtex.height / webtex.width);

            if (webtex.isPlaying)
            {
                capture.WebCamTexture = webtex;
            }
            else
            {
                Debug.Log(string.Format("Unable to start webcam in mode {0}x{1}@{2}", _webcamResolutionWidth, _webcamResolutionHeight, _webcamFrameRate));
                StopWebcam();
            }
        }

        private void StopWebcam()
        {
            if (webtex != null)
            {
                if (capture != null && isCapturing)
                {
                    capture.WebCamTexture = null;
                    capture.StopCapture();
                }

                webtex.Stop();
                //Destroy(webtex);
                webtex = null;
            }

            _selectedWebcamIndex = -1;
        }

        /// <summary>
        /// DrioDown?????X?i?????j??????????????????
        /// </summary>
        /// <param name="index"></param>
        public void OnDropdownValueChanged(int index)
        {
            SelectWebcam(index);
            Debug.Log("You Select: " + index);
        }

        public void StopRecording()
        {
            capture.StopCapture();
        }

        public void StartRecording()
        {
            capture.StartCapture();
        }

        public void CancelRecording()
        {
            capture.CancelCapture();
        }

        /// <summary>
        /// ????????????????
        /// </summary>
        /// <param name="handler"></param>
        public void OnCompleteFinalFileWriting(FileWritingHandler handler)
        {
            // ?t?@?C????????????
            if (File.Exists(handler.Path))
            {
                //?????t?H???_?[?????t?@?C??????????
                string FolderName = PathController.GetDateTimeFileName();
                directoryPath = MAIN_SAVE_FOLDER + "/" + FolderName;
                string destinationFilePath = PathController.GetSavePath(directoryPath, "mp4");
                Debug.Log("Before: " + handler.Path + " ,After: " + destinationFilePath);

                File.Move(handler.Path, destinationFilePath);
                Debug.Log("File moved successfully.");

                VideoFrameSplitter vfs = VideoFrameSplitter.instance;
                vfs.SetVideo(destinationFilePath);
                StartCoroutine(vfs.GetFirstFrame(directoryPath));
              //ModalManager.instance.CreateModal("???????????????????B");

              Utils.ShowInExplorer(destinationFilePath);
            }
            else
            {
                // ?^???L?????Z??????????
                Debug.Log("Source file does not exist.");
            }
        }

        private void OnDestroy()
        {
            StopWebcam();
        }

        /// <summary>
        /// ?h???b?v?_?E?????X?g???X?V
        /// </summary>
        public void OnButtonRefresh()
        {
            ShowDropDownList();
        }
#else
		void Start()
		{
			Debug.LogError("[AVProMovieCapture] To use WebCamTexture capture component/demo you must add the string AVPRO_MOVIECAPTURE_WEBCAMTEXTURE_SUPPORT must be added to `Scriping Define Symbols` in `Player Settings > Other Settings > Script Compilation`");
		}
#endif  // AVPRO_MOVIECAPTURE_WEBCAMTEXTURE_SUPPORT
    }
}