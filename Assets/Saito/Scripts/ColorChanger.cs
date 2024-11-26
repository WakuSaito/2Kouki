using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    //���݂̐F�̃A���t�@�l
    private float currentAlpha;

    [SerializeField]//Mesh���A�^�b�`���ꂽ�I�u�W�F�N�g
    GameObject meshObj;

    private void Awake()
    {
        //�J���[�̃A���t�@�l�擾
        currentAlpha = meshObj.GetComponent<Renderer>().materials[1].color.a;
    }

    //�F�̃A���t�@�l�ύX
    public void ChangeColorAlpha(float _alpha)
    {
        //�F���ς��Ȃ��ꍇ�������s��Ȃ��悤�ɂ���
        if (currentAlpha == _alpha) return;
        currentAlpha = _alpha;

        Color currentColor = meshObj.GetComponent<Renderer>().materials[1].color;
        meshObj.GetComponent<Renderer>().materials[1].color = new Color(currentColor.r, currentColor.g, currentColor.b, _alpha);
    }
}