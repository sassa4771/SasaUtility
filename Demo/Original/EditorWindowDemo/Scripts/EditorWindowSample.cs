using UnityEngine;
using UnityEditor;

public class EditorWindowSample : EditorWindow
{
    private GameObject cubeObject; // Cubeオブジェクトの参照
    private Vector3 currentMoveDirection = Vector3.zero; // 現在の移動方向
    private float moveSpeed = 2.0f; // 移動速度の初期値

    [MenuItem("SassaUtility/Demo/EditorWindowSample")]
    private static void Create()
    {
        // 生成
        EditorWindowSample window = GetWindow<EditorWindowSample>("EditorWindowSample");
    }

    private void OnGUI()
    {
        // EditorWindowのサイズを調整
        float windowHeight = 220f; // ウィンドウの高さを適宜調整
        float windowWidth = 400f; // ウィンドウの幅を適宜調整
        this.minSize = new Vector2(windowWidth, windowHeight);

        // 選択中のオブジェクトを表示
        GUILayout.Label("選択中のオブジェクト: " + (cubeObject != null ? cubeObject.name : "なし"));

        // Cubeオブジェクトを読み込むボタン (黄色)
        GUI.backgroundColor = Color.yellow;
        // 選択中のオブジェクトをHierarchyから選択するボタン
        if (GUILayout.Button("Hierarchyから選択"))
        {
            cubeObject = (GameObject)Selection.activeObject;
        }
        GUI.backgroundColor = Color.white; // 色を元に戻す

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Cubeの移動");
            }

            GUILayout.Space(10); // スペースを挿入

            //コントローラ
            using (new GUILayout.HorizontalScope())
            {
                //XZコントローラ
                using (new GUILayout.VerticalScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白

                        // 上に移動するボタン
                        if (GUILayout.RepeatButton("z軸プラス方向", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.forward;
                        }

                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白

                        // 左に移動するボタン
                        if (GUILayout.RepeatButton("x軸マイナス方向", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.left;
                        }

                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白

                        // 右に移動するボタン
                        if (GUILayout.RepeatButton("x軸プラス方向", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.right;
                        }

                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白

                        // 下に移動するボタン
                        if (GUILayout.RepeatButton("z軸マイナス方向", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.back;
                        }

                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白
                    }
                }
                //Yコントローラ
                using (new GUILayout.VerticalScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白

                        // 上に移動するボタン
                        if (GUILayout.RepeatButton("y軸プラス方向", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.up;
                        }

                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白
                    }
                    GUILayout.Space(20); // スペースを挿入

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白

                        // 下に移動するボタン
                        if (GUILayout.RepeatButton("y軸マイナス方向", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.down;
                        }

                        GUILayout.FlexibleSpace(); // ボタンを中央に配置するための余白
                    }
                }
            }

            GUILayout.Space(10); // スペースを挿入

            using (new GUILayout.VerticalScope())
            {
                // 移動速度を調整するスライダー
                GUILayout.Label("移動速度: " + moveSpeed.ToString("F2"));
                moveSpeed = GUILayout.HorizontalSlider(moveSpeed, 1.0f, 10.0f); // 最小値と最大値を調整
                GUILayout.Space(20); // スペースを挿入
            }

            using (new GUILayout.HorizontalScope(GUI.skin.box))
            {
                GUILayout.Label("移動の停止とリセット：");

                // Cubeオブジェクトを読み込むボタン (黄色)
                GUI.backgroundColor = Color.red;
                // 移動を停止するボタン
                if (GUILayout.Button("停止"))
                {
                    currentMoveDirection = Vector3.zero;
                }

                // Cubeオブジェクトを読み込むボタン (黄色)
                GUI.backgroundColor = Color.blue;

                GUILayout.Space(10); // スペースを挿入

                if (GUILayout.Button("リセット"))
                {
                    ResetCubePosition();
                }
                // Cubeオブジェクトを読み込むボタン (黄色)
                GUI.backgroundColor = Color.white;
            }
        }

    }

    private void Update()
    {
        if (cubeObject != null && currentMoveDirection != Vector3.zero)
        {
            cubeObject.transform.Translate(currentMoveDirection * Time.deltaTime * moveSpeed); // 移動速度を適用
        }
    }

    private void ResetCubePosition()
    {
        if (cubeObject != null)
        {
            cubeObject.transform.position = Vector3.zero; // Cubeの位置をVector3.zeroにリセット
        }
        else
        {
            Debug.LogError("Cubeオブジェクトが見つかりません。");
        }
    }
}
