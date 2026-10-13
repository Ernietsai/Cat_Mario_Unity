using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SK.Framework;

public class Player : MonoBehaviour
{
    public static bool courseClear = false;
    private Rigidbody2D rb2D;
    private AudioSource audioSource;
    private bool touchground = false;
    private bool touchFlag = false;
    private bool isSpeedUp = false;
    private int FinishAnimationState = 0;
    private float finishTime = 1f / 0f;
    private float touchFlagHigh;
    private const float TOUCH_FLAG_HIGH_LOWEST = 1.5f;
    private const float TOUCH_FLAG_HIGH_HIGHEST = 17.5f;
    private int score;
    private const int SCORE_HIGHEST = 1000;
    private const int SCORE_LOWEST = 0;
    private Collider2D flagPoleObjict;
    private Collision2D groundObjict;
    protected float xboxJoyX = 0.0f;
    protected float xboxJoyY = 0.0f;
    [SerializeField] float jumpHeight   = 2;
                     float jumpRadian   = 90 * (Mathf.PI / 180);
    [SerializeField] float moveForse    = 10;
    [SerializeField] float fastestSpeed = 10;
    [SerializeField] float fallSpeed    = 10;
    [SerializeField] float gravityScale = 5;
    [SerializeField] float fallGravityScale = 0;
    [SerializeField] AudioClip flagSound;
    [SerializeField] AudioClip courseClearSound;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("我是你在遊戲中的樣子。");    
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        //Debug.Log(rb2D);
        audioSource = gameObject.GetComponent<AudioSource>();
        //transform.Translate(new Vector2(160 - this.transform.position.x, 0), Space.World);  //此行為作弊用，欲完整遊玩時將此行註解掉即可。
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!(touchFlag))
        {
            float lh = Input.GetAxis(XBox.LeftStickHorizontal);
            float lv = Input.GetAxis(XBox.LeftStickVertical);

            if(Input.GetKey(KeyCode.UpArrow)    || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(XBox.A) || Input.GetKey(XBox.B) || lv > 0.9)
            {
                if (touchground == true && groundObjict != null)
                {
                    //Debug.Log("跳!跳!跳到 . . .");
                    Vector2 location = this.transform.position;
                    Vector2 closestPoint = groundObjict.collider.ClosestPoint(location);
                    //float pointRadian = (Vector2.Angle(location, closestPoint) * -1 + 90) * (Mathf.PI / 180);
                    //if(Mathf.Abs((closestPoint - location).x) < 0.7)
                    if((closestPoint - location).y < 0 || ((closestPoint - location).y > 0 && ((closestPoint - location).x > 0.7 || (closestPoint - location).x < -0.7)))
                    {
                        if((closestPoint - location).x > 0.7)
                        {
                            Jump(-1);
                        }
                        else if((closestPoint - location).x < -0.7)
                        {
                            Jump(1);
                        }
                        else
                        {
                            Jump(0);
                        }
                    }
                }
            }
            isSpeedUp = false;
    
            if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || lh > 0.1) && rb2D.velocity.x < fastestSpeed)
            {
                isSpeedUp = true;
                //transform.Translate(0.1f, 0, 0);
                if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    rb2D.AddForce(Vector2.right * moveForse);
                }
                else
                {
                    rb2D.AddForce(Vector2.right * moveForse * Mathf.Abs(lh));
                }
            } 
            
            if((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || lh < -0.1) && rb2D.velocity.x > -fastestSpeed)
            {
                isSpeedUp = true;
                //transform.Translate(-0.1f, 0, 0);
                if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    rb2D.AddForce (Vector2.left * moveForse);
                }
                else
                {
                    rb2D.AddForce (Vector2.left * moveForse * Mathf.Abs(lh));
                }
            } 

            if(Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if(rb2D.velocity.y < fallSpeed)
            {
                rb2D.gravityScale = fallGravityScale;
            }
            else
            {
                rb2D.gravityScale = gravityScale;
            }

            if (isSpeedUp || !(touchground))
            {
                rb2D.drag = 0;
                rb2D.angularDrag = 0.02f;
            }
            else
            {
                rb2D.drag = 2;
                rb2D.angularDrag = 2;
            }
        }
        else
        {
            if(!(touchground))
            {
                if(FinishAnimationState == 0)
                {
                    finishTime = Time.time;
                    touchFlagHigh = flagPoleObjict.ClosestPoint(this.transform.position).y;
                    audioSource.PlayOneShot(flagSound);
                    rb2D.velocity = Vector2.zero;
                    rb2D.angularVelocity = 0;
                    rb2D.gravityScale = 0;
                    FinishAnimationState = 1;
                    courseClear = true;
                    score = (Mathf.RoundToInt(SCORE_HIGHEST * ((touchFlagHigh - TOUCH_FLAG_HIGH_LOWEST) / (TOUCH_FLAG_HIGH_HIGHEST - TOUCH_FLAG_HIGH_LOWEST))) / 100) * 100;
                    if (score > SCORE_HIGHEST)
                    {
                        score = SCORE_HIGHEST;
                    }
                    else if (score < SCORE_LOWEST)
                    {
                        score = SCORE_LOWEST;
                    }
                    print(score);
                }
                transform.Translate(Vector2.down * 0.2f, Space.World);
            }
            else
            {
                if(Time.time - finishTime >= 1.03f)
                {
                    if(FinishAnimationState == 1)
                    {
                        transform.Translate(Vector2.up * 0.2f, Space.World);
                        audioSource.PlayOneShot(courseClearSound);
                        rb2D.gravityScale = gravityScale;
                        for(int i = 0; i < 3; i++)
                        {
                            Jump(0.3f);
                        }
                        FinishAnimationState = 2;

                    }
                    if(this.transform.position.x < 210)  //190
                    {
                        rb2D.AddForce(Vector2.right * moveForse);
                        if(rb2D.velocity.y < fallSpeed)
                        {
                            rb2D.gravityScale = fallGravityScale;
                        }
                        else
                        {
                            rb2D.gravityScale = gravityScale;
                        }
                    }
                    else
                    {
                        rb2D.velocity = Vector2.zero;
                        rb2D.angularVelocity = 0;
                    }
                }
            }
        }
        
        /*以下為測試用
        if (Input.GetKey(XBox.A)) Debug.Log("A"); 
        if (Input.GetKey(XBox.B)) Debug.Log("B");
        if (Input.GetKey(XBox.X)) Debug.Log("X");
        if (Input.GetKey(XBox.Y)) Debug.Log("Y");
        if (Input.GetKey(XBox.LB)) Debug.Log("LB");
        if (Input.GetKey(XBox.RB)) Debug.Log("RB");
        if (Input.GetKey(XBox.View)) Debug.Log("View");
        if (Input.GetKey(XBox.Menu)) Debug.Log("Menu");
        if (Input.GetKey(XBox.LeftStick)) Debug.Log("LeftStick");
        if (Input.GetKey(XBox.RightStick)) Debug.Log("RightStick");

        float lh = Input.GetAxis(XBox.LeftStickHorizontal);
        float lv = Input.GetAxis(XBox.LeftStickVertical);
        float rh = Input.GetAxis(XBox.RightStickHorizontal);
        float rv = Input.GetAxis(XBox.RightStickVertical);
        float lt = Input.GetAxis(XBox.LT);
        float rt = Input.GetAxis(XBox.RT);
        float dPadH = Input.GetAxis(XBox.DPadHorizontal);
        float dPadV = Input.GetAxis(XBox.DPadVertical);

        if (lh != 0) Debug.Log($"LH:{lh}");
        if (lv != 0) Debug.Log($"LV:{lv}");
        if (rh != 0) Debug.Log($"RH:{rh}");
        if (rv != 0) Debug.Log($"RV:{rv}");
        if (lt != 0) Debug.Log($"LT:{lt}");
        if (rt != 0) Debug.Log($"RT:{rt}");
        if (dPadH != 0) Debug.Log($"DPadH:{dPadH}");
        if (dPadV != 0) Debug.Log($"DPadV:{dPadV}");
        */
    }
    

    private void Jump(float speedMultipleX)
    {
        //Debug.Log("跳!跳!跳到 . . .");
        if(speedMultipleX > 0)
        {
            jumpRadian = (45 + (90 - 45)   * (1 - Mathf.Abs(speedMultipleX))) * (Mathf.PI / 180);
        }
        else if(speedMultipleX < 0)
        {
            jumpRadian = (135 + (90 - 135) * (1 - Mathf.Abs(speedMultipleX))) * (Mathf.PI / 180);
        }
        else
        {
            jumpRadian = 90  * (Mathf.PI / 180);
        }
        //Debug.Log(location - closestPoint);// * (180 / Mathf.PI)
        //Debug.Log(new Vector2(1 * Mathf.Cos(jumpRadian), 1 * Mathf.Sin(jumpRadian)));
        float jumpForse = (Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * rb2D.gravityScale) * -2) * rb2D.mass);
        rb2D.AddForce(new Vector2(1 * Mathf.Cos(jumpRadian), 1 * Mathf.Sin(jumpRadian)) * jumpForse, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Ground")
        {
            touchground = true;
            groundObjict = other;
        }
    }
    private void OnTriggerEnter2D  (Collider2D other)
    {
        if(other.gameObject.tag == "Finish")
        {
            flagPoleObjict = other;
            touchFlag = true;
            //Debug.Log("touched ground");
        }
    }
    private void OnCollisionExit2D (Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            touchground = false;
        }
    }
    private void OnTriggerExit2D   (Collider2D other)
    {
        if(other.gameObject.tag == "Finish")
        {
            //touchFlag = false;
        }
    }
}
