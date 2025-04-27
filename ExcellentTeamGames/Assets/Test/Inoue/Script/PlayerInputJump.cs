using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputJump : MonoBehaviour
{
    public List<string> m_Message;
    public Text m_MessageText;
    public float m_TextTime = 0;
    [Header("����")]
    public Rigidbody m_Rigidbody;
    [Header("�ڒn�t���O")]
    public bool m_EarthFlag;
    [Header("�W�����v�p���[[���ݒl,�ŏ��l,�ő�l]")]
    public Vector3 m_JumpPower = new Vector3(0,100.0f,1200.0f);
    [Header("�W�����v�{��/�������ޗ�")]
    public float m_AngularAndPowerAgnification = 10;
    [Header("�v���C���[�T�C�h")]
    public bool m_PlayerSide;
    [Header("�W�����v�p���[�Q�[�W")]
    public Image m_ImageGage;
    [Header("�I�[�f�B�I�\�[�X")]
    public AudioSource m_AudioSource;
    [Header("�W�����v�s����")]
    public float m_JUmpOKChargePoint = 0.25f;
    [Header("�`���[�W�N�[���^�C��")]
    public float m_ChargeCoolTime = 0.0f;
    [Header("�`���[�W�N�[���ő�^�C��")]
    public float m_ChargeCoolMaxTime = 2.0f;
    void Start()
    {
        m_ChargeCoolTime = 0.0f;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (m_TextTime <= 0.0f)
        {
            m_MessageText.text = "";
        }
        else
        { 
            m_TextTime -= Time.deltaTime;
            if (m_TextTime <= 0.0f)
                m_TextTime = 0.0f;
        }

        m_ChargeCoolTime += 1.0f * Time.deltaTime;
        if (m_ChargeCoolTime >= m_ChargeCoolMaxTime)
            m_ChargeCoolTime = m_ChargeCoolMaxTime;

        PlayerSeidInput();
        /*
        if (m_EarthFlag)
        {
        }
        else
        {
            m_JumpPower.x = m_JumpPower.y;
        }
        */

        if (m_ImageGage)
        {
            m_ImageGage.fillAmount = ChargePoint();
            if (ChargePoint() > m_JUmpOKChargePoint)
            {
                m_ImageGage.color = new Color(1, ChargePoint(), ChargePoint());
            }
            else
                m_ImageGage.color = Color.red;

        }
    }
    public float ChargePoint()
    {
        float Powers = m_JumpPower.x - m_JumpPower.y;
        if (Powers < 0)
            Powers = 0;
        return (1.0f / (m_JumpPower.z - m_JumpPower.y)) * Powers;
    }
    public void PlayerSeidInput()
    {
        if (m_PlayerSide)
        {
            //�`���[�W
            if (Input.GetKey(KeyCode.RightArrow))
                ChargeJumpPowers();
            //�W�����v
            if (Input.GetKeyUp(KeyCode.RightArrow))
                JumpUp();
        }
        else
        {
            //�`���[�W
            if (Input.GetKey(KeyCode.A))
                ChargeJumpPowers();
            //�W�����v
            if (Input.GetKeyUp(KeyCode.A))
                JumpUp();
        }
    }
    public void ChargeJumpPowers()
    {
        if (m_ChargeCoolTime >=  m_ChargeCoolMaxTime)
        {
            //�`���[�W
            m_JumpPower.x += m_AngularAndPowerAgnification * Time.deltaTime;
            if (m_JumpPower.x > m_JumpPower.z)
                m_JumpPower.x = m_JumpPower.z;
        }
    }
    public void JumpUp()
    {
        if (ChargePoint() > m_JUmpOKChargePoint)
        {
            //�W�����v
            m_Rigidbody.AddForce(
                (this.transform.up * m_JumpPower.x) +
                (this.transform.right * Mathf.Max(
                    10.0f,
                    (m_AngularAndPowerAgnification / 10) - (m_JumpPower.x - m_JumpPower.z))));
            m_JumpPower.x = m_JumpPower.y;
            m_EarthFlag = false;
            m_ChargeCoolTime = 0.0f;
            m_MessageText.text = m_Message[Random.Range(0, m_Message.Count)];
            m_TextTime = 2.0f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        m_AudioSource.Stop();
        m_AudioSource.Play();
        //SoundManager.Instance.PlaySe("�v���C���[���n");
    }
    /// <summary>
    /// �ڒn��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (!m_EarthFlag && collision.transform.tag == "Map")
        {
            m_EarthFlag = true;
        }
    }
    /// <summary>
    /// �ڒn����
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "Map")
            m_EarthFlag = false;
    }
}
