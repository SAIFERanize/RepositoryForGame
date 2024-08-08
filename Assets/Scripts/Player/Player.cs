using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float speed;
    public float sprint = 2.0f;
    public Animator animator;
    private bool FacingRight = true;
    public float Health = 100f;
    public int NumOfHearts = 5;
    public Image[] Hearts;
    public Sprite FullHearts;
    public Sprite EmptyHearts;

    void Update()
    {
        float currentSpeed = speed;
        float horizontalMove = Input.GetAxisRaw("Horizontal") * currentSpeed;
        float verticalMove = Input.GetAxisRaw("Vertical") * currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprint;
            horizontalMove = Input.GetAxisRaw("Horizontal") * currentSpeed;
            verticalMove = Input.GetAxisRaw("Vertical") * currentSpeed;
            animator.SetFloat("Run", Mathf.Abs(horizontalMove) + Mathf.Abs(verticalMove));
        }
        else
        {
            animator.SetFloat("Run", 0);
        }

        transform.Translate(new Vector3(horizontalMove, verticalMove, 0) * Time.deltaTime);
        animator.SetFloat("Move", Mathf.Abs(horizontalMove) + Mathf.Abs(verticalMove));

        if (horizontalMove < 0 && FacingRight)
        {
            Flip();
        }
        else if (horizontalMove > 0 && !FacingRight)
        {
            Flip();
        }

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
        UpdateHealthUI();
        Debug.Log("осталось хп: " + Health);
    }

    public void Heal(float amount)
    {
        Health += amount;
        if (Health > 100)
        {
            Health = 100;
        }
        UpdateHealthUI();
        Debug.Log("восстановил хп: " + Health);
    }

    private void UpdateHealthUI()
    {
        int heartPoints = Mathf.CeilToInt(Health / 20f);

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < heartPoints)
            {
                Hearts[i].sprite = FullHearts;
            }
            else
            {
                Hearts[i].sprite = EmptyHearts;
            }

            Hearts[i].enabled = i < NumOfHearts;
        }

        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
