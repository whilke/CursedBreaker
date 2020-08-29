using System;
using System.Linq;
using UnityEngine;

namespace enBask
{
    public class ItemPanel : MonoBehaviour
    {
        public ItemUI Prefab;
        public ItemDatabase ItemDatbase;

        public int PanelSize = 5;


        private ItemUI[] items;

        private void Awake()
        {
            this.SetupQueue();
        }

        private void SetupQueue()
        {
            this.items = new ItemUI[this.PanelSize];

            Enumerable.Range(0, this.PanelSize).ToList().ForEach(x => this.AddNewItem(x));
        }

        public ItemData PickItem()
        {
            var items = this.ItemDatbase.Items.OrderBy(x => x.Chance).ToArray();
            var chanceTotal = items.Sum(x => x.Chance);

            var r = UnityEngine.Random.Range(0f, chanceTotal);
            float total = 0f;
            foreach (var item in items)
            {
                if (r <= item.Chance + total)
                {
                    return item;
                }
                total += item.Chance;
            }

            return null;
        }

        public void AddNewItem(int slot, bool setCooldown = false)
        {
            var item = this.PickItem();
            var go = Instantiate(this.Prefab, this.transform, false);
            go.SetItem(item);

            if (setCooldown)
            {
                go.EnableCooldown();
            }

            this.items[slot] = go;
            go.transform.SetSiblingIndex(slot);
        }

        public void ReplaceItem(ItemUI item)
        {
            var slot = this.GetSlot(item);
            var child = this.transform.GetChild(slot);
            this.transform.GetChild(1).GetComponent<ItemUI>().Button.interactable = true;
            this.AddNewItem(slot, true);
            Destroy(child.gameObject);
        }

        private int GetSlot(ItemUI slot)
        {
            for (var i = 0; i < this.transform.childCount; ++i)
            {
                if (this.transform.GetChild(i).GetComponent<ItemUI>() == slot)
                    return i;
            }

            return -1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.PlaceItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.PlaceItem(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                this.PlaceItem(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                this.PlaceItem(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                this.PlaceItem(4);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var mgr = GameObject.FindObjectOfType<ItemManager>();
                if (!mgr.CanPlaceItem)
                {
                    mgr.CancelPlaceItem();
                }
            }
        }

        private void PlaceItem(int slot)
        {
            var mgr = GameObject.FindObjectOfType<ItemManager>();
            if (!mgr.CanPlaceItem)
            {
                mgr.CancelPlaceItem();
            }

            ItemUI item = this.transform.GetChild(slot).GetComponent<ItemUI>();
            if (item.isCoolingDown) return;

            mgr.StartPlaceItem(item.GetItem(), item);

        }
    }
}