namespace ohm_server
{
    partial class ServerGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerGUI));
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.intervalTimer = new System.Windows.Forms.Timer(this.components);
            this.StopButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SendInterval = new System.Windows.Forms.ComboBox();
            this.BaudRate = new System.Windows.Forms.ComboBox();
            this.COMPort = new System.Windows.Forms.ComboBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cpuLabel = new System.Windows.Forms.Label();
            this.gpuLabel = new System.Windows.Forms.Label();
            this.hddLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 57600;
            // 
            // intervalTimer
            // 
            this.intervalTimer.Tick += new System.EventHandler(this.intervalTimer_Tick);
            // 
            // StopButton
            // 
            this.StopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopButton.Location = new System.Drawing.Point(518, 12);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(60, 23);
            this.StopButton.TabIndex = 7;
            this.StopButton.Text = "STOP";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(450, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(60, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "START";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "COM Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baud Rate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(293, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Interval (ms)";
            // 
            // SendInterval
            // 
            this.SendInterval.FormattingEnabled = true;
            this.SendInterval.Items.AddRange(new object[] {
            "100",
            "250",
            "500",
            "750",
            "1000",
            "2500",
            "5000",
            "10000",
            "50000",
            "100000"});
            this.SendInterval.Location = new System.Drawing.Point(360, 12);
            this.SendInterval.Name = "SendInterval";
            this.SendInterval.Size = new System.Drawing.Size(80, 21);
            this.SendInterval.TabIndex = 2;
            this.SendInterval.DropDown += new System.EventHandler(this.SendInterval_DropDown);
            this.SendInterval.SelectedIndexChanged += new System.EventHandler(this.SendInterval_SelectedIndexChanged);
            // 
            // BaudRate
            // 
            this.BaudRate.FormattingEnabled = true;
            this.BaudRate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400"});
            this.BaudRate.Location = new System.Drawing.Point(209, 12);
            this.BaudRate.Name = "BaudRate";
            this.BaudRate.Size = new System.Drawing.Size(80, 21);
            this.BaudRate.TabIndex = 1;
            this.BaudRate.DropDown += new System.EventHandler(this.BaudRate_DropDown);
            this.BaudRate.SelectedIndexChanged += new System.EventHandler(this.BaudRate_SelectedIndexChanged);
            // 
            // COMPort
            // 
            this.COMPort.FormattingEnabled = true;
            this.COMPort.Location = new System.Drawing.Point(60, 12);
            this.COMPort.Name = "COMPort";
            this.COMPort.Size = new System.Drawing.Size(80, 21);
            this.COMPort.TabIndex = 0;
            this.COMPort.DropDown += new System.EventHandler(this.COMPort_DropDown);
            this.COMPort.SelectedIndexChanged += new System.EventHandler(this.COMPort_SelectedIndexChanged);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Double-click to Restore";
            this.notifyIcon.BalloonTipTitle = "Minimized to Tray";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "OHM-Server";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // cpuLabel
            // 
            this.cpuLabel.AutoSize = true;
            this.cpuLabel.Location = new System.Drawing.Point(12, 49);
            this.cpuLabel.Name = "cpuLabel";
            this.cpuLabel.Size = new System.Drawing.Size(51, 13);
            this.cpuLabel.TabIndex = 8;
            this.cpuLabel.Text = "cpuLabel";
            // 
            // gpuLabel
            // 
            this.gpuLabel.AutoSize = true;
            this.gpuLabel.Location = new System.Drawing.Point(12, 83);
            this.gpuLabel.Name = "gpuLabel";
            this.gpuLabel.Size = new System.Drawing.Size(51, 13);
            this.gpuLabel.TabIndex = 9;
            this.gpuLabel.Text = "gpuLabel";
            // 
            // hddLabel
            // 
            this.hddLabel.AutoSize = true;
            this.hddLabel.Location = new System.Drawing.Point(12, 117);
            this.hddLabel.Name = "hddLabel";
            this.hddLabel.Size = new System.Drawing.Size(51, 13);
            this.hddLabel.TabIndex = 10;
            this.hddLabel.Text = "hddLabel";
            // 
            // ServerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 161);
            this.Controls.Add(this.hddLabel);
            this.Controls.Add(this.gpuLabel);
            this.Controls.Add(this.cpuLabel);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SendInterval);
            this.Controls.Add(this.BaudRate);
            this.Controls.Add(this.COMPort);
            this.MaximizeBox = false;
            this.Name = "ServerGUI";
            this.Text = "OHM-Serial v0.1";
            this.Resize += new System.EventHandler(this.ServerGUI_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.Timer intervalTimer;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SendInterval;
        private System.Windows.Forms.ComboBox BaudRate;
        private System.Windows.Forms.ComboBox COMPort;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label cpuLabel;
        private System.Windows.Forms.Label gpuLabel;
        private System.Windows.Forms.Label hddLabel;
    }
}

