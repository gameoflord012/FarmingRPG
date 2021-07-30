﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    public List<InventoryItem>[] inventoryLists;
    [HideInInspector] public int[] inventoryListCapacityIntArray;

    [SerializeField] private SO_ItemList itemList = null;

    protected override void Awake()
    {
        base.Awake();

        CreateInventoryLists();

        CreateItemDetailsDictionary();
    }

    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];
        for(int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacityIntArray = new int[(int)InventoryLocation.count];
        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();
        foreach(ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if(itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }
    }

    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);
        Destroy(gameObjectToDelete);
    }

    public void AddItem(InventoryLocation inventoryLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryList, itemCode);

        if(itemPosition == -1)
        {
            AddItemAtPosition(inventoryList, itemCode);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    private int FindItemInInventory(List<InventoryItem> inventoryList, int itemCode)
    {
        for(int i = 0; i < inventoryList.Count; i++)
        {
            if(inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        }
        return -1;
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);

        DebugPrintInventoryList(inventoryList);
    }    

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem inventoryItem = new InventoryItem();
       
        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = inventoryList[itemPosition].itemQuantity + 1;
        inventoryList[itemPosition] = inventoryItem;

        DebugPrintInventoryList(inventoryList);
    }

    private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {
        foreach(InventoryItem inventoryItem in inventoryList)
        {
            Debug.Log($"Description : {GetItemDetails(inventoryItem.itemCode).itemDescription}                 ||               Quantity : {inventoryItem.itemQuantity}");
        }
        Debug.Log("==============================================================");
    }
}
