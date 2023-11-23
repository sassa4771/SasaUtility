using UnityEngine;

namespace SasaUtility.Demo.Original
{
  public class FadeOutImage : MonoBehaviour
  {
    public float waitTime = 3.0f;     // 待ち時間
    public float fadeTime = 3.0f;
    private float fadeSpeed = 3.0f;    // 透明度が変わるスピード

    public CanvasGroup canvasGroup;   // Materialにアクセスする容器
    private float countTime = 0;

    private void Start()
    {
      fadeSpeed = fadeTime;
    }

    void Update()
    {
      countTime += Time.deltaTime;
      if (countTime > waitTime)
      {
        fadeSpeed -= Time.deltaTime;
        StartFadeOut(); //boolにチェックが入っていたら実行     
      }
    }

    void StartFadeOut()
    {
      canvasGroup.alpha = fadeSpeed / fadeTime;

      if (fadeSpeed <= 0)              // 完全に透明になったら処理を抜ける
      {
        Destroy(this.gameObject);
      }
    }
  }
}