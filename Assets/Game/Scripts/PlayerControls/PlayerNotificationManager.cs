using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class PlayerNotificationManager : MonoBehaviourPunCallbacks
{
    public Text notificationText; 

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ShowNotification(otherPlayer.NickName + " покинул игру.");
    }

    void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideAfterDelay(3f));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationText.gameObject.SetActive(false);
    }
}