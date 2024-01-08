namespace StudySyncSystem
{
    partial class frmArchivedTasks
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvArchivedTasks = new System.Windows.Forms.DataGridView();
            this.TaskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskStatus = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsArchived = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CategoryID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unarchive = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArchivedTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 18);
            this.label1.TabIndex = 23;
            this.label1.Text = "A R C H I V E D  T A S K S";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(8)))), ((int)(((byte)(20)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbFilter);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.dgvArchivedTasks);
            this.panel1.Location = new System.Drawing.Point(13, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(703, 362);
            this.panel1.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(453, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 18);
            this.label2.TabIndex = 29;
            this.label2.Text = "Filter by :";
            // 
            // cmbFilter
            // 
            this.cmbFilter.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "Pending",
            "Complete",
            "All Categories",
            "Upcoming Tasks",
            "Category: General",
            "Category: Research",
            "Category: Project",
            "Category: Homework",
            "Category: Seatwork",
            "Category: Readings",
            "Category: Presentations",
            "Category: Exams",
            "Category: Socials",
            "Category: Others"});
            this.cmbFilter.Location = new System.Drawing.Point(525, 27);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(165, 26);
            this.cmbFilter.TabIndex = 28;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(8)))), ((int)(((byte)(20)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(74, 29);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(241, 21);
            this.txtSearch.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 18);
            this.label3.TabIndex = 26;
            this.label3.Text = "Search :";
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Yellow;
            this.btnClose.Location = new System.Drawing.Point(565, 318);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(125, 26);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.Yellow;
            this.btnSearch.Location = new System.Drawing.Point(321, 27);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 26);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // dgvArchivedTasks
            // 
            this.dgvArchivedTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvArchivedTasks.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(8)))), ((int)(((byte)(20)))));
            this.dgvArchivedTasks.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvArchivedTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArchivedTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TaskID,
            this.TaskTitle,
            this.TaskStatus,
            this.StartDate,
            this.EndDate,
            this.UserID,
            this.IsArchived,
            this.DateCreated,
            this.CategoryID,
            this.Unarchive});
            this.dgvArchivedTasks.Location = new System.Drawing.Point(14, 62);
            this.dgvArchivedTasks.Name = "dgvArchivedTasks";
            this.dgvArchivedTasks.Size = new System.Drawing.Size(676, 246);
            this.dgvArchivedTasks.TabIndex = 14;
            this.dgvArchivedTasks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArchivedTasks_CellContentClick);
            // 
            // TaskID
            // 
            this.TaskID.DataPropertyName = "TaskID";
            this.TaskID.HeaderText = "";
            this.TaskID.Name = "TaskID";
            this.TaskID.Visible = false;
            // 
            // TaskTitle
            // 
            this.TaskTitle.DataPropertyName = "TaskTitle";
            this.TaskTitle.HeaderText = "Task Title";
            this.TaskTitle.Name = "TaskTitle";
            // 
            // TaskStatus
            // 
            this.TaskStatus.DataPropertyName = "TaskStatus";
            this.TaskStatus.HeaderText = "Task Status";
            this.TaskStatus.Items.AddRange(new object[] {
            "Pending",
            "Complete"});
            this.TaskStatus.Name = "TaskStatus";
            this.TaskStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TaskStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // StartDate
            // 
            this.StartDate.DataPropertyName = "StartDate";
            this.StartDate.HeaderText = "Start Date";
            this.StartDate.Name = "StartDate";
            // 
            // EndDate
            // 
            this.EndDate.DataPropertyName = "EndDate";
            this.EndDate.HeaderText = "End Date";
            this.EndDate.Name = "EndDate";
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "";
            this.UserID.Name = "UserID";
            this.UserID.Visible = false;
            // 
            // IsArchived
            // 
            this.IsArchived.DataPropertyName = "IsArchived";
            this.IsArchived.HeaderText = "Is Archived";
            this.IsArchived.Name = "IsArchived";
            this.IsArchived.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsArchived.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsArchived.Visible = false;
            // 
            // DateCreated
            // 
            this.DateCreated.DataPropertyName = "DateCreated";
            this.DateCreated.HeaderText = "Date Created";
            this.DateCreated.Name = "DateCreated";
            this.DateCreated.Visible = false;
            // 
            // CategoryID
            // 
            this.CategoryID.DataPropertyName = "CategoryID";
            this.CategoryID.HeaderText = "Category ID";
            this.CategoryID.Name = "CategoryID";
            this.CategoryID.Visible = false;
            // 
            // Unarchive
            // 
            this.Unarchive.HeaderText = "Unarchive";
            this.Unarchive.Name = "Unarchive";
            // 
            // frmArchivedTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(729, 403);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmArchivedTasks";
            this.Text = "frmArchivedTasks";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArchivedTasks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvArchivedTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskTitle;
        private System.Windows.Forms.DataGridViewComboBoxColumn TaskStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsArchived;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCreated;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryID;
        private System.Windows.Forms.DataGridViewButtonColumn Unarchive;
    }
}