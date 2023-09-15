using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public List<Vector3> path;
    public float speed;
    private bool isActive;
    public Animator eAnim;
    public float health;
    public Slider healthSlider;
    public List<Transform> troops = new List<Transform>();
    public Transform currentTarget;
    public float distanceToTarget;
    public bool moveOveride;

    [SerializeField]
    private bool isAttacking = false;
    public float attackSpeed;
    public int attackPower;

    void Start()
    {
        path = GameObject.Find("Path").GetComponent<WayPoints>().positions;
        StartCoroutine("followPath");
        StartCoroutine("checkDeath");
        StartCoroutine("target");
    }

    IEnumerator target()
    {
        while (true)
        {
            if (troops.Count > 0)
            {
                print("troops list has at least one item");
                currentTarget = troops[0];
            }
            else if (currentTarget != null && currentTarget.GetComponent<Troop>().health < 1)
            {
                currentTarget = null;
                moveOveride = false;
            }
            else
            {
                currentTarget = null;
            }

            if (currentTarget != null)
            {
                distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
                eAnim.SetBool("IsMoving", true);
            }
            else
            {
                distanceToTarget = 0;
            }

            if (distanceToTarget > 2f)
            {
                moveOveride = true;
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    currentTarget.position,
                    speed
                );
                eAnim.SetBool("IsMoving", true);
            }

            if (!isAttacking && distanceToTarget < 2f && currentTarget != null)
            {
                StartCoroutine("attack");
                print("start attack");
                isAttacking = true;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator attack()
    {
        print("attacked" + " " + currentTarget);
        if (currentTarget.gameObject.GetComponent<Troop>().health > 0)
        {
            currentTarget.gameObject.GetComponent<Troop>().health -= attackPower;
            eAnim.SetTrigger("Attack");
            yield return new WaitForSeconds(attackSpeed);
        }
        isAttacking = false;
    }

    IEnumerator checkDeath()
    {
        while (true)
        {
            if (health < 1)
            {
                path = new List<Vector3>();
                eAnim.SetBool("Death", true);
                yield return new WaitForSeconds(3);
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator followPath()
    {
        isActive = true;

        for (int i = 0; i < path.Count; i++)
        {
            while (transform.position != path[i])
            {
                if (!moveOveride)
                {
                    transform.position = Vector3.MoveTowards(transform.position, path[i], speed);
                }
                yield return new WaitForSeconds(0.01f);
                eAnim.SetBool("IsMoving", true);
                if (!moveOveride)
                {
                    if ((path[i] - transform.position).normalized.x < 0)
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
            }
        }

        isActive = false;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("troop"))
        {
            troops.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.transform == currentTarget)
        {
            troops.Remove(troops[0]);
            currentTarget = null;
            moveOveride = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        print(troops.Count);
        healthSlider.value = health;
        if ((currentTarget.position - transform.position).normalized.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < troops.Count; i++)
        {
            if (troops[i] == null)
            {
                troops.Remove(troops[i]);
            }
        }
    }
}
