using UnityEngine;
using UnityEditor;

public class EditorWindowSample : EditorWindow
{
    private GameObject cubeObject; // Cube�I�u�W�F�N�g�̎Q��
    private Vector3 currentMoveDirection = Vector3.zero; // ���݂̈ړ�����
    private float moveSpeed = 2.0f; // �ړ����x�̏����l

    [MenuItem("Editor/Sample")]
    private static void Create()
    {
        // ����
        EditorWindowSample window = GetWindow<EditorWindowSample>("�T���v��");
    }

    private void OnGUI()
    {
        // EditorWindow�̃T�C�Y�𒲐�
        float windowHeight = 205f; // �E�B���h�E�̍�����K�X����
        float windowWidth = 400f; // �E�B���h�E�̕���K�X����
        this.minSize = new Vector2(windowWidth, windowHeight);

        // Cube�I�u�W�F�N�g��ǂݍ��ރ{�^�� (���F)
        GUI.backgroundColor = Color.yellow;
        // Cube�I�u�W�F�N�g��ǂݍ��ރ{�^��
        if (GUILayout.Button("Inspector���Cube�I�u�W�F�N�g��ǂݍ���"))
        {
            cubeObject = GameObject.Find("Cube"); // Cube�I�u�W�F�N�g���擾

            if (cubeObject != null) Debug.Log("Cube��ǂݍ��݂܂����B����\�ł��B");
            else Debug.LogError("Cube��������܂���BHierarchy���Cube�I�u�W�F�N�g�����݂��邱�Ƃ��m�F���Ă��������B");
        }
        GUI.backgroundColor = Color.white; // �F�����ɖ߂�

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Cube�̈ړ�");
            }

            GUILayout.Space(10); // �X�y�[�X��}��

            //�R���g���[��
            using (new GUILayout.HorizontalScope())
            {
                //XZ�R���g���[��
                using (new GUILayout.VerticalScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��

                        // ��Ɉړ�����{�^��
                        if (GUILayout.RepeatButton("z���v���X����", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.forward;
                        }

                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��

                        // ���Ɉړ�����{�^��
                        if (GUILayout.RepeatButton("x���}�C�i�X����", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.left;
                        }

                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��

                        // �E�Ɉړ�����{�^��
                        if (GUILayout.RepeatButton("x���v���X����", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.right;
                        }

                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��

                        // ���Ɉړ�����{�^��
                        if (GUILayout.RepeatButton("z���}�C�i�X����", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.back;
                        }

                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��
                    }
                }
                //Y�R���g���[��
                using (new GUILayout.VerticalScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��

                        // ��Ɉړ�����{�^��
                        if (GUILayout.RepeatButton("y���v���X����", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.up;
                        }

                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��
                    }
                    GUILayout.Space(20); // �X�y�[�X��}��

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��

                        // ���Ɉړ�����{�^��
                        if (GUILayout.RepeatButton("y���}�C�i�X����", GUILayout.Width(100)))
                        {
                            currentMoveDirection = Vector3.down;
                        }

                        GUILayout.FlexibleSpace(); // �{�^���𒆉��ɔz�u���邽�߂̗]��
                    }
                }
            }

            GUILayout.Space(10); // �X�y�[�X��}��

            using (new GUILayout.VerticalScope())
            {
                // �ړ����x�𒲐�����X���C�_�[
                GUILayout.Label("�ړ����x: " + moveSpeed.ToString("F2"));
                moveSpeed = GUILayout.HorizontalSlider(moveSpeed, 1.0f, 10.0f); // �ŏ��l�ƍő�l�𒲐�
                GUILayout.Space(20); // �X�y�[�X��}��
            }

            using (new GUILayout.HorizontalScope(GUI.skin.box))
            {
                GUILayout.Label("�ړ��̒�~�ƃ��Z�b�g�F");

                // Cube�I�u�W�F�N�g��ǂݍ��ރ{�^�� (���F)
                GUI.backgroundColor = Color.red;
                // �ړ����~����{�^��
                if (GUILayout.Button("��~"))
                {
                    currentMoveDirection = Vector3.zero;
                }

                // Cube�I�u�W�F�N�g��ǂݍ��ރ{�^�� (���F)
                GUI.backgroundColor = Color.blue;

                GUILayout.Space(10); // �X�y�[�X��}��

                if (GUILayout.Button("���Z�b�g"))
                {
                    ResetCubePosition();
                }
                // Cube�I�u�W�F�N�g��ǂݍ��ރ{�^�� (���F)
                GUI.backgroundColor = Color.white;
            }
        }

    }

    private void Update()
    {
        if (cubeObject != null && currentMoveDirection != Vector3.zero)
        {
            cubeObject.transform.Translate(currentMoveDirection * Time.deltaTime * moveSpeed); // �ړ����x��K�p
        }
    }

    private void ResetCubePosition()
    {
        if (cubeObject != null)
        {
            cubeObject.transform.position = Vector3.zero; // Cube�̈ʒu��Vector3.zero�Ƀ��Z�b�g
        }
        else
        {
            Debug.LogError("Cube�I�u�W�F�N�g��������܂���B");
        }
    }
}
