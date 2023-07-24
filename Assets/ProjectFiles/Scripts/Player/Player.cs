using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ILateStart, IDamageable
{
    public static Player instance;

    public string PlayerName = "Isabella";

    GameManager manager;
    [SerializeField] HotbarHighlight currentItem;
    public Interactor interactor { get; private set; }


    public InputControls controls { get; private set; }

    public int Health { get { return Health; } set { Health += value; } }
    public int MaxHealth { get { return MaxHealth; } set { MaxHealth = value; } }

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
        //controls.Player.Use.performed += use => UseItem();

        controls.Player.HotbarQuick.performed += HotbarHighlight => HotbarSelection(controls.Player.HotbarQuick.ReadValue<float>());

        controls.Enable();
    }


    public SO_Inventory inventory;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public delegate void OnHotbarScroll(Vector2 scrollDelta);
    public OnItemChanged onHotbarScrollCallback;

    public delegate void OnHotbarQuickSelect(float slotPos);
    public OnHotbarQuickSelect onHotbarQuickSelect;

    public delegate void OnInventoryToggle();
    public OnItemChanged onInventoryToggleCallback;

    public delegate void OnInventoryCreated();
    public OnInventoryCreated onInventoryCreatedCallback;

    void Start()
    {
        if (currentItem == null)
        {
            Debug.LogWarning("No Hotbar is defined.");
        }
        manager = GameManager.Instance;

        interactor = GetComponent<Interactor>();

        StartCoroutine(LateStart());
    }


    public IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        CreateInventory();
    }

    void CreateInventory()
    {
        int inventorySize = inventory.inventorySize;

        for(int i = 0; i < inventorySize; i++)
        {
            inventory.inventoryItems.Add(new InventorySlot(null, 0, i));
        }

        onInventoryCreatedCallback?.Invoke();
    }

    void HotbarSelection(float value)
    {
        onHotbarQuickSelect?.Invoke(value);
    }

    void Scroll()
    {
        onHotbarScrollCallback?.Invoke();
    }

    void InventoryInteraction()
    {
        onInventoryToggleCallback?.Invoke();
    }

    bool UseItem()
    {
        bool _itemUsed = false;
        SO_Item _curItem = GetCurrentItem();
        if (manager.GetCurrentState() != GameManager.GameState.Normal)
        {
            return false;
        }
        if (_curItem == null)
        {
            return false;
        }
        
        _itemUsed = _curItem.CanBeUsed(interactor.GetGameObjects().Item2);

        return _itemUsed;

    }

    void Interacting()
    {
        if (interactor.GetOverlaps().Item1)
        {
            interactor.GetOverlaps().Item2.Interact(interactor);
        }
    }

    //LOGIC INTO INTERACTIONS

    public SO_Item GetCurrentItem()
    {
        if(currentItem.GetCurrentlyEquippedItem() == null)
        {
            return null;
        }
        return currentItem.GetCurrentlyEquippedItem();
    }


    public void TakeDamage(int dmg)
    {
        Health -= dmg;
        if(Health <= 0)
        {
            Debug.Log($"{gameObject.name} has died.");
        }
    }

    public void Heal(int healHP)
    {
        Health += healHP;
        if(Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var item = hit.gameObject.GetComponent<ItemHolder>();
        if (item)
        {

            bool wasItemAdded = inventory.AddItem(item.item, 1);
            if (wasItemAdded)
            {
                
                onItemChangedCallback?.Invoke();
                
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
