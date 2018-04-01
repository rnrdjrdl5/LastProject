using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class TimeBar {

    void UpdateTime()
    {
        if (MaxTime <= NowTime + Time.deltaTime)
        {
            NowTime = MaxTime;
            DestroyObjects();
        }

        else
        {
            NowTime += Time.deltaTime;
        }

    }


    public void CreateTimeBarPanel()
    {
        TimeBarPanelObject = Instantiate(TimeBarPanelPrefab);

        TimeBarPanelObject.transform.SetParent(InGameCanvas.transform);

        TimeBarPanelObject.transform.localScale = Vector3.one;


        Vector3 v3 = new Vector3();

        v3.x = Screen.width / 2;
        v3.y = Screen.height / 2;
        v3.z = 0.0f;

        TimeBarPanelObject.transform.position = v3;



        NowTimeBar = TimeBarPanelObject.transform.Find("NowTimeBar").gameObject;

        NowTimeBarImage = NowTimeBar.GetComponent<Image>();
    }

    void UpdateTimeBarImage()
    {
        NowTimeBarImage.fillAmount =
            NowTime / MaxTime;
    }


    public void DestroyObjects()
    {
        Destroy(TimeBarPanelObject);

        TimeBarPanelObject = null;

        isCount = false;

        NowTimeBar = null;
        NowTimeBarImage = null;

        NowTime = 0.0f;

        MaxTime = 0.0f;

    }

}
