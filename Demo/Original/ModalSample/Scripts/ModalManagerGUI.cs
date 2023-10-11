using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;

namespace SasaUtility.Demo.Original
{
  public class ModalManagerGUI : MonoBehaviour
  {
    [SerializeField] Button Button_Modal;
    [SerializeField] Button Button_ModalButton;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI textGUI;
    ModalManager modal;
    int count = 0;

    private void Start()
    {
      if (modal == null) modal = ModalManager.instance;

      Button_Modal.OnPointerClickAsObservable().Subscribe(_=>
      {
        OnClickCreateFadeoutModal();
      }).AddTo(this);

      Button_ModalButton.OnPointerClickAsObservable().Subscribe(_ =>
      {
        OnClickCreateFadeoutModalButton();
      }).AddTo(this);
    }

    public void OnClickCreateFadeoutModal()
    {
      string message = inputField.text;
      if (message == "") message = "モーダルデモ";

      modal.CreateModal(message);
    }

    public void OnClickCreateFadeoutModalButton()
    {
      string message = inputField.text;
      if (message == "") message = "モーダルデモ";


      modal.CreateModalButton(message, "Action", ActionButton);
    }

    private void ActionButton()
    {
      string actionMessage = "アクションクリック:" + count.ToString();
      count++;
      Debug.Log(actionMessage);
      textGUI.text = actionMessage;
    }
  }
}