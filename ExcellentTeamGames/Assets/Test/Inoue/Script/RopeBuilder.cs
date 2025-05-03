// ===============================================
// RopeBuilder.cs
// ���ԃ`�F�C�������{�Ȑ��\���X�N���v�g
// �i�n�_�ƏI�_��A�����镡���̒��ԃ`�F�C���������������ALineRenderer�ŋȐ���ɕ`��j
// ===============================================

using UnityEngine;
using System.Collections.Generic;

// RopeBuilder�N���X�F���[�v�i�`�F�C���j�𐶐����ĕ`�悷��N���X
public class RopeBuilder : MonoBehaviour
{
    //�N�x�J���[
    public float freshness;
    //�N�x������
    public float freshnessLost;

    // �n�_�ƂȂ�Rigidbody
    public Rigidbody startPoint;

    // �I�_�ƂȂ�Rigidbody
    public Rigidbody endPoint;

    // �쐬���钆�ԃ`�F�C���̐�
    public int chainCount = 10;

    // �`�F�C�����m�̊Ԋu
    public float chainSpacing = 0.5f;

    // �`�F�C���p�Ɏg��Prefab�iSphere�Ȃǁj
    public GameObject chainPrefab;

    // �������ꂽ�`�F�C����Rigidbody���X�g
    private List<Rigidbody> chainLinks = new List<Rigidbody>();

    // �Ȑ��`��pLineRenderer
    private LineRenderer lineRenderer;
    private LineRenderer outlineRenderer;

    [SerializeField]
    private float lineSize = 1.1f;

    [SerializeField]
    private Gradient m_color = new Gradient();
    public Color m_OutLineColor = Color.white;

    public GameManager manager;

    // �N�����Ƀ`�F�C��������LineRenderer�Z�b�g�A�b�v
    void Start()
    {
        freshness = 1.0f;
        // ���ԃ`�F�C���𐶐�
        CreateChain();

        // LineRenderer���Z�b�g�A�b�v
        SetupLineRenderer();
    }

    // ���t���[���A���[�v�̋Ȑ����X�V
    void Update()
    {
        UpdateLineRenderer();
    }

    // ���ԃ`�F�C���𐶐����ĘA�����鏈��
    void CreateChain()
    {
        // ���O��Rigidbody�i�ŏ���startPoint�j
        Rigidbody prevBody = startPoint;

        // �w�萔�������ԃ`�F�C���𐶐�
        for (int i = 0; i < chainCount; i++)
        {
            // �n�_����I�_�܂œ��Ԋu�Ŕz�u
            GameObject chain = Instantiate(
                chainPrefab,
                Vector3.Lerp(startPoint.position, endPoint.position, (i + 1f) / (chainCount + 1f)),
                Quaternion.identity,
                transform
            );

            // Rigidbody��������Βǉ�
            Rigidbody rb = chain.GetComponent<Rigidbody>();
            if (rb == null) rb = chain.AddComponent<Rigidbody>();

            // SpringJoint�őO�̃{�f�B�Ɛڑ�
            SpringJoint joint = chain.AddComponent<SpringJoint>();
            joint.connectedBody = prevBody;

            // ���́i�o�l�̋����j��ݒ�
            joint.spring = 100f;

            // �����i�o�l�̗h��}���j��ݒ�
            joint.damper = 5f;

            // �����A���J�[�ݒ�𖳌���
            joint.autoConfigureConnectedAnchor = false;

            // �ڑ��_�i���g���j�����_��
            joint.anchor = Vector3.zero;

            // �ڑ��_�i���葤�j�����_��
            joint.connectedAnchor = Vector3.zero;

            // �쐬�����`�F�C�������X�g�ɒǉ�
            chainLinks.Add(rb);

            // ���̃`�F�C���̐ڑ���ɂ���
            prevBody = rb;
        }

        // �Ō��endPoint�ƍŌ�̃`�F�C����ڑ�
        SpringJoint endJoint = endPoint.gameObject.AddComponent<SpringJoint>();
        endJoint.connectedBody = prevBody;
        endJoint.spring = 100f;
        endJoint.damper = 5f;
        endJoint.autoConfigureConnectedAnchor = false;
        endJoint.anchor = Vector3.zero;
        endJoint.connectedAnchor = Vector3.zero;
    }

    //// LineRenderer��ݒ肷�鏈��
    //void SetupLineRenderer()
    //{
    //    // ���g��LineRenderer��ǉ�
    //    lineRenderer = gameObject.AddComponent<LineRenderer>();

    //    // ���̒��_���i�n�_�{���ԁ{�I�_�j
    //    lineRenderer.positionCount = chainCount + 2;

    //    // ���̎n�_���̕�
    //    lineRenderer.startWidth = lineSize;

    //    // ���̏I�_���̕�
    //    lineRenderer.endWidth = lineSize;

    //    // �ȈՃ}�e���A���iSprite�p�j��ݒ�
    //    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

    //    // ���̗��[�����炩�Ɋۂ߂�
    //    lineRenderer.numCapVertices = 5;
    //}

    void SetupLineRenderer()
    {
        // �A�E�g���C���p�I�u�W�F�N�g���쐬
        GameObject outlineObj = new GameObject("LineOutline");
        outlineObj.transform.SetParent(this.transform, false);
        outlineRenderer = outlineObj.AddComponent<LineRenderer>();

        // �{�̗p LineRenderer ��ǉ�
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.sortingOrder = 10;
        outlineRenderer.sortingOrder = 9;

        // --- ���ʐݒ� ---
        SetupRendererCommon(lineRenderer, lineSize, Color.white);
        SetupRendererCommon(outlineRenderer, lineSize + 0.05f, m_OutLineColor);
    }

    void SetupRendererCommon(LineRenderer lr, float width, Color color)
    {
        lr.positionCount = chainCount + 2;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = color;
        lr.numCapVertices = 5;
    }

    // LineRenderer�𖈃t���[���X�V�i�e�`�F�C���ʒu�𔽉f�j
    void UpdateLineRenderer()
    {
        /*
        freshness -= freshnessLost;
        if (freshness < 0.0f)
            freshness = 0.0f;
        */

        float STL = (1.0f / manager.MaxTime) * manager.elapsedTime;
        if (STL > 1.0f) STL = 1.0f;

        Color freshnessSet = m_color.Evaluate(STL);


        lineRenderer.startColor = freshnessSet;
        lineRenderer.endColor = freshnessSet;

        // LineRenderer�����݂��Ȃ���Ή������Ȃ�
        if (lineRenderer == null) return;

        // �n�_�ʒu���Z�b�g
        lineRenderer.SetPosition(0, startPoint.position);
        outlineRenderer.SetPosition(0, startPoint.position);

        // ���ԃ`�F�C���̈ʒu�����ԂɃZ�b�g
        for (int i = 0; i < chainLinks.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, chainLinks[i].position);
            outlineRenderer.SetPosition(i + 1, chainLinks[i].position);
        }

        // �I�_�ʒu���Z�b�g
        lineRenderer.SetPosition(chainLinks.Count + 1, endPoint.position);
        outlineRenderer.SetPosition(chainLinks.Count + 1, endPoint.position);
    }
}
