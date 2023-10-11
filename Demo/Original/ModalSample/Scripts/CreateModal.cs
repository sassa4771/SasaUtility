using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SasaUtility;
using TMPro;

public class CreateModal : MonoBehaviour
{
    [SerializeField] GameObject FadeoutModal;
    [SerializeField] GameObject FadeoutModalButton;
    [SerializeField] Transform ModalArea;

    public void OnClickCreateFadeoutModal()
    {
        GameObject instance = Instantiate(FadeoutModal, Vector2.zero, Quaternion.identity, ModalArea);
        TextMeshProUGUI text = instance.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        string filename = PathController.GetDateTimeFileName();
        text.text = filename + "Ç…FadeoutModalÇçÏê¨ÇµÇ‹ÇµÇΩÅB";
    }

    public void OnClickCreateFadeoutModalButton()
    {
        GameObject instance = Instantiate(FadeoutModalButton, Vector2.zero, Quaternion.identity, ModalArea);
        TextMeshProUGUI text = instance.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        string filename = PathController.GetDateTimeFileName();
        text.text = filename + "Ç…FadeoutModalButtonÇçÏê¨ÇµÇ‹ÇµÇΩÅB";
        Button button = instance.transform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(ButtonCick);
    }

    public void ButtonCick()
    {
        Debug.Log("You Click Button");
    }
}
