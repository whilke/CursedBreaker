using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace enBask
{
    public class ItemManager : MonoBehaviour
    {
        public MouseMover MousePrefab;

        private bool placingItem = false;
        private ItemData item = null;
        private ItemUI itemUI;

        private MouseMover currentMouse;

        public bool CanPlaceItem => !this.placingItem;

        public void StartPlaceItem(ItemData item, ItemUI itemUI)
        {
            this.item = item;
            this.itemUI = itemUI;
            this.placingItem = true;

            if (this.currentMouse != null)
            {
                Destroy(this.currentMouse.gameObject);
            }
            this.currentMouse = Instantiate(this.MousePrefab, null, false);
            this.currentMouse.Sprite.sprite = this.item.Icon;
            this.currentMouse.Sprite.transform.localScale =
                this.item.Activator.GetComponentInChildren<SpriteRenderer>().transform.localScale;
            this.currentMouse.SetOverMouse();
        }

        private void Update()
        {
            this.PlaceItem();
        }

        private void PlaceItem(bool ignoreMouse = false)
        {
            if (!this.placingItem) return;

            if (Input.GetMouseButtonDown(0) || ignoreMouse)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100f))
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 5f);
                    var go = Instantiate(this.item.Activator, null, false);
                    go.transform.position = hit.point;
                }

                this.placingItem = false;
                this.item = null;

                if (this.currentMouse != null)
                {
                    Destroy(this.currentMouse.gameObject);
                    this.currentMouse = null;
                }

                GameObject.FindObjectOfType<ItemPanel>().ReplaceItem(this.itemUI);
            }
        }

        public void StopPlaceItem()
        {
            this.PlaceItem(ignoreMouse: true);
        }

        public void CancelPlaceItem()
        {
            if (this.currentMouse != null)
            {
                Destroy(this.currentMouse.gameObject);
            }

            this.placingItem = false;
            this.item = null;
        }
    }
}