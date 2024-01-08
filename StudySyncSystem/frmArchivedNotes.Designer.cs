namespace StudySyncSystem
{
    partial class frmArchivedNotes
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvArchivedNotes = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NoteID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoteTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoteContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsArchived = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Unarchive = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArchivedNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(8)))), ((int)(((byte)(20)))));
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.dgvArchivedNotes);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(703, 362);
            this.panel1.TabIndex = 24;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.Yellow;
            this.btnSearch.Location = new System.Drawing.Point(492, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(101, 26);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(100, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 21;
            this.label2.Text = "Search :";
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(8)))), ((int)(((byte)(20)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(171, 26);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(307, 21);
            this.txtSearch.TabIndex = 20;
            // 
            // dgvArchivedNotes
            // 
            this.dgvArchivedNotes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvArchivedNotes.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(8)))), ((int)(((byte)(20)))));
            this.dgvArchivedNotes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvArchivedNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArchivedNotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NoteID,
            this.NoteTitle,
            this.NoteContent,
            this.UserID,
            this.DateCreated,
            this.IsArchived,
            this.Unarchive});
            this.dgvArchivedNotes.Location = new System.Drawing.Point(14, 61);
            this.dgvArchivedNotes.Name = "dgvArchivedNotes";
            this.dgvArchivedNotes.Size = new System.Drawing.Size(676, 246);
            this.dgvArchivedNotes.TabIndex = 14;
            this.dgvArchivedNotes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArchivedNotes_CellContentClick);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Yellow;
            this.btnClose.Location = new System.Drawing.Point(565, 318);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(125, 26);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 18);
            this.label1.TabIndex = 25;
            this.label1.Text = "A R C H I V E D  N O T E S";
            // 
            // NoteID
            // 
            this.NoteID.DataPropertyName = "NoteID";
            this.NoteID.HeaderText = "NoteID";
            this.NoteID.Name = "NoteID";
            this.NoteID.Visible = false;
            // 
            // NoteTitle
            // 
            this.NoteTitle.DataPropertyName = "NoteTitle";
            this.NoteTitle.HeaderText = "Note Title";
            this.NoteTitle.Name = "NoteTitle";
            // 
            // NoteContent
            // 
            this.NoteContent.DataPropertyName = "NoteContent";
            this.NoteContent.HeaderText = "Note Content";
            this.NoteContent.Name = "NoteContent";
            this.NoteContent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "User ID";
            this.UserID.Name = "UserID";
            this.UserID.Visible = false;
            // 
            // DateCreated
            // 
            this.DateCreated.DataPropertyName = "DateCreated";
            this.DateCreated.HeaderText = "Date Created";
            this.DateCreated.Name = "DateCreated";
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
            // Unarchive
            // 
            this.Unarchive.HeaderText = "Unarchive";
            this.Unarchive.Name = "Unarchive";
            this.Unarchive.Text = "Unarchive";
            // 
            // frmArchivedNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(729, 403);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmArchivedNotes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmArchivedNotes";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArchivedNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvArchivedNotes;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoteID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoteTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoteContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCreated;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsArchived;
        private System.Windows.Forms.DataGridViewButtonColumn Unarchive;
    }
}