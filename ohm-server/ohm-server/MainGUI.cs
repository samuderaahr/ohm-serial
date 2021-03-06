﻿using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenHardwareMonitor.Collections;
using ohm_server.Properties;

namespace ohm_server
{
    public partial class ServerGUI : Form
    {

        protected StatusBar mainStatusBar = new StatusBar();
        protected StatusBarPanel statusPanel = new StatusBarPanel();

        //initialization of hardware support on computer object via OHM Lib
        private OpenHardwareMonitor.Hardware.Computer myComputer =
            new OpenHardwareMonitor.Hardware.Computer();

        //hardware names
        public string CPUName = String.Empty;
        public string GPUName = String.Empty;
        public string HDDName = String.Empty;

        //persistent variables
        public string totalDRAM = "000000";
        public string totalVRAM = "000000";

        //changing variables
        public string cpuTemp;
        public string gpuTemp;
        public string hddTemp;
        public string gpuLoad;
        public string cpuLoad;
        public string dramFree;
        public string vramFree;

        public bool hasArduinoReset = false;
        string[] portList;
        bool portsDetected = false;

        public ServerGUI()
        {
            InitializeComponent();
            this.Icon = Resources.oshw;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //Comboboxes and buttons init sequence
            InitializeControls();
            //Sensor initialization sequence
            InitializeSensors();
            //Get Human-readable Hardware Names and Specs
            GetHardwareNames();
            //Update hardware at least once to avoid null temp/other values
            UpdateHardware();
        }

        // main program sequence, what happens on timerTick
        private void intervalTimer_Tick(object sender, EventArgs e)
        {
            //reset arduino implemented here as the Thread.Sleep method interferes with NVIDIA GPU Readings
            //TODO: Make this work with interval values less than 2000
            if (!hasArduinoReset)
            {
                serialPort.DtrEnable = true;
                hasArduinoReset = true;
            }

            else
                serialPort.DtrEnable = false;

            // TODO: make workaround to make calls more eye-friendly
            cpuTemp = ReadSensor("CPU Package", OpenHardwareMonitor.Hardware.SensorType.Temperature);
            cpuLoad = ReadSensor("CPU Total", OpenHardwareMonitor.Hardware.SensorType.Load);
            gpuTemp = ReadSensor("GPU Core", OpenHardwareMonitor.Hardware.SensorType.Temperature);
            gpuLoad = ReadSensor("GPU Core", OpenHardwareMonitor.Hardware.SensorType.Load);
            hddTemp = ReadSensor("Temperature", OpenHardwareMonitor.Hardware.SensorType.Temperature);
            dramFree = ReadSensor("Available Memory", OpenHardwareMonitor.Hardware.SensorType.Data);
            vramFree = ReadSensor("GPU Memory Free", OpenHardwareMonitor.Hardware.SensorType.SmallData);

            if (totalDRAM == "000000")
            {
                try
                {
                    totalDRAM = ReadSensor("Used Memory", OpenHardwareMonitor.Hardware.SensorType.Data);
                    decimal temp = Convert.ToDecimal(totalDRAM);
                    totalDRAM = ReadSensor("Available Memory", OpenHardwareMonitor.Hardware.SensorType.Data);
                    temp += Convert.ToDecimal(totalDRAM);
                    totalDRAM = temp.ToString();
                }

                catch (FormatException) { }
            }

            if (totalVRAM == "000000")
                totalVRAM = ReadSensor("GPU Memory Total", OpenHardwareMonitor.Hardware.SensorType.SmallData);

            if (serialPort.IsOpen)
                sendRoutine();

            updateGUILabels();
        }

        private void UpdateHardware()
        {
            foreach (OpenHardwareMonitor.Hardware.IHardware myHardware in myComputer.Hardware)
            {
                myHardware.Update();
            }
        }

        private string ReadSensor(string sensorName, OpenHardwareMonitor.Hardware.SensorType sensorType)
        {
            //iterate thru each hardware via IHardware iterables
            foreach (OpenHardwareMonitor.Hardware.IHardware myHardware in myComputer.Hardware)
            {
                //TODO: Relocate the update to just once per interval that might reduce 
                //CPU usage significantly at the cost of data latency
                myHardware.Update();
                foreach (OpenHardwareMonitor.Hardware.ISensor mySensor in myHardware.Sensors)
                {
                    if (sensorType == mySensor.SensorType)
                    {
                        if (sensorName == mySensor.Name)
                        {
                            if (mySensor.Value != null)
                            {
                                float temp;
                                if (mySensor.SensorType == OpenHardwareMonitor.Hardware.SensorType.Data)
                                    temp = (float)Math.Round((double)(mySensor.Value*1024));
                                else
                                    temp = (int) mySensor.Value;
                                return temp.ToString().PadLeft(3, '0');
                            }
                        }
                    }
                }
            }

            return "N/A";
        }

        private void sendRoutine()
        {
            string temp = String.Empty;

            temp = '#' + cpuTemp + '.' + gpuTemp + '.' + hddTemp + '.' + cpuLoad + '.' + gpuLoad + '.' + dramFree + '.' + vramFree;

            serialPort.WriteLine(temp);
        }

        private void sendOnce()
        {
            string temp = String.Empty;

            temp = '$' + CPUName + '.' + GPUName + '.' + HDDName + '.' + totalDRAM + '.' + totalVRAM;

            serialPort.WriteLine(temp);
        }

        private void updateGUILabels()
        {
            cpuTempLabel.Text = cpuTemp + " °C";
            gpuTempLabel.Text = gpuTemp + " °C";
            hddTempLabel.Text = hddTemp + " °C";
            cpuLoadLabel.Text = cpuLoad + " %";
            gpuLoadLabel.Text = gpuLoad + " %";
            dramLabel.Text = dramFree + "MB / " + totalDRAM + "MB";
            vramLabel.Text = vramFree + "MB / " + totalVRAM + "MB";
        }

        private void COMPort_SelectedIndexChanged(object sender, EventArgs e)
        {
           // handle re-update issue
            try
            {
                string PortValue = ((KeyValuePair<int, string>)COMPort.SelectedItem).Value;
                serialPort.PortName = PortValue;
            }

            catch (NullReferenceException) { }
        }

        private void BaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort.BaudRate = Convert.ToInt32(BaudRate.SelectedItem.ToString());
        }

        private void SendInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            intervalTimer.Interval = Convert.ToInt32(SendInterval.SelectedItem.ToString());
        }

        private void COMPort_DropDown(object sender, EventArgs e)
        {
            Stop();
        }

        private void BaudRate_DropDown(object sender, EventArgs e)
        {
            Stop();
        }

        private void SendInterval_DropDown(object sender, EventArgs e)
        {
            Stop();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            Stop();

            portList = null;

            detectPorts();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (portsDetected)
                {
                    if (!serialPort.IsOpen)
                        serialPort.Open();
                    
                    //serialPort.WriteLine("WHATSUP MADDAFAKKA");
                    sendOnce();

                    statusPanel.Text = "Transmission Started | Port: " + serialPort.PortName + " | Baud: " + serialPort.BaudRate + " | Interval: " + intervalTimer.Interval;

                    intervalTimer.Start();
                }

                else statusPanel.Text = "Cannot start transmission. No COM ports are detected. Please click refresh";
            }

            catch (UnauthorizedAccessException)
            {
                statusPanel.Text = "Another app is using the COM Port";
            }
        }

        private void Stop()
        {
            hasArduinoReset = false;

            intervalTimer.Stop();
            serialPort.Close();

            //workaround to flaky IsOpen getter
            portsDetected = false;

            statusPanel.Text = "Transmission Stopped";
        }

        private void InitializeControls()
        {
            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel);

            mainStatusBar.ShowPanels = true;
            this.Controls.Add(mainStatusBar);
            statusPanel.Text = "Ready.";

            intervalTimer.Enabled = true;
            intervalTimer.Interval = 100;

            detectPorts();

            if (portsDetected)
            {
                COMPort.SelectedIndex = 0;      //first com port default
                BaudRate.SelectedIndex = 4;     //predefined data. 57600 default
                SendInterval.SelectedIndex = 4; //predefined data. 1000ms interval default

                string PortValue = ((KeyValuePair<int, string>)COMPort.SelectedItem).Value;

                serialPort.PortName = PortValue;
                serialPort.BaudRate = Convert.ToInt32(BaudRate.SelectedItem.ToString());
                intervalTimer.Interval = Convert.ToInt32(SendInterval.SelectedItem.ToString());
            }
        }

        private void detectPorts()
        {
            //reinit to zero first
            portList = null;
            COMPort.DataSource = null;

            bool success;
            portList = SerialPort.GetPortNames();

            //if no serial, program cannot continue
            if (portList == null || portList.Length == 0)
                success = false;

            else
            {
                Dictionary<Int32, string> COMPortItems = new Dictionary<int, string>();
                int index = 0;

                foreach (string port in portList)
                {
                    COMPortItems.Add(index++, port);
                }

                try
                {
                    COMPort.DataSource = new BindingSource(COMPortItems, null);
                    COMPort.DisplayMember = "Value";
                    COMPort.ValueMember = "Key";

                    success = true;
                }

                catch (ArgumentException) { success = false; }

                if (success)
                {
                    portsDetected = true;
                    statusPanel.Text = "Serial port list updated";
                }

                else
                {
                    portsDetected = false;
                    statusPanel.Text = "No COM Port detected. Please click refresh.";
                }
            }
        }

        private void ServerGUI_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void InitializeSensors()
        {
            myComputer.CPUEnabled = true;
            myComputer.GPUEnabled = true;
            myComputer.HDDEnabled = true;
            myComputer.RAMEnabled = true;
            myComputer.Open();
        }

        private void GetHardwareNames()
        {
            foreach (OpenHardwareMonitor.Hardware.IHardware myHardware in myComputer.Hardware)
            {
                if (myHardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.CPU)
                    CPUName = myHardware.Name;

                if (myHardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.GpuAti)
                    GPUName = myHardware.Name;

                if (myHardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.GpuNvidia)
                    GPUName = myHardware.Name;

                if (myHardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.HDD)
                    HDDName = myHardware.Name;
            }

            if (cpuLabel.Text == "cpuLabel")
                if (CPUName != String.Empty)
                    cpuLabel.Text = CPUName;

            if (gpuLabel.Text == "gpuLabel")
                if (GPUName != String.Empty)
                    gpuLabel.Text = GPUName;

            if (hddLabel.Text == "hddLabel")
                if (HDDName != String.Empty)
                    hddLabel.Text = HDDName;
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
