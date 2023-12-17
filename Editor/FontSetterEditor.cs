using TMPro;
using UnityEditor;
using UnityEngine;

namespace SasaUtility
{
    public class FontSetterEditor : EditorWindow
    {
        private TMP_FontAsset yourFont; // 使いたいフォントをここにアサインしてください

        [MenuItem("SasaUtility/Set Font for TextMeshProUGUI")]
        private static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(FontSetterEditor));
            window.titleContent = new GUIContent("Font Setter");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Select the Font for TextMeshProUGUI", EditorStyles.boldLabel);
            yourFont = EditorGUILayout.ObjectField("Font Asset", yourFont, typeof(TMP_FontAsset), false) as TMP_FontAsset;

            if (GUILayout.Button("Set Font for All TextMeshProUGUI"))
            {
                SetFontForAllTextComponents();
            }
        }

        private void SetFontForAllTextComponents()
        {
            TMP_Text[] textComponents = FindObjectsOfType<TMP_Text>();

            foreach (TMP_Text textComponent in textComponents)
            {
                Undo.RecordObject(textComponent, "Font Change");
                textComponent.font = yourFont;
                EditorUtility.SetDirty(textComponent);
            }
        }
    }
}