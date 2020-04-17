using UnityEngine;
using System.Collections;
using System;

public class Teleport : MonoBehaviour
{
    [SerializeField] public Transform _endPoint;
    [SerializeField] float _teleportTime;

    public void ShouldTeleport(Transform player, Action callback)
    {
        StartCoroutine(Co_Teleport(player, callback));
    }

    private IEnumerator Co_Teleport(Transform player, Action callback)
    {
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(_teleportTime);
        player.gameObject.SetActive(true);
        player.position = _endPoint.position;
        callback();
    }
}
