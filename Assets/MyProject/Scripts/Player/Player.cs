using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] HotbarHighlight currentItem;
    public Interactor interactor { get; private set; }


    public InputControls controls { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
        controls = new InputControls();
        controls.Player.Interact.performed += interaction => Interacting();
        controls.Player.Inventory.performed += inventory => InventoryInteraction();
        controls.Player.Scroll.performed += scroll => Scroll();
    }


    public SO_Inventory inventory;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public delegate void OnHotbarScroll(Vector2 scrollDelta);
    public OnItemChanged onHotbarScrollCallback;

    public delegate void OnInventoryToggle();
    public OnItemChanged onInventoryToggleCallback;

    void Start()
    {
        if (currentItem == null)
        {
            Debug.LogWarning("No Hotbar is defined.");
        }
        //Grab cuz its on player
        interactor = GetComponent<Interactor>();
    }

    void Scroll()
    {
        onHotbarScrollCallback?.Invoke();
    }

    void InventoryInteraction()
    {
        onInventoryToggleCallback?.Invoke();
    }

    void Interacting()
    {
        Debug.Log("Interaction has been clicked");
        if (interactor.GetOverlaps().Item1)
        {
            interactor.GetOverlaps().Item2.Interact(interactor);
        }
    }

    //LOGIC INTO INTERACTIONS

    public SO_Item GetCurrentItem()
    {
        return currentItem.GetCurrentlyEquippedItem();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var item = hit.gameObject.GetComponent<ItemHolder>();
        if (item)
        {
            Debug.Log($"Adding {item} to inventory");
            bool wasItemAdded = inventory.AddItem(item.item, 1);
            if (wasItemAdded)
            {
                if (onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }
                else
                {
                    Debug.LogError("No methods subscribed");
                }
                Destroy(hit.gameObject);
            }
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnApplicationQuit()
    {
        //B4 clear, save inventory
        if (inventory.inventoryItems.Count > 0)
        {
            inventory.inventoryItems.Clear();
        }
    }
}
