using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace SasaUtility.InputSystem.PenDemo
{
    public class PenSupportManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displaynameText;           // Apple Pencil対応
        [SerializeField] private TextMeshProUGUI tipText;                   // Apple Pencil対応
        [SerializeField] private TextMeshProUGUI eraserText;
        [SerializeField] private TextMeshProUGUI firstBarrelButtonText;
        [SerializeField] private TextMeshProUGUI secondBarrelButtonText;
        [SerializeField] private TextMeshProUGUI thirdBarrelButtonText;
        [SerializeField] private TextMeshProUGUI fourthBarrelButtonText;
        [SerializeField] private TextMeshProUGUI inRangeText;
        [SerializeField] private TextMeshProUGUI tiltText;                  // Apple Pencil対応
        [SerializeField] private TextMeshProUGUI twistText;
        [SerializeField] private TextMeshProUGUI pressureText;              // Apple Pencil対応
        [SerializeField] private TextMeshProUGUI penPositionText;           // Apple Pencil対応

        void Update()
        {
            displaynameText.text = "displayName: " + Pen.current.displayName;
            tipText.text = "tip: " + Pen.current.tip.ReadValue();
            eraserText.text = "eraser: " + Pen.current.eraser.ReadValue();
            firstBarrelButtonText.text = "firstBarrelButton: " + Pen.current.firstBarrelButton.ReadValue();
            secondBarrelButtonText.text = "secondBarrelButton: " + Pen.current.secondBarrelButton.ReadValue();
            thirdBarrelButtonText.text = "thirdBarrelButton: " + Pen.current.thirdBarrelButton.ReadValue();
            fourthBarrelButtonText.text = "fourthBarrelButton: " + Pen.current.fourthBarrelButton.ReadValue();
            inRangeText.text = "inRange: " + Pen.current.inRange.ReadValue();
            tiltText.text = "tilt: " + Pen.current.tilt.ReadValue();
            twistText.text = "twist: " + Pen.current.twist.ReadValue();
            pressureText.text = "pressure: " + Pen.current.pressure.ReadValue();

            if (Pen.current.tip.ReadValue() > 0)
            {
                //Vector2 temppos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 temppos = Input.mousePosition;
                penPositionText.text = "position: " + temppos.ToString();
            }
        }

        public void DestroyDraw()
        {
            string targetName = "Draw(Clone)";
            string targetName2 = "Slash(Clone)";

            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in objects)
            {
                if (obj.name == targetName || obj.name == targetName2)
                {
                    Destroy(obj);
                }
            }
        }
    }
}