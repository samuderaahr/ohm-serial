using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ohm_server.Properties;

namespace ohm_server
{
    public partial class ServerGUI : Form
    {

        protected StatusBar mainStatusBar = new StatusBar();
        protected StatusBarPanel statusPanel = new StatusBarPanel();

        public ServerGUI()
        {
            InitializeComponent();
            this.Icon = Resources.oshw;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //Comboboxes and buttons init sequence
            InitializeControls();
        }

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



        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
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
