using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ohm_server
{
    public partial class Form1 : Form
    {

        protected StatusBar mainStatusBar = new StatusBar();
        protected StatusBarPanel statusPanel = new StatusBarPanel();
        
        public Form1()
        {
            InitializeComponent();

            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.Text = "READY";
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel);

            mainStatusBar.ShowPanels = true;
            this.Controls.Add(mainStatusBar);

            serialPort.BaudRate = 9600;
            intervalTimer.Enabled = true;
            intervalTimer.Interval = 100;

            COMPort.Enabled = true;

            string[] ports = SerialPort.GetPortNames();
            int index = 0;
            Dictionary<Int32, string> COMPortItems = new Dictionary<int, string>();

            foreach (string port in ports)
            {
                COMPortItems.Add(index++, port);
            }

            try
            {
                COMPort.DataSource = new BindingSource(COMPortItems, null);
                COMPort.DisplayMember = "Value";
                COMPort.ValueMember = "Key";

                COMPort.SelectedIndex = 0;
            }

            catch (ArgumentNullException) { }

            //Dictionary<string, string> COMPortItems = new Dictionary<string, string>();
            //COMPortItems.Add("1", "COM3");
            //COMPortItems.Add("2", "COM4");
            //COMPortItems.Add("3", "COM5");

            //COMPort.DataSource = new BindingSource(COMPortItems, null);
            //COMPort.DisplayMember = "Value";
            //COMPort.ValueMember = "Key";

            //COMPort.SelectedIndex = 1;
        }

        private void COMPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (COMPort.SelectedIndex)
            {
                case 0 : break;
                case 1 : break;
                case 2 : break;
            }
        }

        private void BaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SendInterval_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            serialPort.PortName = COMPort.SelectedIndex.ToString();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }
          
        private void Stop()
        {
            intervalTimer.Stop();
            serialPort.Close();
            
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
