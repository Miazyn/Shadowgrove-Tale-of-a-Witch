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

    const float MAXBUILDDISTANCE = 10f;
    const float MAXBUILDDISTANCEYAXIS = 5f;

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

        MoveItemOnGrid();
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);
        delayedStart = true;
    }
    
    private void Update()
    {
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

    void MoveItemOnGrid()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit, 10000, (1 << 8)))
        {
            //var view = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            //var IsOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

            //Debug.Log($"Camera is outside viewport? {IsOutside}");


            Vector3 playerPos = Player.instance.transform.position;
            Vector3 finalPos = grid.GetNearestPointOnGrid(hit.point);

            float xAbs = Mathf.Abs(playerPos.x - finalPos.x);
            float yAbs = Mathf.Abs(playerPos.y - finalPos.y);
            float zAbs = Mathf.Abs(playerPos.z - finalPos.z);

            if(xAbs > MAXBUILDDISTANCE)
            {
                return;
            }
            if (yAbs > MAXBUILDDISTANCE)
            {
                return;
            }
            if (zAbs > MAXBUILDDISTANCE)
            {
                return;
            }


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
