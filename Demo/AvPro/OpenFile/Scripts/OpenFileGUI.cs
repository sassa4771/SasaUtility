using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProMovieCapture;
using UniRx;
using UniRx.Triggers;

public class OpenFileGUI : MonoBehaviour
{
  [SerializeField] Button OpenButton;
  // Start is called before the first frame update
  void Start()
  {
    OpenButton.OnPointerClickAsObservable().Subscribe(_ =>
    {
      BrowseFile();
    }).AddTo(this);
  }

  public void BrowseFile()
  {
    string foldername = Application.persistentDataPath;
    //Utils.ShowInExplorer(CaptureBase.LastFileSaved);
    Utils.ShowInExplorer(foldername + @"/");
  }
}
