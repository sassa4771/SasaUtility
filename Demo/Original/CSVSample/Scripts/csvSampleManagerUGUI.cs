using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace SasaUtility.Demo.Original
{
    public class csvSampleManagerUGUI : MonoBehaviour
    {
        [SerializeField] Button StartButton;
        [SerializeField] Button StopButton;
        csvSmapleManager csv;

        void Start()
        {
            csv = GetComponent<csvSmapleManager>();

            StartButton.OnPointerClickAsObservable().Subscribe(_ =>
            {
                csv.StartSave();
                StartButton.gameObject.SetActive(false);
                StopButton.gameObject.SetActive(true);
            }).AddTo(this);

            StopButton.OnPointerClickAsObservable().Subscribe(_ =>
            {
                csv.StopSave();
                StartButton.gameObject.SetActive(true);
                StopButton.gameObject.SetActive(false);
            }).AddTo(this);
        }
    }
}