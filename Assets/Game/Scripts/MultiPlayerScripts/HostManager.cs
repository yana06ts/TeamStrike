using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using Photon.Realtime;

public class HostManager : MonoBehaviourPunCallbacks
{
    public static HostManager instance;

    public GameObject hostLeftPanel;  // панель с сообщением
    public Text hostLeftText;         // текст сообщения

    void Awake()
    {
        instance = this;
    }

    // Вызывается кнопкой "Выйти" у MasterClient в меню паузы
    public void MasterClientLeave()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Сначала уведомляем всех через RPC
            GetComponent<PhotonView>().RPC(
                "RPC_HostClosedRoom", RpcTarget.Others);

            // Небольшая задержка чтобы RPC успел дойти
            StartCoroutine(LeaveAfterDelay());
        }
        else
        {
            // Обычный выход не-хоста
            PhotonNetwork.LeaveRoom();
        }
    }

    IEnumerator LeaveAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("ShowTitleMenu", 1);
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    void RPC_HostClosedRoom()
    {
        Debug.Log("[HostManager] Host closed the room!");
        StartCoroutine(ShowMessageAndExit());
    }

    IEnumerator ShowMessageAndExit()
    {
        // Замораживаем игру и показываем сообщение
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hostLeftPanel.SetActive(true);
        hostLeftText.text = "Хост закрыл комнату.\nВозврат в меню...";

        // WaitForSecondsRealtime потому что timeScale = 0
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        PlayerPrefs.SetInt("ShowTitleMenu", 1);
        PhotonNetwork.Disconnect();
    }
}