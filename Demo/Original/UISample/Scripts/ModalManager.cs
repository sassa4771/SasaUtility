using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace SasaUtility.Demo.Original
{
  public class ModalManager : MonoBehaviour
  {
    public static ModalManager instance { get { return Instance; } }
    private static ModalManager Instance;

    [SerializeField] GameObject FadeoutModal;
    [SerializeField] GameObject FadeoutModalButton;
    [SerializeField] Transform ModalArea;

    private void Awake()
    {
      if (instance == null) Instance = this;
      else Destroy(gameObject);
    }

    public void CreateModal(string message)
    {
      GameObject instance = Instantiate(FadeoutModal, Vector2.zero, Quaternion.identity, ModalArea);
      TextMeshProUGUI text = instance.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
      text.text = message;
    }

    public void CreateModalButton(string message, string messageButton, UnityAction action = null)
    {
      GameObject instance = Instantiate(FadeoutModalButton, Vector2.zero, Quaternion.identity, ModalArea);
      TextMeshProUGUI text = instance.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
      text.text = message;

      Button button = instance.transform.Find("Button").GetComponent<Button>();
      TextMeshProUGUI buttonText = button.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

      if (action != null) button.onClick.AddListener(action);
      buttonText.text = messageButton;
    }

    public void ButtonCick()
    {
      Debug.Log("You Click Button");
    }
  }
}