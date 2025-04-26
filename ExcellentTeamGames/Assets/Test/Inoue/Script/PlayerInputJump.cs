using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputJump : MonoBehaviour
{
    [Header("物理")]
    public Rigidbody m_Rigidbody;
    [Header("接地フラグ")]
    public bool m_EarthFlag;
    [Header("ジャンプパワー[現在値,最小値,最大値]")]
    public Vector3 m_JumpPower = new Vector3(0,100.0f,1200.0f);
    [Header("ジャンプ倍率/加速減退率")]
    public float m_AngularAndPowerAgnification = 10;
    [Header("プレイヤーサイド")]
    public bool m_PlayerSide;
    [Header("ジャンプパワーゲージ")]
    public Image m_ImageGage;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (m_EarthFlag)
        {
            PlayerSeidInput();
        }
        else
        {
            m_JumpPower.x = m_JumpPower.y;
        }
        if (m_ImageGage)
        {
            float Powers = m_JumpPower.x - m_JumpPower.y;
            if (Powers < 0)
                Powers = 0;
            m_ImageGage.fillAmount = (1.0f / (m_JumpPower.z - m_JumpPower.y)) * Powers;
        }
    }
    public void PlayerSeidInput()
    {
        if (m_PlayerSide)
        {
            //チャージ
            if (Input.GetKey(KeyCode.RightArrow))
                ChargeJumpPowers();
            //ジャンプ
            if (Input.GetKeyUp(KeyCode.RightArrow))
                JumpUp();
        }
        else
        {
            //チャージ
            if (Input.GetKey(KeyCode.A))
                ChargeJumpPowers();
            //ジャンプ
            if (Input.GetKeyUp(KeyCode.A))
                JumpUp();
        }
    }
    public void ChargeJumpPowers()
    {
        //チャージ
        m_JumpPower.x += m_AngularAndPowerAgnification * Time.deltaTime;
        if (m_JumpPower.x > m_JumpPower.z)
            m_JumpPower.x = m_JumpPower.z;
    }
    public void JumpUp()
    {
        //ジャンプ
        m_Rigidbody.AddForce(
            (this.transform.up * m_JumpPower.x) +
            (this.transform.right * Mathf.Max(
                10.0f,
                (m_AngularAndPowerAgnification / 10) - (m_JumpPower.x - m_JumpPower.z))));
        m_EarthFlag = false;
    }

    /// <summary>
    /// 接地中
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (!m_EarthFlag && collision.transform.tag == "Map") 
            m_EarthFlag = true;
    }
    /// <summary>
    /// 接地解除
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "Map")
            m_EarthFlag = false;
    }
}
