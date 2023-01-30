using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BlueprintGhostObject : MonoBehaviour
{
    RaycastHit hit;
    Vector3 movePoint;
    [SerializeField] SO_Structures structureSO;
    public GameObject prefab;
    bool delayedStart = false;

    [SerializeField] Renderer ghostRenderer;
    [SerializeField] Material validMat;
    [SerializeField] Material invalidMat;

    [SerializeField] float maxBuildDistance = 200f;

    GameManager manager;

    BoxCollider _col;

    Grid grid;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    void Start()
    {

        manager = GameManager.Instance;
        manager.ChangeGameState(GameManager.GameState.BuildMode);

        _col = GetComponent<BoxCollider>();

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
        //GetMousePos();
        MoveItemOnGrid();

        if (CanPlaceItem(transform.position))
        {
            ghostRenderer.material = validMat;
        }
        else
        {
            ghostRenderer.material = invalidMat;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            PlacingItem();
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
        if (Physics.Raycast(ray, out hit, maxBuildDistance, (1<<8) ))
        {
            transform.position = hit.point;
            transform.position = transform.position + (Vector3.up * prefab.transform.position.y);
        }
    }

    void MoveItemOnGrid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit, maxBuildDistance, (1 << 8)))
        {

            var finalPos = grid.GetNearestPointOnGrid(hit.point);
            transform.position = finalPos;
            transform.position = new Vector3(transform.position.x, prefab.transform.position.y, transform.position.z);
        }
    }

    bool CanPlaceItem(Vector3 _centerPos)
    {
        var CheckValidPlacementBox = Physics.OverlapBox(_col.bounds.center, _col.size);
        Terrain _terrain;

        if(CheckValidPlacementBox == null)
        {
            Debug.Log($"Collided with {CheckValidPlacementBox}");
            return true;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        foreach (var item in CheckValidPlacementBox)
        {
            if (item.gameObject.layer != 8 && !item.gameObject.TryGetComponent<Terrain>(out _terrain))
            {
                return false;
            }
            if (!item.gameObject.TryGetComponent<Terrain>(out _terrain))
            {
                return false;
            }
        }

        return true;
    }

    bool PlacingItem()
    {
        if (!CanPlaceItem(transform.position))
        {
            return false;
        }

        if (!delayedStart)
        {
            return false;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        { 
            return false;
        }


        Instantiate(prefab, transform.position, transform.rotation);

        Player.instance.inventory.RemoveItem(structureSO);

        manager.ChangeGameState(GameManager.GameState.Normal);
        Destroy(gameObject);

        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetComponent<BoxCollider>().bounds.center, GetComponent<BoxCollider>().size);

    
    }
}
