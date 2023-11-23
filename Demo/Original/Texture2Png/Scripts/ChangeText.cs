using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using TMPro;

namespace SasaUtility.Demo.Original
{
    public class ChangeText : MonoBehaviour
    {
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private TextMeshProUGUI _text;

        void Start()
        {
            //セーブボタン
            _saveButton.OnPointerClickAsObservable().Subscribe(_ => textChange("Image Saved!")).AddTo(this);
            //ロードボタン
            _loadButton.OnPointerClickAsObservable().Subscribe(_ => textChange("Image Load!")).AddTo(this);
        }

        private void textChange(string changeText)
        {
            _text.text = changeText;
        }
    }
}