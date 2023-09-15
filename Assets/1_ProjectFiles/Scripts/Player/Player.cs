using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController), typeof(Interactor), typeof(CharacterController))]
public class Player : MonoBehaviour, ILateStart, IDamageable
{
    public static Player instance;

    public string PlayerName = "Isabella";

    public int Money { get; private set; }

    [SerializeField] int StarterMoney = 1000;

    GameManager manager;

    [SerializeField] HotbarHighlight currentItem;

    [Header("Tools")]
    [SerializeField] GameObject hammer;
    [SerializeField] GameObject hoe;
    [SerializeField] GameObject axe;
    [SerializeField] GameObject wateringCan;
    [SerializeField] GameObject wand;

    public Interactor interactor { get; private set; }


    private IInteractable lastInteraction = null;

    [Header("Read ONLY")]
    public List<FriendshipStats> allFriendships = new List<FriendshipStats>();

    public InputControls controls { get; private set; }

    public bool firstStart = true;

    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    private int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public class FriendshipStats
    {
        public int friendShipLevel;
        public SO_NPC npc;

        public int talkPoints = 10;

        public FriendshipStats(int _friendLvl, SO_NPC _npc)
        {
            friendShipLevel = _friendLvl;
            npc = _npc;
        }
    }

    public int Endurance { get; private set; }
    public int MaxEndurance { get; private set; }


    public Vector3 nonInstancePos;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else if(this != instance)
        //{
        //    instance.GetComponent<FadingObjectBlockingObject>().Camera = this.GetComponent<FadingObjectBlockingObject>().Camera;
        //    instance.GetComponent<FadingObjectBlockingObject>().Camera.GetComponent<CameraFollow>().characterToFollow = instance.gameObject;
        //    instance.currentItem = this.currentItem;


        //    Debug.Log("I am not the instance");

        //    Destroy(gameObject);
        //}

        controls = new InputControls();
        controls.Player.Interact.performed += interaction => Interacting();
        controls.Player.Inventory.performed += inventory => InventoryInteraction();
        controls.Player.Scroll.performed += scroll => Scroll();
        controls.Player.Cancel.performed += cancel => CancelAction();
        controls.Player.Use.performed += use => UseItem();

        controls.Player.HotbarQuick.performed += HotbarHighlight => HotbarSelection(controls.Player.HotbarQuick.ReadValue<float>());

        controls.Enable();


        MaxEndurance = 100;
        Endurance = MaxEndurance;

        MaxHealth = 100;
        Health = MaxHealth;
    }


    public SO_Inventory inventory;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public delegate void OnHotbarScroll(Vector2 scrollDelta);
    public OnItemChanged onHotbarScrollCallback;

    public delegate void OnHotbarQuickSelect(float slotPos);
    public OnHotbarQuickSelect onHotbarQuickSelect;

    public delegate void OnInventoryCreated();
    public OnInventoryCreated onInventoryCreatedCallback;

    public delegate void OnPlayerMoneyChanged(int curMoney);
    public OnPlayerMoneyChanged onPlayerMoneyChangedCallback;

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        if (this == instance)
        {

            controls.Disable();
            if (EventManager.OnDayChanged != null)
            {
                EventManager.OnDayChanged.RemoveListener(ResetEndurance);
                EventManager.OnDayChanged.RemoveListener(ResetHealth);
            }
        }

    }

    private void OnDestroy()
    {
    }

    void Start()
    {

        if (currentItem == null)
        {
            Debug.LogWarning("No Hotbar is defined.");
        }
        manager = GameManager.Instance;

        interactor = GetComponent<Interactor>();


        EventManager.OnDayChanged.AddListener(ResetEndurance);
        EventManager.OnDayChanged.AddListener(ResetHealth);

        onPlayerMoneyChangedCallback?.Invoke(Money);

        StartCoroutine(LateStart());
    }

    public IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        CreateInventory();

        if (firstStart)
        {
            AddMoney(StarterMoney);
            firstStart = false;
        }

        onPlayerMoneyChangedCallback?.Invoke(Money);
    }

    private void Update()
    {
        bool isOverlapping = interactor.GetOverlaps().Item1;

        if (isOverlapping)
        {
            if(lastInteraction == null)
            {
                lastInteraction = interactor.GetOverlaps().Item2;
                lastInteraction.ShowInteractPrompt();
            }
            if(interactor.GetOverlaps().Item2 != lastInteraction)
            {
                lastInteraction.HideInteractPrompt();

                lastInteraction = interactor.GetOverlaps().Item2;
                lastInteraction.ShowInteractPrompt();
            }
        }
        else
        {
            if(lastInteraction != null)
            {
                lastInteraction.HideInteractPrompt();
                lastInteraction = null;
            }
        }
    }

    public bool CheckPlayerStat(NpcScript _npc)
    {
        if (_npc.interacted) return true;

        SO_NPC curNpc = _npc._NPC;

        foreach(FriendshipStats npc in allFriendships)
        {
            if(npc.npc.NpcName == curNpc.NpcName)
            {
                npc.friendShipLevel += npc.talkPoints;
                return true;
            }
        }

        FriendshipStats addedFriend = new FriendshipStats(0, curNpc);

        Debug.Log("New NPC");
        allFriendships.Add(addedFriend);
        return false;
    }

    public int CheckFriendshipLevel(SO_NPC _npc)
    {
        if (_npc == null) return 0;

        foreach(FriendshipStats npc in allFriendships)
        {
            if (npc.npc.NpcName == _npc.NpcName)
            {
                return npc.friendShipLevel;
            }
        }

        FriendshipStats addedFriend = new FriendshipStats(0, _npc);

        allFriendships.Add(addedFriend);
        return 0;
    }

    public bool CanBuy(int buyPrice)
    {
        if (Money >= buyPrice)
        {
            return true;
        }

        return false;
    }

    public void SetMoney(int _money)
    {
        Money = _money;
        onPlayerMoneyChangedCallback?.Invoke(Money);
    }

    public void SetMoneyAmount(int _addedMoney)
    {
        Money = Money + _addedMoney <= 0 ? 0 : Money + _addedMoney;
        onPlayerMoneyChangedCallback?.Invoke(Money);
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

    private void AddMoney(int _addedMoney)
    {
        Money = Money + _addedMoney <= 0 ? 0 : Money + _addedMoney;
        onPlayerMoneyChangedCallback?.Invoke(Money);
    }


    private void CancelAction()
    {
        manager.onMenuClosedCallback?.Invoke();
    }

    void HotbarSelection(float value)
    {
        onHotbarQuickSelect?.Invoke(value);

    }

    void Scroll()
    {
        onHotbarScrollCallback?.Invoke();

    }

    public void UseTool(GameObject _objectToLookAt)
    {
        if (_objectToLookAt != null)
        {
            gameObject.transform.LookAt(_objectToLookAt.transform);
        }

        if (!IsTool())
        {
            return;
        }
        //Calls On PlayerController for Anim
        gameObject.GetComponent<PlayerController>().GetToolAnimation(GetCurrentItem().ItemName);
    }

    public void EquipTool()
    {
        if (!IsTool())
        {
            wateringCan.SetActive(false);
            axe.SetActive(false);
            hammer.SetActive(false);
            hoe.SetActive(false);
            wand.SetActive(false);

            return;
        }

        string itemName = GetCurrentItem().ItemName;

        if(itemName.Contains("Wateringcan"))
        {
            wateringCan.SetActive(true);

            axe.SetActive(false);
            hammer.SetActive(false);
            hoe.SetActive(false);
            wand.SetActive(false);
        }
        else if (itemName.Contains("Axe"))
        {
            wateringCan.SetActive(false);

            axe.SetActive(true);
            hammer.SetActive(false);
            hoe.SetActive(false);
            wand.SetActive(false);
        }
        else if (itemName.Contains("Hammer"))
        {
            wateringCan.SetActive(false);

            axe.SetActive(false);
            hammer.SetActive(true);
            hoe.SetActive(false);
            wand.SetActive(false);
        }
        else if (itemName.Contains("Garden hoe"))
        {
            wateringCan.SetActive(false);

            axe.SetActive(false);
            hammer.SetActive(false);
            hoe.SetActive(true);
            wand.SetActive(false);
        }
        else if (itemName.Contains("Wand"))
        {
            wateringCan.SetActive(false);
            axe.SetActive(false);
            hammer.SetActive(false);
            hoe.SetActive(false);
            wand.SetActive(true);
        }
    }

    public bool IsTool()
    {
        if(GetCurrentItem() as SO_Tools == null)
        {
            return false;
        }
        return true;
    }

    void InventoryInteraction()
    {
        manager.onAnyMenuToggleCallback?.Invoke(UIController.Menu.Inventory);
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

    public void SetMaxHealth(int newValue)
    {
        MaxHealth = newValue;
    }

    public void SetEndurance(int value)
    {
        Endurance = value > MaxEndurance ? MaxEndurance : value;
    }

    public void SetHealth(int value)
    {
        Health = value > MaxHealth ? MaxHealth : value;
    }

    public void ResetEndurance()
    {
        Endurance = MaxEndurance;
        manager.onPlayerEnduranceChangeCallback?.Invoke(MaxEndurance, Endurance);
    }

    public void SetMaxEndurance(int newValue)
    {
        MaxEndurance = newValue;
    }

    public void EnduranceChanged(int endurance)
    {
        Endurance = Mathf.Clamp(Endurance + endurance, 0, MaxEndurance);

        manager.onPlayerEnduranceChangeCallback?.Invoke(MaxEndurance, Endurance);
    }

    public void ResetHealth()
    {
        Health = MaxHealth;

        manager.onPlayerHealthChangeCallback?.Invoke(MaxHealth, Health);
    }

    public void TakeDamage(int dmg)
    {
        Health -= dmg;
        if(Health <= 0)
        {
            Debug.Log($"{gameObject.name} has died.");
        }
        manager.onPlayerHealthChangeCallback?.Invoke(MaxHealth, Health);
    }

    public void Heal(int healHP)
    {
        Health += healHP;
        if(Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        manager.onPlayerHealthChangeCallback?.Invoke(MaxHealth, Health);
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //var item = hit.gameObject.GetComponent<ItemHolder>();
        //if (item)
        //{

        //    bool wasItemAdded = inventory.AddItem(item.item, 1);
        //    if (wasItemAdded)
        //    {
                
        //        onItemChangedCallback?.Invoke();
                
        //        Destroy(hit.gameObject);
        //    }
        //}
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
