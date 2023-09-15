using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Troop : MonoBehaviour
{
    public List<Transform> potetialTargets = new List<Transform>();
    public Transform currentTarget;
    public float distanceToTarget;
    public float speed;
    public bool mouseOver;
    public Animator pAnim;
    public bool moveOveride = false;
    public Vector3 moveToPosGlobal;
    public float initX;
    public Vector3 rotation;
    public float attackSpeed;
    public int attackPower;
    private bool isAttacking;
    public List<targeting> targetSystem = new List<targeting>();
    public int ID;
    public int maxHealth;
    public int health;
    public Slider healthSlider;

    [System.Serializable]
    public class targeting
    {
        public string name;
        public bool enabled;
    }

    void Start()
    {
        StartCoroutine("target");
        StartCoroutine("playerInterction");
        StartCoroutine("checkDeath");
        initX = transform.position.x;
        targeting first = new targeting();
        first.name = "First";
        targeting last = new targeting();
        last.name = "Last";
        if (Random.Range(0, 2) == 1)
        {
            first.enabled = true;
            last.enabled = false;
        }
        else
        {
            first.enabled = false;
            last.enabled = true;
        }
        targetSystem.Add(first);
        targetSystem.Add(last);
        health = maxHealth;
    }

    IEnumerator playerInterction()
    {
        while (true)
        {
            if (mouseOver)
            {
                GameObject.Find("Canvas").GetComponent<UIManagement>().mouseOverTroop = true;
                GameObject.Find("Canvas").GetComponent<UIManagement>().troopID = ID;
                GameObject.Find("Canvas").GetComponent<UIManagement>().healthText.text =
                    health.ToString() + "/" + maxHealth.ToString();
                GameObject.Find("Canvas").GetComponent<UIManagement>().speedText.text = (
                    speed * 100
                ).ToString();
                GameObject.Find("Canvas").GetComponent<UIManagement>().attackPowerText.text =
                    attackPower.ToString();
                if (Input.GetMouseButtonDown(0))
                {
                    yield return new WaitForSeconds(0.1f);
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = 10.0f;
                    Vector3 moveToPos = Camera.main.ScreenToWorldPoint(mousePos);
                    moveToPosGlobal = moveToPos;
                    while (transform.position != moveToPos)
                    {
                        print(moveToPos.x - transform.position.x);
                        moveOveride = true;
                        pAnim.SetBool("IsMoving", true);
                        transform.position = Vector3.MoveTowards(
                            transform.position,
                            moveToPos,
                            speed * 1.5f
                        );
                        yield return new WaitForSeconds(0.01f);
                    }
                    moveOveride = false;
                }
            }
            else
            {
                GameObject.Find("Canvas").GetComponent<UIManagement>().mouseOverTroop = false;
                GameObject.Find("Canvas").GetComponent<UIManagement>().healthText.text = "";
                GameObject.Find("Canvas").GetComponent<UIManagement>().speedText.text = "";
                GameObject.Find("Canvas").GetComponent<UIManagement>().attackPowerText.text = "";
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator target()
    {
        while (true)
        {
            if (potetialTargets.Count > 0)
            {
                if (targetSystem[0].enabled)
                {
                    currentTarget = potetialTargets[0];
                }
                else
                {
                    currentTarget = potetialTargets[potetialTargets.Count - 1];
                }
            }
            else if (currentTarget != null && currentTarget.GetComponent<Enemy>().health < 1)
            {
                currentTarget = null;
            }
            else
            {
                currentTarget = null;
            }

            if (currentTarget != null)
            {
                distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
                pAnim.SetBool("IsMoving", true);
            }
            else
            {
                distanceToTarget = 0;
                if (!moveOveride)
                {
                    pAnim.SetBool("IsMoving", false);
                }
            }

            if (distanceToTarget > 2f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    currentTarget.position,
                    speed
                );
                pAnim.SetBool("IsMoving", true);
                if (!isAttacking)
                {
                    StartCoroutine("attack");
                }
            }
            else if (!moveOveride)
            {
                pAnim.SetBool("IsMoving", false);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator attack()
    {
        while (
            currentTarget != null
            && currentTarget.GetComponent<Enemy>().health > 0
            && !pAnim.GetBool("Death")
        )
        {
            isAttacking = true;
            pAnim.SetTrigger("Attack");
            currentTarget.GetComponent<Enemy>().health -= attackPower;
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
                pAnim.SetBool("Death", true);
                yield return new WaitForSeconds(3);
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            if ((currentTarget.position - transform.position).normalized.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else if (moveOveride)
        {
            if ((moveToPosGlobal - transform.position).normalized.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("scatter");
        }
        healthSlider.value = health;
    }

    IEnumerator scatter()
    {
        moveToPosGlobal = new Vector3(Random.Range(-14f, 13f), Random.Range(-9f, 8f), 0);
        while (transform.position != moveToPosGlobal)
        {
            moveOveride = true;
            pAnim.SetBool("IsMoving", true);
            transform.position = Vector3.MoveTowards(
                transform.position,
                moveToPosGlobal,
                speed * 1.5f
            );
            yield return new WaitForSeconds(0.01f);
        }
        pAnim.SetBool("IsMoving", false);
        moveOveride = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name);
        if (other.gameObject.CompareTag("enemy"))
        {
            potetialTargets.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        potetialTargets.Remove(other.gameObject.transform);
    }
}
