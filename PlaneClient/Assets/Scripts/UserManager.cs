using AirPlaneData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public Transform Plane;
    float moveSpeed;

    AirPlaneMoveData planedata = new AirPlaneMoveData();

    // Start is called before the first frame update
    void Start()
    {
        ClientNetManager.Instance.OnSendToServer(Define.CS_Login, new byte[4]);

        MessageCenter<byte[]>.Instance.OnAddListener(Define.SC_LoginCall, SC_LoginCall);
        MessageCenter<byte[]>.Instance.OnAddListener(Define.SC_MoveCall, SC_MoveCall);
    }

   

    private void SC_MoveCall(byte[] obj)
    {
        AirPlaneMoveData data = AirPlaneMoveData.Parser.ParseFrom(obj);
        planedata = data;
        SetPlane();
    }

    public void SetPlane()
    {
        Plane.transform.position = new Vector3(planedata.Px, planedata.Py, planedata.Pz);
        Plane.transform.eulerAngles = new Vector3(planedata.EulerAnglesX, planedata.EulerAnglesY, planedata.EulerAnglesZ);
    }

    /// <summary>
    /// 初始化飞机位置
    /// </summary>
    /// <param name="obj"></param>
    private void SC_LoginCall(byte[] obj)
    {
        AirPlaneMoveData data = AirPlaneMoveData.Parser.ParseFrom(obj);
        planedata = data;
        SetPlane();
    }


    // Update is called once per frame
    void Update()
    {

        Camera.main.transform.position = Plane.position + Plane.up * 10 - Plane.forward * 20;
        Camera.main.transform.eulerAngles = Plane.eulerAngles + new Vector3(30, 0, 0);
    }
}
