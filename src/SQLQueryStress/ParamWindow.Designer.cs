namespace SQLQueryStress
{
    partial class ParamWindow
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
      this.columnMapGrid = new System.Windows.Forms.DataGridView();
      this.Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Datatype = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.parameter_query_label = new System.Windows.Forms.Label();
      this.getColumnsButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.parameter_mappings_label = new System.Windows.Forms.Label();
      this.database_button = new System.Windows.Forms.Button();
      this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
      ((System.ComponentModel.ISupportInitialize)(this.columnMapGrid)).BeginInit();
      this.SuspendLayout();
      // 
      // columnMapGrid
      // 
      this.columnMapGrid.AllowUserToAddRows = false;
      this.columnMapGrid.AllowUserToDeleteRows = false;
      this.columnMapGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.columnMapGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.columnMapGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column,
            this.Datatype,
            this.Parameter});
      this.columnMapGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
      this.columnMapGrid.Location = new System.Drawing.Point(16, 422);
      this.columnMapGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.columnMapGrid.Name = "columnMapGrid";
      this.columnMapGrid.RowHeadersWidth = 51;
      this.columnMapGrid.ShowEditingIcon = false;
      this.columnMapGrid.Size = new System.Drawing.Size(584, 285);
      this.columnMapGrid.TabIndex = 3;
      // 
      // Column
      // 
      this.Column.HeaderText = "Parameter";
      this.Column.MinimumWidth = 6;
      this.Column.Name = "Column";
      this.Column.Width = 130;
      // 
      // Datatype
      // 
      this.Datatype.HeaderText = "Datatype";
      this.Datatype.MinimumWidth = 6;
      this.Datatype.Name = "Datatype";
      this.Datatype.Width = 130;
      // 
      // Parameter
      // 
      this.Parameter.HeaderText = "Column";
      this.Parameter.MinimumWidth = 6;
      this.Parameter.Name = "Parameter";
      this.Parameter.Width = 130;
      // 
      // parameter_query_label
      // 
      this.parameter_query_label.AutoSize = true;
      this.parameter_query_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.parameter_query_label.Location = new System.Drawing.Point(16, 15);
      this.parameter_query_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.parameter_query_label.Name = "parameter_query_label";
      this.parameter_query_label.Size = new System.Drawing.Size(132, 17);
      this.parameter_query_label.TabIndex = 2;
      this.parameter_query_label.Text = "Parameter Query";
      // 
      // getColumnsButton
      // 
      this.getColumnsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.getColumnsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.getColumnsButton.Location = new System.Drawing.Point(20, 337);
      this.getColumnsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.getColumnsButton.Name = "getColumnsButton";
      this.getColumnsButton.Size = new System.Drawing.Size(131, 35);
      this.getColumnsButton.TabIndex = 1;
      this.getColumnsButton.Text = "Get Columns";
      this.getColumnsButton.UseVisualStyleBackColor = true;
      this.getColumnsButton.Click += new System.EventHandler(this.getColumnsButton_Click);
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.okButton.Location = new System.Drawing.Point(392, 715);
      this.okButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(100, 35);
      this.okButton.TabIndex = 4;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.cancelButton.Location = new System.Drawing.Point(500, 715);
      this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(100, 35);
      this.cancelButton.TabIndex = 5;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // parameter_mappings_label
      // 
      this.parameter_mappings_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.parameter_mappings_label.AutoSize = true;
      this.parameter_mappings_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.parameter_mappings_label.Location = new System.Drawing.Point(16, 397);
      this.parameter_mappings_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.parameter_mappings_label.Name = "parameter_mappings_label";
      this.parameter_mappings_label.Size = new System.Drawing.Size(157, 17);
      this.parameter_mappings_label.TabIndex = 6;
      this.parameter_mappings_label.Text = "Parameter Mappings";
      // 
      // database_button
      // 
      this.database_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.database_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.database_button.Location = new System.Drawing.Point(159, 337);
      this.database_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.database_button.Name = "database_button";
      this.database_button.Size = new System.Drawing.Size(125, 35);
      this.database_button.TabIndex = 2;
      this.database_button.Text = "Database";
      this.database_button.UseVisualStyleBackColor = true;
      this.database_button.Click += new System.EventHandler(this.database_button_Click);
      // 
      // elementHost1
      // 
      this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.elementHost1.Location = new System.Drawing.Point(16, 55);
      this.elementHost1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.elementHost1.Name = "elementHost1";
      this.elementHost1.Size = new System.Drawing.Size(584, 272);
      this.elementHost1.TabIndex = 0;
      this.elementHost1.Text = "elementHost1";
      // 
      // ParamWindow
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(616, 769);
      this.Controls.Add(this.elementHost1);
      this.Controls.Add(this.database_button);
      this.Controls.Add(this.parameter_mappings_label);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.getColumnsButton);
      this.Controls.Add(this.parameter_query_label);
      this.Controls.Add(this.columnMapGrid);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(594, 744);
      this.Name = "ParamWindow";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "Parameter Substitution";
      ((System.ComponentModel.ISupportInitialize)(this.columnMapGrid)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label parameter_query_label;
        private System.Windows.Forms.Button getColumnsButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label parameter_mappings_label;
        private System.Windows.Forms.Button database_button;
        private System.Windows.Forms.DataGridView columnMapGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Datatype;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private SqlControl sqlControl1;
    }
}