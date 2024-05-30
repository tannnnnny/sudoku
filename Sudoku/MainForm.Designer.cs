using System.Windows.Forms;

namespace Sudoku
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            StartEasyButton = new Button();
            StartMediumButton = new Button();
            StartHardButton = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            TimerLabel = new Label();
            SolveButton = new Button();
            ClearButton = new Button();
            MakeOneStepButton = new Button();
            ShowErrorsButton = new Button();
            SuspendLayout();
            // 
            // StartEasyButton
            // 
            StartEasyButton.Location = new Point(645, 342);
            StartEasyButton.Name = "StartEasyButton";
            StartEasyButton.Size = new Size(94, 29);
            StartEasyButton.TabIndex = 0;
            StartEasyButton.Text = "Easy";
            StartEasyButton.UseVisualStyleBackColor = true;
            StartEasyButton.Click += StartEasyButton_Click;
            // 
            // StartMediumButton
            // 
            StartMediumButton.Location = new Point(645, 377);
            StartMediumButton.Name = "StartMediumButton";
            StartMediumButton.Size = new Size(94, 29);
            StartMediumButton.TabIndex = 1;
            StartMediumButton.Text = "Medium";
            StartMediumButton.UseVisualStyleBackColor = true;
            StartMediumButton.Click += StartMediumButton_Click;
            // 
            // StartHardButton
            // 
            StartHardButton.Location = new Point(645, 416);
            StartHardButton.Name = "StartHardButton";
            StartHardButton.Size = new Size(94, 29);
            StartHardButton.TabIndex = 2;
            StartHardButton.Text = "Hard";
            StartHardButton.UseVisualStyleBackColor = true;
            StartHardButton.Click += StartHardButton_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // TimerLabel
            // 
            TimerLabel.AutoSize = true;
            TimerLabel.Location = new Point(23, 13);
            TimerLabel.Name = "TimerLabel";
            TimerLabel.Size = new Size(44, 20);
            TimerLabel.TabIndex = 3;
            TimerLabel.Text = "00:00";
            // 
            // SolveButton
            // 
            SolveButton.ForeColor = Color.Maroon;
            SolveButton.Location = new Point(645, 74);
            SolveButton.Name = "SolveButton";
            SolveButton.Size = new Size(94, 29);
            SolveButton.TabIndex = 4;
            SolveButton.Text = "Solve";
            SolveButton.UseVisualStyleBackColor = true;
            SolveButton.Click += SolveButton_Click;
            // 
            // ClearButton
            // 
            ClearButton.Location = new Point(645, 109);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(94, 29);
            ClearButton.TabIndex = 5;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // MakeOneStepButton
            // 
            MakeOneStepButton.Location = new Point(645, 144);
            MakeOneStepButton.Name = "MakeOneStepButton";
            MakeOneStepButton.Size = new Size(94, 29);
            MakeOneStepButton.TabIndex = 6;
            MakeOneStepButton.Text = "One Step";
            MakeOneStepButton.UseVisualStyleBackColor = true;
            MakeOneStepButton.Click += MakeOneStepButton_Click;
            // 
            // ShowErrorsButton
            // 
            ShowErrorsButton.Location = new Point(635, 179);
            ShowErrorsButton.Name = "ShowErrorsButton";
            ShowErrorsButton.Size = new Size(116, 29);
            ShowErrorsButton.TabIndex = 7;
            ShowErrorsButton.Text = "Show Errors";
            ShowErrorsButton.UseVisualStyleBackColor = true;
            ShowErrorsButton.Click += ShowErrorsButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(917, 597);
            Controls.Add(ShowErrorsButton);
            Controls.Add(MakeOneStepButton);
            Controls.Add(ClearButton);
            Controls.Add(SolveButton);
            Controls.Add(TimerLabel);
            Controls.Add(StartHardButton);
            Controls.Add(StartMediumButton);
            Controls.Add(StartEasyButton);
            KeyPreview = true;
            Name = "MainForm";
            Text = "Sudoku";
            Load += MainForm_Load;
            KeyDown += MainForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartEasyButton;
        private Button StartMediumButton;
        private Button StartHardButton;
        private System.Windows.Forms.Timer timer1;
        private Label TimerLabel;
        private Button SolveButton;
        private Button ClearButton;
        private Button MakeOneStepButton;
        private Button ShowErrorsButton;
    }
}