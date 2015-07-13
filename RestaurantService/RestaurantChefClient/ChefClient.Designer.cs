namespace RestaurantChefClient
{
    partial class ChefClient
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnGetOrder = new System.Windows.Forms.Button();
            this.btnApproveOrder = new System.Windows.Forms.Button();
            this.btnDeclineOrder = new System.Windows.Forms.Button();
            this.btnOrderReady = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader4});
            this.listView1.Font = new System.Drawing.Font("Constantia", 11.25F);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(48, 29);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(657, 286);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ORDER ID";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "TABLE ID";
            this.columnHeader2.Width = 92;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "START TIME";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "END TIME";
            this.columnHeader5.Width = 131;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "DURATION";
            this.columnHeader6.Width = 95;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "ORDER STATUS";
            this.columnHeader4.Width = 124;
            // 
            // btnGetOrder
            // 
            this.btnGetOrder.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnGetOrder.Font = new System.Drawing.Font("Constantia", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnGetOrder.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnGetOrder.Location = new System.Drawing.Point(48, 335);
            this.btnGetOrder.Name = "btnGetOrder";
            this.btnGetOrder.Size = new System.Drawing.Size(148, 44);
            this.btnGetOrder.TabIndex = 9;
            this.btnGetOrder.Text = "GET ORDER";
            this.btnGetOrder.UseVisualStyleBackColor = false;
            this.btnGetOrder.Click += new System.EventHandler(this.BtnGetOrder_Click);
            // 
            // btnApproveOrder
            // 
            this.btnApproveOrder.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnApproveOrder.Font = new System.Drawing.Font("Constantia", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnApproveOrder.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnApproveOrder.Location = new System.Drawing.Point(222, 335);
            this.btnApproveOrder.Name = "btnApproveOrder";
            this.btnApproveOrder.Size = new System.Drawing.Size(148, 44);
            this.btnApproveOrder.TabIndex = 10;
            this.btnApproveOrder.Text = "APPROVE ORDER";
            this.btnApproveOrder.UseVisualStyleBackColor = false;
            this.btnApproveOrder.Click += new System.EventHandler(this.BtnApproveOrder_Click);
            // 
            // btnDeclineOrder
            // 
            this.btnDeclineOrder.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnDeclineOrder.Font = new System.Drawing.Font("Constantia", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnDeclineOrder.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDeclineOrder.Location = new System.Drawing.Point(557, 335);
            this.btnDeclineOrder.Name = "btnDeclineOrder";
            this.btnDeclineOrder.Size = new System.Drawing.Size(148, 44);
            this.btnDeclineOrder.TabIndex = 11;
            this.btnDeclineOrder.Text = "DECLINE ORDER";
            this.btnDeclineOrder.UseVisualStyleBackColor = false;
            this.btnDeclineOrder.Click += new System.EventHandler(this.BtnDeclineOrder_Click);
            // 
            // btnOrderReady
            // 
            this.btnOrderReady.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnOrderReady.Font = new System.Drawing.Font("Constantia", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnOrderReady.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnOrderReady.Location = new System.Drawing.Point(393, 335);
            this.btnOrderReady.Name = "btnOrderReady";
            this.btnOrderReady.Size = new System.Drawing.Size(148, 44);
            this.btnOrderReady.TabIndex = 13;
            this.btnOrderReady.Text = "ORDER READY";
            this.btnOrderReady.UseVisualStyleBackColor = false;
            this.btnOrderReady.Click += new System.EventHandler(this.BtnOrderReady_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.btnOrderReady);
            this.groupBox1.Controls.Add(this.btnDeclineOrder);
            this.groupBox1.Controls.Add(this.btnGetOrder);
            this.groupBox1.Controls.Add(this.btnApproveOrder);
            this.groupBox1.Font = new System.Drawing.Font("Constantia", 12.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(37, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(735, 397);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ORDER STATUS";
            // 
            // ChefClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(823, 453);
            this.Controls.Add(this.groupBox1);
            this.Name = "ChefClient";
            this.Text = "Chef UI";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnGetOrder;
        private System.Windows.Forms.Button btnApproveOrder;
        private System.Windows.Forms.Button btnDeclineOrder;
        private System.Windows.Forms.Button btnOrderReady;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}
