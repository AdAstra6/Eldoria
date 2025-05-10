using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectItemPanel : MonoBehaviour
{
    public static CollectItemPanel Instance { get; private set; }

    [SerializeField] private GameObject collectItemPanel;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button ignoreButton;
    [SerializeField] private Button collectButton;

    private ItemType ItemType;
    private int quantity;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        HideCollectItemPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowCollectItemPanel(ItemType item , int quantity)
    {
        collectItemPanel.SetActive(true);
        this.itemName.text = ItemsTypeExtensioin.GetName(item);
        this.quantityText.text = quantity.ToString();
        this.itemIcon.sprite = ItemsTypeExtensioin.GetIcon(item);
        this.ItemType = item;
        this.quantity = quantity;
    }

    public void HideCollectItemPanel()
    {
        collectItemPanel.SetActive(false);
    }
    public void OnIgnoreButtonClicked()
    {
        HideCollectItemPanel();
    }
    public void OnCollidableClicked()
    {
        GameplayManager.Instance.AddItemToPlayer(this.ItemType, quantity);
    }

}
