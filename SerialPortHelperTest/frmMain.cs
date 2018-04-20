﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SerialPortHelperLib;

namespace SerialPortHelperTest
{
    public partial class frmMain : Form
    {
        //定义DetectCom类
        private DetectCom dc;

        //定义ConfigCom类
        private ConfigCom cc;

        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载入口函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            //检测串口列表测试
            DetectComTest();

            //串口配置控件绑定
            ConfigComTest();
        }

        #region DetectCom函数
        private void DetectComTest()
        {
            //手动刷新(不推荐)
            ReflashSerialPortList();

            //实例化自动刷新（简单实例）
            dc = new DetectCom(new DetectCom.DelegateSerialPortListEvent(AutoReflashSericalPortList));

            //可以强制使用线程或定时器刷新
            dc = new DetectCom(new DetectCom.DelegateSerialPortListEvent(AutoReflashSericalPortList));
            dc.DetectComMode = DetectComModeEnum.Thread;    //线程刷新
            dc.DetectComMode = DetectComModeEnum.Timer;     //定时器刷新

            //可以自定义刷新间隔事件
            dc = new DetectCom(new DetectCom.DelegateSerialPortListEvent(AutoReflashSericalPortList));
            dc.DetectComInterval = 100; //设置刷新间隔100ms

            //打开自动刷新
            dc.Open();

            //关闭自动刷新
            //dc.Close();

        }

        /// <summary>
        /// 刷新串口列表
        /// </summary>
        private void ReflashSerialPortList()
        {
            string[] arrSerialPortNames = DetectCom.GetSerialProtNames;
            listSerialPort.Items.Clear();
            foreach (string item in arrSerialPortNames)
            {
                listSerialPort.Items.Add(item);
            }
        }

        /// <summary>
        /// 手动刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReflashList_Click(object sender, EventArgs e)
        {
            ReflashSerialPortList();
        }

        /// <summary>
        /// 自动刷新列表开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAutoReflash_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoReflash.Checked)
            {
                dc.Open();
            }
            else
            {
                dc.Close();
            }
        }

        /// <summary>
        /// 自动刷新串口列表处理函数，串口发生变化时触发。
        /// </summary>
        /// <param name="list">串口列表</param>
        private void AutoReflashSericalPortList(List<string> list)
        {
            listSerialPort.Items.Clear();
            foreach (string item in list)
            {
                listSerialPort.Items.Add(item);
            }
        }
        #endregion

        #region ConfigCom函数
        private void ConfigComTest()
        {
            cc = new ConfigCom(cbSerial);
            cc.BindBaudRateObj(cbBaudRate);
            cc.BindDataBitsObj(cbDataBits);
            cc.BindStopBitsObj(cbStop);
            cc.BindParityObj(cbParity);
        }
        #endregion

    }
}
