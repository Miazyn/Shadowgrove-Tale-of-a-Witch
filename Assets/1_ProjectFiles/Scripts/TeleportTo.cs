using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTo : MonoBehaviour
{
    [SerializeField] GameObject targetPos;
    GameObject lastHit;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>().NeedStillness = true;

        other.transform.position = targetPos.transform.position;


        lastHit = other.gameObject;

        StartCoroutine(GracePeriod());
    }

    public IEnumerator GracePeriod()
    {
        yield return new WaitForSeconds(0.2f);
        lastHit.GetComponent<PlayerController>().NeedStillness = false;
    }

}
