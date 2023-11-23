using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public float rotationSpeed = 50f; // 回転速度を設定する変数

    void Update()
    {
        // Cubeを回転させる
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Vector3.upはY軸方向の単位ベクトル
    }
}
