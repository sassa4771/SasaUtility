using UnityEngine;
using UnityEngine.UI;

public class NetworkStatusChecker : MonoBehaviour
{
    public GameObject NetworkErrorPopUp;

    void Start()
    {
        CheckNetworkStatus(); // ゲーム起動時にネットワーク接続の状態を確認
    }

    public void CheckNetworkStatus()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // ネットワークに接続されていない場合
            NetworkErrorPopUp.SetActive(true);
            Debug.Log("ネットワークに接続されていません。");
        }
        else
        {
            NetworkErrorPopUp.SetActive(false);
            Debug.Log("ネットワークに接続されています。");
        }
    }
}
