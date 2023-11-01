using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;  // UIテキストオブジェクトへの参照

    private void Update()
    {
        // 現在のフレームレートを計算
        float currentFPS = 1.0f / Time.deltaTime;

        // テキストにFPSを表示
        fpsText.text = "FPS: " + Mathf.Round(currentFPS).ToString();
    }
}
