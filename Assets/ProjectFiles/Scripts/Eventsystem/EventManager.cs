using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static UnityEvent OnDialogueStart = new UnityEvent();
    public static UnityEvent OnDialogueEnd = new UnityEvent();
}
