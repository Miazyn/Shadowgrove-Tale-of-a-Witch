using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlueprintGhostObject : MonoBehaviour
{
    RaycastHit hit;
    Vector3 movePoint;
    [SerializeField] SO_Structures structureSO;
    public GameObject prefab;
    bool delayedStart = false;

    [SerializeField] Renderer renderer;
    [SerializeField] Material validMat;
    [SerializeField] Material invalidMat;

    GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;
        manager.ChangeGameState(GameManager.GameState.BuildMode);

        GetMousePos();
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);
        delayedStart = true;
    }
    
    private void Update()
    {
        GetMousePos();

        if (CanPlaceItem())
        {
            renderer.material = validMat;
        }
        else
        {
            renderer.material = invalidMat;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            if (delayedStart)
            {
                Instantiate(prefab, transform.position, transform.rotation);

                Player.instance.inventory.RemoveItem(structureSO);

                manager.ChangeGameState(GameManager.GameState.Normal);
                Destroy(gameObject);
            }
        }
        if (Mouse.current.rightButton.isPressed)
        {
            manager.ChangeGameState(GameManager.GameState.Normal);
            Destroy(gameObject);
        }
    }

    void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit, 5000f, (1<<8) ))
        {
            transform.position = hit.point;
            transform.position = transform.position + (Vector3.up * prefab.transform.position.y);
        }
    }

    bool CanPlaceItem()
    {
        return true;
    }
}
