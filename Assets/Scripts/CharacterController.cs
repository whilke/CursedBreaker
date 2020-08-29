using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D attackCollider;

    private bool moving = false;
    private bool attack;

    public void OnHitCallback()
    {
//         var gm = GameObject.FindObjectOfType<CharacterSpawner>();
//         var tree = gm.quadTree;
//         var hits = tree.RetrieveObjectsInArea(new Rect(this.attackCollider.bounds.min, this.attackCollider.bounds.size));
// 
//         var hit = hits.Where(x => x.infected)
//             .DefaultIfEmpty()
//             .OrderBy(x =>  (x.position - (Vector2)this.transform.position).sqrMagnitude)
//             .FirstOrDefault();
//         if (hit.renderer != null)
//         {
//             gm.HitCharacter(hit);
//         }
    }

    private void Update()
    {
        this.moving = false;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.MoveLeft();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.MoveRight();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            this.MoveUp();
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            this.MoveDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.StartAttack();
        }

        this.UpdateAnimationProperties();

    }

    private void MoveLeft()
    {
        this.moving = true;
        this.transform.localScale = new Vector3(-1,1,1);
        this.transform.Translate(Vector2.left * this.speed * Time.deltaTime, Space.World);
    }

    private void MoveRight()
    {
        this.moving = true;
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.Translate(Vector2.right * this.speed * Time.deltaTime, Space.World);
    }

    private void MoveUp()
    {
        this.moving = true;
        this.transform.Translate(Vector2.up * this.speed * Time.deltaTime, Space.World);
    }

    private void MoveDown()
    {
        this.moving = true;
        this.transform.Translate(Vector2.down * this.speed * Time.deltaTime, Space.World);
    }

    private void UpdateAnimationProperties()
    {
        this.animator.SetBool("Idle", !this.moving);
        this.animator.SetBool("Walk", this.moving);
        if (this.attack)
        {
            this.animator.SetTrigger("Attack");
            this.attack = false;
        }
    }

    public void StartAttack()
    {
        this.attack = true;
    }
}
