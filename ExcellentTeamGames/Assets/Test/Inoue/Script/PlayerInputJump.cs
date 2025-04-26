using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputJump : MonoBehaviour
{
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

    public float m_JUmpOKChargePoint = 0.25f;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
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
            m_ImageGage.fillAmount = ChargePoint();
            if (ChargePoint() > m_JUmpOKChargePoint)
                m_ImageGage.color = Color.blue;
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
        //�`���[�W
        m_JumpPower.x += m_AngularAndPowerAgnification * Time.deltaTime;
        if (m_JumpPower.x > m_JumpPower.z)
            m_JumpPower.x = m_JumpPower.z;
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
            m_EarthFlag = false;
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
            m_EarthFlag = true;
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
