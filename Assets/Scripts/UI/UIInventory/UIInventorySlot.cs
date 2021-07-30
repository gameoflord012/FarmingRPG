using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Transform parentItem;
    private GameObject draggeditem;
    
    [SerializeField] Image inventorySlotHighlight;
    [SerializeField] Image inventorySlotImage;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Sprite emptySlotSprite;

    [SerializeField] UIInventoryBar inventoryBar = null;
    [SerializeField] Item itemPrefab;

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    private void Awake()
    {
        ClearInventorySlot();
        mainCamera = Camera.main;
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }

    public void UpdateInventorySlot(ItemDetails itemDetails, int itemQuantity)
    {
        this.itemDetails = itemDetails;
        this.itemQuantity = itemQuantity;
        inventorySlotImage.sprite = itemDetails.itemSprite;
        textMeshProUGUI.text = itemQuantity.ToString();
    }

    public void ClearInventorySlot()
    {
        itemDetails = null;
        itemQuantity = 0;
        textMeshProUGUI.text = "";
        inventorySlotImage.sprite = emptySlotSprite;
    }    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails == null) return;
        Player.Instance.DisablePlayerInputAndResetMovement();
        draggeditem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);
        Image draggedItemImage = draggeditem.GetComponentInChildren<Image>();
        draggedItemImage.sprite = inventorySlotImage.sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggeditem == null) return;
        draggeditem.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggeditem == null) return;
        Destroy(draggeditem);

        if(eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventoryBar>() != null)
        {

        }
        else
        {
            if(itemDetails.canBeDropped)
            {
                DropSelectedItemAtMousePosition();
            }
        }

        Player.Instance.EnablePlayerInput();
    }    

    private void DropSelectedItemAtMousePosition()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z)
        );

        Item item = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
        item.Init(itemDetails.itemCode);

        InventoryManager.Instance.RemoveItem(InventoryLocation.player, itemDetails.itemCode);
    }
}
