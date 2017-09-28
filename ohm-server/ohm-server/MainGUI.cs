using System;
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
        public string HDDName= String.Empty;

        //persistent variables
        public string totalDRAM = "0000";
        public string totalVRAM = "0000";

        //changing variables
        public string cpuTemp;
        public string gpuTemp;
        public string hddTemp;
        public string gpuLoad;
        public string cpuLoad;
        public string dramFree;
        public string vramFree;

        public ServerGUI()
        {
            InitializeComponent();
            this.Icon = Resources.oshw;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //Comboboxes and buttons init sequence
            InitializeControls();
            //Sensor initialization sequence
            InitializeSensors();
            //TODO: Get Human-readable Hardware Names and Specs
            GetHardwareNames();
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

            if (CPUName != String.Empty)
                cpuLabel.Text = CPUName;

            if (GPUName != String.Empty)
                gpuLabel.Text = GPUName;

            if (HDDName != String.Empty)
                hddLabel.Text = HDDName;
        }

        private string ReadSensor(string sensorName)
        {
            //iterate thru each hardware via IHardware iterables
            foreach (OpenHardwareMonitor.Hardware.IHardware myHardware in myComputer.Hardware)
            {
                foreach (OpenHardwareMonitor.Hardware.ISensor mySensor in myHardware.Sensors)
                {
                    if (sensorName == mySensor.Name)
                    {
                        if (mySensor.Value != null)
                        {
                            int temp = (int)mySensor.Value;
                            return temp.ToString();
                        }
                    }
                }
            }

            return String.Empty;
        }

        //    myHardware.Update();
        //    foreach (OpenHardwareMonitor.Hardware.ISensor mySensor in myHardware.Sensors)
        //    {
        //        int temp = (int)mySensor.Value;
        //        if (mySensor.SensorType == OpenHardwareMonitor.Hardware.SensorType.Temperature)
        //        {
        //            if (temp != null)
        //            {
        //                switch (mySensor.Name)
        //                {
        //                    case "CPU Package":
        //                        cpuTemp = temp.ToString().PadLeft(3, '0');
        //                        break;
        //                    case "GPU Core":
        //                        gpuTemp = temp.ToString().PadLeft(3, '0');
        //                        break;
        //                    case "Temperature":
        //                        hddTemp = temp.ToString().PadLeft(3, '0');
        //                        break;
        //                }
        //            }
        //        }

        //        if (mySensor.SensorType == OpenHardwareMonitor.Hardware.SensorType.Load)
        //        {
        //            if (temp != null)
        //            {
        //                switch (mySensor.Name)
        //                {
        //                    case "CPU Total":
        //                        cpuLoad = temp.ToString().PadLeft(3, '0');
        //                        break;
        //                    case "GPU Core":
        //                        gpuLoad = temp.ToString().PadLeft(3, '0');
        //                        break;
        //                }
        //            }
        //        }

        //        if (mySensor.SensorType == OpenHardwareMonitor.Hardware.SensorType.Data)
        //        {
        //            if (temp != null)
        //            {
        //                switch (mySensor.Name)
        //                {
        //                    case "Available Memory":
        //                        dramFree = temp.ToString().PadLeft(3, '0');
        //                        break;
        //                    case "GPU Memory Free":
        //                        vramFree = temp.ToString().PadLeft(3, '0');
        //                        break;
        //                }
        //            }
        //        }
        //    }

        private void COMPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PortValue = ((KeyValuePair<int, string>)COMPort.SelectedItem).Value;

            serialPort.PortName = PortValue;
        }

        private void BaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort.BaudRate = Convert.ToInt32(BaudRate.SelectedItem.ToString());
        }

        private void SendInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            intervalTimer.Interval = Convert.ToInt32(SendInterval.SelectedIndex.ToString());
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

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                    serialPort.Open();

                serialPort.WriteLine("WHATSUP MOTHAFUCKA");

                statusPanel.Text = "Transmission Started | Port: " + serialPort.PortName + " | Baud: " + serialPort.BaudRate;

                intervalTimer.Start();
            }

            catch (UnauthorizedAccessException)
            {
                statusPanel.Text = "Another app is using the COM Port";
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            intervalTimer.Stop();
            serialPort.Close();

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

            string[] ports = SerialPort.GetPortNames();

            //if no serial, program cannot continue
            //TODO: Make serial refresh
            if (ports == null || ports.Length == 0)
                statusPanel.Text = "No COM Port detected. Please restart program.";

            //if com port detected, assumed successful init
            else
            {
                Dictionary<Int32, string> COMPortItems = new Dictionary<int, string>();
                int index = 0;

                foreach (string port in ports)
                {
                    COMPortItems.Add(index++, port);
                }

                try
                {
                    COMPort.DataSource = new BindingSource(COMPortItems, null);
                    COMPort.DisplayMember = "Value";
                    COMPort.ValueMember = "Key";
                }

                catch (ArgumentException) { }

                COMPort.SelectedIndex = 0;      //first com port default
                BaudRate.SelectedIndex = 4;     //predefault data. 57600 default
                SendInterval.SelectedIndex = 4; //predefined data. 1000ms interval default

                string PortValue = ((KeyValuePair<int, string>)COMPort.SelectedItem).Value;

                serialPort.PortName = PortValue;
                serialPort.BaudRate = Convert.ToInt32(BaudRate.SelectedItem.ToString());
                intervalTimer.Interval = Convert.ToInt32(SendInterval.SelectedItem.ToString());
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

        private void intervalTimer_Tick(object sender, EventArgs e)
        {
            sendString();

            // update hardware AFTER successfully send string
            foreach (OpenHardwareMonitor.Hardware.IHardware myHardware in myComputer.Hardware)
            {
                myHardware.Update();
            }
        }

        private void sendString()
        {
            
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
