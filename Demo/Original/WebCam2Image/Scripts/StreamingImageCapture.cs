using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SasaUtility.Demo.Original
{
  public class StreamingImageCapture : MonoBehaviour
  {
    private string savePath = "SavedImages/";

    private WebCamTexture webcamTexture;

    public RawImage rawimage;
    private bool capturing = false;

    private List<Texture2D> saveQueue = new List<Texture2D>();

    int count = 0;

    private void Start()
    {
      webcamTexture = new WebCamTexture();

      rawimage.texture = webcamTexture;
      webcamTexture.Play();
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        capturing = !capturing;

        if (capturing)
        {
          Debug.Log("Start capturing");
        }
        else
        {
          Debug.Log("Stop capturing");
          Debug.Log(Application.dataPath + "/" + savePath);
          System.Diagnostics.Process.Start(Application.dataPath + "/" + savePath);
        }
      }

      if (capturing)
      {
        StartCoroutine(SaveFramesCoroutine());
        Debug.Log(saveQueue.Count);
        saveQueue.Add(new Texture2D(webcamTexture.width, webcamTexture.height));
      }
    }

    /// <summary>
    /// saveQueue?????????????????????????????????\?b?h
    /// </summary>
    /// <returns></returns>
    private IEnumerator SaveFramesCoroutine()
    {
      yield return null;

      string previousImage = null;

      while (saveQueue.Count > 0)
      {
        if (previousImage == null || File.Exists(previousImage))
        {
          previousImage = saveImage(saveQueue[0]);
          saveQueue.RemoveAt(0);
          Debug.Log("?????c???F" + saveQueue.Count);
        }
        yield return null;
      }
    }

    /// <summary>
    /// ?????????????????\?b?h
    /// </summary>
    /// <param name="txt2d"></param>
    /// <returns></returns>
    private string saveImage(Texture2D txt2d)
    {
      txt2d.SetPixels(webcamTexture.GetPixels());
      txt2d.Apply();

      //byte[] bytes = txt2d.EncodeToPNG();
      byte[] bytes = txt2d.EncodeToJPG();
      string filename = "image_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + count + ".png";
      string filePath = Path.Combine(Application.dataPath + "/" + savePath, filename);

      if (!Directory.Exists(Application.dataPath + "/" + savePath))
      {
        Directory.CreateDirectory(Application.dataPath + "/" + savePath);
      }
      File.WriteAllBytes(filePath, bytes);
      count++;

      return filePath;
    }


    private void OnApplicationQuit()
    {
      webcamTexture.Stop();
    }
  }
}