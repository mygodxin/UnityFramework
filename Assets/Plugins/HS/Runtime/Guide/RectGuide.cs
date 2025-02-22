using UnityEngine;

public class RectGuide : GuideBase
{
    protected float width;
    protected float height;

    float scalewidth;
    float scaleheight;

    public float Widths, Heights;//���ÿ��� ����
    public override void Guide(Canvas canvas, RectTransform target)
    {
        base.Guide(canvas, target);
        //�������
        var p = target.sizeDelta * target.lossyScale / canvas.transform.localScale;
        width = p.x / 2;//(targetCorners[3].x - targetCorners[0].x) / 2;
        height = p.y / 2;//(targetCorners[1].y - targetCorners[0].y) / 2;
        material.SetFloat("_SliderX", width + Widths);
        material.SetFloat("_SliderY", height + Heights);
    }

    public override void Guide(Canvas canvas, RectTransform target, float scale, float time)
    {
        this.Guide(canvas, target);

        scalewidth = width * scale;
        scaleheight = height * scale;
        material.SetFloat("_SliderX", scalewidth);
        material.SetFloat("_SliderY", scaleheight);

        this.time = time;
        isScaling = true;
        timer = 0;

    }

    public void SetRect(Vector2 vec)
    {
        material.SetFloat("_SliderX", vec.x);
        material.SetFloat("_SliderY", vec.y);
    }

    protected override void Update()
    {
        base.Update();
        if (isScaling)
        {
            this.material.SetFloat("_SliderX", Mathf.Lerp(scalewidth, width, timer));
            this.material.SetFloat("_SliderY", Mathf.Lerp(scaleheight, height, timer));
        }
    }
}