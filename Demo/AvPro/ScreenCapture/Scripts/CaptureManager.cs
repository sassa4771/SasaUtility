using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using RenderHeads.Media.AVProMovieCapture;
using TMPro;
using System;
using System.Collections.Generic;
using UniRx;

namespace SasaUtility.Demo.AVPro
{
  public class CaptureManager : MonoBehaviour
  {
    public static CaptureManager instance { get { return Instance; } }
    private static CaptureManager Instance;

    [Header("Capture")]
    [SerializeField] CaptureBase _capture = null;

    [Header("Button")]
    [SerializeField] Button StartButton;
    [SerializeField] Button StopButton;
    [SerializeField] Button BrowseButton;

    public bool canCapture = false;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Start()
    {
      StartButton.OnClickAsObservable().Subscribe(_ =>
      {
        StartRecording();
      }).AddTo(this);
      StopButton.OnClickAsObservable().Subscribe(_ =>
      {
        StopRecording();
      }).AddTo(this);
      BrowseButton.OnClickAsObservable().Subscribe(_ =>
      {
        BrowseFile();
      }).AddTo(this);

      _capture.CompletedFileWritingAction += OnCompleteFinalFileWriting;
    }

    public void StartRecording()
    {
      if (canCapture) _capture.StartCapture();
    }

    public void StopRecording()
    {
      if (canCapture) _capture.StopCapture();
    }

    public void BrowseFile()
    {
      string foldername = Application.persistentDataPath;
      //Utils.ShowInExplorer(CaptureBase.LastFileSaved);
      Utils.ShowInExplorer(foldername + @"/");
    }

    private void OnCompleteFinalFileWriting(FileWritingHandler handler)
    {
      string fileName = System.IO.Path.GetFileName(handler.Path);
      Original.ModalManager.instance.CreateModalButton(fileName + "is Saved!", "Open File", () => { Utils.ShowInExplorer(CaptureBase.LastFileSaved); });
      Debug.Log("Completed capture '" + handler.Path + "' with status: " + handler.Status.ToString());
    }
  }
}