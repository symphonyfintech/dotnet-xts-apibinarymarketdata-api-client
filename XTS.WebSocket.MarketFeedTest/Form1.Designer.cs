
namespace XTS.WebSocket.MarketFeedTest
{
    partial class Form1
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAppKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBaseUrl = new System.Windows.Forms.TextBox();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chkPrintTouchlineMode = new System.Windows.Forms.CheckBox();
            this.lblLatTouchlineAt = new System.Windows.Forms.Label();
            this.lblLastEvent = new System.Windows.Forms.Label();
            this.lblJoinedAt = new System.Windows.Forms.Label();
            this.lblInitializedAt = new System.Windows.Forms.Label();
            this.lblDisconnectedCount = new System.Windows.Forms.Label();
            this.lblConnectedSince = new System.Windows.Forms.Label();
            this.lblLastDiscibbectedAt = new System.Windows.Forms.Label();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtTokensToSubscribeNSECM = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSubscribe = new System.Windows.Forms.Button();
            this.txtTokensToSubscribeNSEFO = new System.Windows.Forms.RichTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(332, 110);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.OnConnectButtonClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "App Key :";
            // 
            // txtAppKey
            // 
            this.txtAppKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAppKey.Location = new System.Drawing.Point(83, 84);
            this.txtAppKey.Name = "txtAppKey";
            this.txtAppKey.Size = new System.Drawing.Size(405, 20);
            this.txtAppKey.TabIndex = 19;
            this.txtAppKey.Text = "8252b796ad556d674c3723";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Secret Key :";
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSecretKey.Location = new System.Drawing.Point(83, 58);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(405, 20);
            this.txtSecretKey.TabIndex = 17;
            this.txtSecretKey.Text = "Dlxs330#n2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Base URL :";
            // 
            // txtBaseUrl
            // 
            this.txtBaseUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBaseUrl.Location = new System.Drawing.Point(83, 32);
            this.txtBaseUrl.Name = "txtBaseUrl";
            this.txtBaseUrl.Size = new System.Drawing.Size(405, 20);
            this.txtBaseUrl.TabIndex = 13;
            this.txtBaseUrl.Text = "http://192.168.52.52:3500";
            // 
            // txtClientId
            // 
            this.txtClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClientId.Location = new System.Drawing.Point(83, 6);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(405, 20);
            this.txtClientId.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Client ID :";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(413, 110);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(11, 141);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(477, 2);
            this.panel1.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Initialized At :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Last Joined At :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Last Event :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 222);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Last Touchline At :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(237, 201);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Disconnect Count :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(237, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Connected Since :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(237, 159);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Last Disconnected At :";
            // 
            // chkPrintTouchlineMode
            // 
            this.chkPrintTouchlineMode.AutoSize = true;
            this.chkPrintTouchlineMode.Location = new System.Drawing.Point(237, 222);
            this.chkPrintTouchlineMode.Name = "chkPrintTouchlineMode";
            this.chkPrintTouchlineMode.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkPrintTouchlineMode.Size = new System.Drawing.Size(138, 17);
            this.chkPrintTouchlineMode.TabIndex = 30;
            this.chkPrintTouchlineMode.Text = ": Write Toucline To Log";
            this.chkPrintTouchlineMode.UseVisualStyleBackColor = true;
            this.chkPrintTouchlineMode.CheckedChanged += new System.EventHandler(this.OnPrintTouchlineModeChanged);
            // 
            // lblLatTouchlineAt
            // 
            this.lblLatTouchlineAt.AutoSize = true;
            this.lblLatTouchlineAt.Location = new System.Drawing.Point(112, 222);
            this.lblLatTouchlineAt.Name = "lblLatTouchlineAt";
            this.lblLatTouchlineAt.Size = new System.Drawing.Size(84, 13);
            this.lblLatTouchlineAt.TabIndex = 34;
            this.lblLatTouchlineAt.Text = "<Dummy Value>";
            // 
            // lblLastEvent
            // 
            this.lblLastEvent.AutoSize = true;
            this.lblLastEvent.Location = new System.Drawing.Point(112, 201);
            this.lblLastEvent.Name = "lblLastEvent";
            this.lblLastEvent.Size = new System.Drawing.Size(84, 13);
            this.lblLastEvent.TabIndex = 33;
            this.lblLastEvent.Text = "<Dummy Value>";
            // 
            // lblJoinedAt
            // 
            this.lblJoinedAt.AutoSize = true;
            this.lblJoinedAt.Location = new System.Drawing.Point(112, 180);
            this.lblJoinedAt.Name = "lblJoinedAt";
            this.lblJoinedAt.Size = new System.Drawing.Size(84, 13);
            this.lblJoinedAt.TabIndex = 32;
            this.lblJoinedAt.Text = "<Dummy Value>";
            // 
            // lblInitializedAt
            // 
            this.lblInitializedAt.AutoSize = true;
            this.lblInitializedAt.Location = new System.Drawing.Point(112, 159);
            this.lblInitializedAt.Name = "lblInitializedAt";
            this.lblInitializedAt.Size = new System.Drawing.Size(84, 13);
            this.lblInitializedAt.TabIndex = 31;
            this.lblInitializedAt.Text = "<Dummy Value>";
            // 
            // lblDisconnectedCount
            // 
            this.lblDisconnectedCount.AutoSize = true;
            this.lblDisconnectedCount.Location = new System.Drawing.Point(358, 201);
            this.lblDisconnectedCount.Name = "lblDisconnectedCount";
            this.lblDisconnectedCount.Size = new System.Drawing.Size(84, 13);
            this.lblDisconnectedCount.TabIndex = 37;
            this.lblDisconnectedCount.Text = "<Dummy Value>";
            // 
            // lblConnectedSince
            // 
            this.lblConnectedSince.AutoSize = true;
            this.lblConnectedSince.Location = new System.Drawing.Point(358, 180);
            this.lblConnectedSince.Name = "lblConnectedSince";
            this.lblConnectedSince.Size = new System.Drawing.Size(84, 13);
            this.lblConnectedSince.TabIndex = 36;
            this.lblConnectedSince.Text = "<Dummy Value>";
            // 
            // lblLastDiscibbectedAt
            // 
            this.lblLastDiscibbectedAt.AutoSize = true;
            this.lblLastDiscibbectedAt.Location = new System.Drawing.Point(358, 159);
            this.lblLastDiscibbectedAt.Name = "lblLastDiscibbectedAt";
            this.lblLastDiscibbectedAt.Size = new System.Drawing.Size(84, 13);
            this.lblLastDiscibbectedAt.TabIndex = 35;
            this.lblLastDiscibbectedAt.Text = "<Dummy Value>";
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 250;
            this.timerUpdate.Tick += new System.EventHandler(this.OnTimerUpdate);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(11, 252);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(477, 2);
            this.panel2.TabIndex = 38;
            // 
            // txtTokensToSubscribeNSECM
            // 
            this.txtTokensToSubscribeNSECM.Location = new System.Drawing.Point(11, 291);
            this.txtTokensToSubscribeNSECM.Name = "txtTokensToSubscribeNSECM";
            this.txtTokensToSubscribeNSECM.Size = new System.Drawing.Size(232, 83);
            this.txtTokensToSubscribeNSECM.TabIndex = 40;
            this.txtTokensToSubscribeNSECM.Text = "";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 257);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(263, 13);
            this.label12.TabIndex = 39;
            this.label12.Text = "Instrument Tokens To Subscribe (Comma Separated) :";
            // 
            // btnSubscribe
            // 
            this.btnSubscribe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubscribe.Location = new System.Drawing.Point(130, 380);
            this.btnSubscribe.Name = "btnSubscribe";
            this.btnSubscribe.Size = new System.Drawing.Size(113, 23);
            this.btnSubscribe.TabIndex = 41;
            this.btnSubscribe.Text = "Subscribe NSECM";
            this.btnSubscribe.UseVisualStyleBackColor = true;
            this.btnSubscribe.Click += new System.EventHandler(this.OnSubscribeNSECMTokensClicked);
            // 
            // txtTokensToSubscribeNSEFO
            // 
            this.txtTokensToSubscribeNSEFO.Location = new System.Drawing.Point(257, 291);
            this.txtTokensToSubscribeNSEFO.Name = "txtTokensToSubscribeNSEFO";
            this.txtTokensToSubscribeNSEFO.Size = new System.Drawing.Size(231, 83);
            this.txtTokensToSubscribeNSEFO.TabIndex = 42;
            this.txtTokensToSubscribeNSEFO.Text = "";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 275);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 13);
            this.label13.TabIndex = 43;
            this.label13.Text = "For Segment NSECM :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(254, 275);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "For Segment NSEFO :";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(375, 380);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 45;
            this.button1.Text = "Subscribe NSEFO";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnSubscribeNSEFOTokensClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(498, 406);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtTokensToSubscribeNSEFO);
            this.Controls.Add(this.btnSubscribe);
            this.Controls.Add(this.txtTokensToSubscribeNSECM);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblDisconnectedCount);
            this.Controls.Add(this.lblConnectedSince);
            this.Controls.Add(this.lblLastDiscibbectedAt);
            this.Controls.Add(this.lblLatTouchlineAt);
            this.Controls.Add(this.lblLastEvent);
            this.Controls.Add(this.lblJoinedAt);
            this.Controls.Add(this.lblInitializedAt);
            this.Controls.Add(this.chkPrintTouchlineMode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAppKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSecretKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBaseUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.btnConnect);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "<Dummy Value>";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAppKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBaseUrl;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkPrintTouchlineMode;
        private System.Windows.Forms.Label lblLatTouchlineAt;
        private System.Windows.Forms.Label lblLastEvent;
        private System.Windows.Forms.Label lblJoinedAt;
        private System.Windows.Forms.Label lblInitializedAt;
        private System.Windows.Forms.Label lblDisconnectedCount;
        private System.Windows.Forms.Label lblConnectedSince;
        private System.Windows.Forms.Label lblLastDiscibbectedAt;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox txtTokensToSubscribeNSECM;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnSubscribe;
        private System.Windows.Forms.RichTextBox txtTokensToSubscribeNSEFO;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button1;
    }
}

