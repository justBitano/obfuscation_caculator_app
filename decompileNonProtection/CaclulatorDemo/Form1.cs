using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CaclulatorDemo;

public class Form1 : Form
{
	private enum Operation
	{
		Add,
		Sub,
		Mul,
		Div,
		None
	}

	private const string divideByZero = "Err!";

	private const string syntaxErr = "SYNTAX ERROR!";

	private bool decimalPointActive = false;

	private Operation previousOperation = Operation.None;

	private Operation currentOperation = Operation.None;

	private IContainer components = null;

	private TableLayoutPanel tableLayoutPanel1;

	private TextBox txtDisplay;

	private TableLayoutPanel tableLayoutPanel2;

	private Button btnRes;

	private Button btn0;

	private Button btnAdd;

	private Button btn3;

	private Button btn2;

	private Button btn1;

	private Button btnSub;

	private Button btn6;

	private Button btn5;

	private Button btn4;

	private Button btnMul;

	private Button btn9;

	private Button btn8;

	private Button btnDiv;

	private Button btnClear;

	private Button btnReset;

	private Button btnCopy;

	private Button btn7;

	private Button btnDecimal;

	public Form1()
	{
		InitializeComponent();
	}

	private void BtnCopy_Click(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(txtDisplay.Text))
		{
			Clipboard.SetText(txtDisplay.Text);
		}
	}

	private void BtnReset_Click(object sender, EventArgs e)
	{
		decimalPointActive = false;
		PreCheck_ButtonClick();
		previousOperation = Operation.None;
		txtDisplay.Clear();
	}

	private void BtnClear_Click(object sender, EventArgs e)
	{
		decimalPointActive = false;
		PreCheck_ButtonClick();
		if (txtDisplay.Text.Length > 0)
		{
			if (!double.TryParse(txtDisplay.Text[txtDisplay.Text.Length - 1].ToString(), out var _))
			{
				previousOperation = Operation.None;
			}
			txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1, 1);
		}
		if (txtDisplay.Text.Length == 0)
		{
			previousOperation = Operation.None;
		}
		if (previousOperation != Operation.None)
		{
			currentOperation = previousOperation;
		}
	}

	private void BtnDiv_Click(object sender, EventArgs e)
	{
		if (txtDisplay.TextLength != 0)
		{
			PreCheck_ButtonClick();
			currentOperation = Operation.Div;
			PerformCalculation(previousOperation);
			previousOperation = currentOperation;
			EnableOperatorButtons(enable: false);
			txtDisplay.Text += (sender as Button).Text;
		}
	}

	private void BtnMul_Click(object sender, EventArgs e)
	{
		if (txtDisplay.TextLength != 0)
		{
			PreCheck_ButtonClick();
			currentOperation = Operation.Mul;
			PerformCalculation(previousOperation);
			previousOperation = currentOperation;
			EnableOperatorButtons(enable: false);
			txtDisplay.Text += (sender as Button).Text;
		}
	}

	private void BtnSub_Click(object sender, EventArgs e)
	{
		if (txtDisplay.TextLength != 0 && previousOperation != Operation.Sub)
		{
			PreCheck_ButtonClick();
			currentOperation = Operation.Sub;
			PerformCalculation(previousOperation);
			previousOperation = currentOperation;
			EnableOperatorButtons(enable: false);
			txtDisplay.Text += (sender as Button).Text;
		}
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		if (txtDisplay.TextLength != 0)
		{
			PreCheck_ButtonClick();
			currentOperation = Operation.Add;
			PerformCalculation(previousOperation);
			previousOperation = currentOperation;
			EnableOperatorButtons(enable: false);
			txtDisplay.Text += (sender as Button).Text;
		}
	}

	private void PerformCalculation(Operation previousOperation)
	{
		try
		{
			if (previousOperation == Operation.None)
			{
				return;
			}
			List<double> list = null;
			switch (previousOperation)
			{
			case Operation.Add:
				if (currentOperation == Operation.Sub)
				{
					currentOperation = Operation.Add;
					break;
				}
				list = txtDisplay.Text.Split('+').Select(double.Parse).ToList();
				txtDisplay.Text = (list[0] + list[1]).ToString();
				break;
			case Operation.Sub:
			{
				int num = txtDisplay.Text.LastIndexOf('-');
				if (num > 0)
				{
					double num2 = Convert.ToDouble(txtDisplay.Text.Substring(0, num));
					double num3 = Convert.ToDouble(txtDisplay.Text.Substring(num + 1));
					txtDisplay.Text = (num2 - num3).ToString();
				}
				break;
			}
			case Operation.Mul:
				if (currentOperation == Operation.Sub)
				{
					currentOperation = Operation.Mul;
					break;
				}
				list = txtDisplay.Text.Split('*').Select(double.Parse).ToList();
				txtDisplay.Text = (list[0] * list[1]).ToString();
				break;
			case Operation.Div:
				if (currentOperation == Operation.Sub)
				{
					currentOperation = Operation.Div;
					break;
				}
				try
				{
					list = txtDisplay.Text.Split('/').Select(double.Parse).ToList();
					if (list[1] == 0.0)
					{
						throw new DivideByZeroException();
					}
					txtDisplay.Text = (list[0] / list[1]).ToString();
					break;
				}
				catch (DivideByZeroException)
				{
					txtDisplay.Text = "Err!";
					break;
				}
			case Operation.None:
				break;
			}
		}
		catch (Exception)
		{
			txtDisplay.Text = "SYNTAX ERROR!";
		}
	}

	private void BtnNum_Click(object btn, EventArgs e)
	{
		if (txtDisplay.Text == "SYNTAX ERROR!" || txtDisplay.Text == "Err!")
		{
			txtDisplay.Text = string.Empty;
		}
		EnableOperatorButtons();
		PreCheck_ButtonClick();
		txtDisplay.Text += (btn as Button).Text;
	}

	private void PreCheck_ButtonClick()
	{
		if (txtDisplay.Text == "Err!" || txtDisplay.Text == "SYNTAX ERROR!")
		{
			txtDisplay.Clear();
		}
		if (previousOperation != Operation.None)
		{
			EnableOperatorButtons();
		}
	}

	private void EnableOperatorButtons(bool enable = true)
	{
		btnMul.Enabled = enable;
		btnDiv.Enabled = enable;
		btnAdd.Enabled = enable;
		if (!enable)
		{
			decimalPointActive = false;
		}
	}

	private void BtnRes_Click(object sender, EventArgs e)
	{
		if (txtDisplay.TextLength != 0)
		{
			if (previousOperation != Operation.None)
			{
				PerformCalculation(previousOperation);
			}
			previousOperation = Operation.None;
		}
	}

	private void BtnDecimal_Click(object sender, EventArgs e)
	{
		if (!decimalPointActive)
		{
			if (txtDisplay.Text == "SYNTAX ERROR!" || txtDisplay.Text == "Err!")
			{
				txtDisplay.Text = string.Empty;
			}
			EnableOperatorButtons();
			PreCheck_ButtonClick();
			txtDisplay.Text += (sender as Button).Text;
			decimalPointActive = true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaclulatorDemo.Form1));
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.txtDisplay = new System.Windows.Forms.TextBox();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.btnRes = new System.Windows.Forms.Button();
		this.btn0 = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btn3 = new System.Windows.Forms.Button();
		this.btn2 = new System.Windows.Forms.Button();
		this.btn1 = new System.Windows.Forms.Button();
		this.btnSub = new System.Windows.Forms.Button();
		this.btn6 = new System.Windows.Forms.Button();
		this.btn5 = new System.Windows.Forms.Button();
		this.btn4 = new System.Windows.Forms.Button();
		this.btnMul = new System.Windows.Forms.Button();
		this.btn9 = new System.Windows.Forms.Button();
		this.btn8 = new System.Windows.Forms.Button();
		this.btnDiv = new System.Windows.Forms.Button();
		this.btnClear = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnCopy = new System.Windows.Forms.Button();
		this.btn7 = new System.Windows.Forms.Button();
		this.btnDecimal = new System.Windows.Forms.Button();
		this.tableLayoutPanel1.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		base.SuspendLayout();
		this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel1.ColumnCount = 1;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Controls.Add(this.txtDisplay, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
		this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 15);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 2;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(279, 348);
		this.tableLayoutPanel1.TabIndex = 0;
		this.txtDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtDisplay.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDisplay.Location = new System.Drawing.Point(4, 4);
		this.txtDisplay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtDisplay.Multiline = true;
		this.txtDisplay.Name = "txtDisplay";
		this.txtDisplay.ReadOnly = true;
		this.txtDisplay.Size = new System.Drawing.Size(271, 61);
		this.txtDisplay.TabIndex = 0;
		this.txtDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.tableLayoutPanel2.ColumnCount = 4;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel2.Controls.Add(this.btnRes, 3, 4);
		this.tableLayoutPanel2.Controls.Add(this.btn0, 1, 4);
		this.tableLayoutPanel2.Controls.Add(this.btnAdd, 3, 3);
		this.tableLayoutPanel2.Controls.Add(this.btn3, 2, 3);
		this.tableLayoutPanel2.Controls.Add(this.btn2, 1, 3);
		this.tableLayoutPanel2.Controls.Add(this.btn1, 0, 3);
		this.tableLayoutPanel2.Controls.Add(this.btnSub, 3, 2);
		this.tableLayoutPanel2.Controls.Add(this.btn6, 2, 2);
		this.tableLayoutPanel2.Controls.Add(this.btn5, 1, 2);
		this.tableLayoutPanel2.Controls.Add(this.btn4, 0, 2);
		this.tableLayoutPanel2.Controls.Add(this.btnMul, 3, 1);
		this.tableLayoutPanel2.Controls.Add(this.btn9, 2, 1);
		this.tableLayoutPanel2.Controls.Add(this.btn8, 1, 1);
		this.tableLayoutPanel2.Controls.Add(this.btnDiv, 3, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnClear, 2, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnReset, 1, 0);
		this.tableLayoutPanel2.Controls.Add(this.btnCopy, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.btn7, 0, 1);
		this.tableLayoutPanel2.Controls.Add(this.btnDecimal, 2, 4);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 73);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 5;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(271, 271);
		this.tableLayoutPanel2.TabIndex = 1;
		this.btnRes.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRes.Location = new System.Drawing.Point(205, 220);
		this.btnRes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnRes.Name = "btnRes";
		this.btnRes.Size = new System.Drawing.Size(62, 47);
		this.btnRes.TabIndex = 19;
		this.btnRes.Text = "=";
		this.btnRes.UseVisualStyleBackColor = true;
		this.btnRes.Click += new System.EventHandler(BtnRes_Click);
		this.btn0.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn0.Location = new System.Drawing.Point(71, 220);
		this.btn0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn0.Name = "btn0";
		this.btn0.Size = new System.Drawing.Size(59, 47);
		this.btn0.TabIndex = 17;
		this.btn0.Text = "0";
		this.btn0.UseVisualStyleBackColor = true;
		this.btn0.Click += new System.EventHandler(BtnNum_Click);
		this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.Location = new System.Drawing.Point(205, 166);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(62, 46);
		this.btnAdd.TabIndex = 15;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnAdd.Click += new System.EventHandler(BtnAdd_Click);
		this.btn3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn3.Location = new System.Drawing.Point(138, 166);
		this.btn3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn3.Name = "btn3";
		this.btn3.Size = new System.Drawing.Size(59, 46);
		this.btn3.TabIndex = 14;
		this.btn3.Text = "3";
		this.btn3.UseVisualStyleBackColor = true;
		this.btn3.Click += new System.EventHandler(BtnNum_Click);
		this.btn2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn2.Location = new System.Drawing.Point(71, 166);
		this.btn2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn2.Name = "btn2";
		this.btn2.Size = new System.Drawing.Size(59, 46);
		this.btn2.TabIndex = 13;
		this.btn2.Text = "2";
		this.btn2.UseVisualStyleBackColor = true;
		this.btn2.Click += new System.EventHandler(BtnNum_Click);
		this.btn1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn1.Location = new System.Drawing.Point(4, 166);
		this.btn1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn1.Name = "btn1";
		this.btn1.Size = new System.Drawing.Size(59, 46);
		this.btn1.TabIndex = 12;
		this.btn1.Text = "1";
		this.btn1.UseVisualStyleBackColor = true;
		this.btn1.Click += new System.EventHandler(BtnNum_Click);
		this.btnSub.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnSub.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSub.Location = new System.Drawing.Point(205, 112);
		this.btnSub.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnSub.Name = "btnSub";
		this.btnSub.Size = new System.Drawing.Size(62, 46);
		this.btnSub.TabIndex = 11;
		this.btnSub.Text = "-";
		this.btnSub.UseVisualStyleBackColor = true;
		this.btnSub.Click += new System.EventHandler(BtnSub_Click);
		this.btn6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn6.Location = new System.Drawing.Point(138, 112);
		this.btn6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn6.Name = "btn6";
		this.btn6.Size = new System.Drawing.Size(59, 46);
		this.btn6.TabIndex = 10;
		this.btn6.Text = "6";
		this.btn6.UseVisualStyleBackColor = true;
		this.btn6.Click += new System.EventHandler(BtnNum_Click);
		this.btn5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn5.Location = new System.Drawing.Point(71, 112);
		this.btn5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn5.Name = "btn5";
		this.btn5.Size = new System.Drawing.Size(59, 46);
		this.btn5.TabIndex = 9;
		this.btn5.Text = "5";
		this.btn5.UseVisualStyleBackColor = true;
		this.btn5.Click += new System.EventHandler(BtnNum_Click);
		this.btn4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn4.Location = new System.Drawing.Point(4, 112);
		this.btn4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn4.Name = "btn4";
		this.btn4.Size = new System.Drawing.Size(59, 46);
		this.btn4.TabIndex = 8;
		this.btn4.Text = "4";
		this.btn4.UseVisualStyleBackColor = true;
		this.btn4.Click += new System.EventHandler(BtnNum_Click);
		this.btnMul.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnMul.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMul.Location = new System.Drawing.Point(205, 58);
		this.btnMul.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnMul.Name = "btnMul";
		this.btnMul.Size = new System.Drawing.Size(62, 46);
		this.btnMul.TabIndex = 7;
		this.btnMul.Text = "*";
		this.btnMul.UseVisualStyleBackColor = true;
		this.btnMul.Click += new System.EventHandler(BtnMul_Click);
		this.btn9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn9.Location = new System.Drawing.Point(138, 58);
		this.btn9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn9.Name = "btn9";
		this.btn9.Size = new System.Drawing.Size(59, 46);
		this.btn9.TabIndex = 6;
		this.btn9.Text = "9";
		this.btn9.UseVisualStyleBackColor = true;
		this.btn9.Click += new System.EventHandler(BtnNum_Click);
		this.btn8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn8.Location = new System.Drawing.Point(71, 58);
		this.btn8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn8.Name = "btn8";
		this.btn8.Size = new System.Drawing.Size(59, 46);
		this.btn8.TabIndex = 5;
		this.btn8.Text = "8";
		this.btn8.UseVisualStyleBackColor = true;
		this.btn8.Click += new System.EventHandler(BtnNum_Click);
		this.btnDiv.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDiv.Location = new System.Drawing.Point(205, 4);
		this.btnDiv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnDiv.Name = "btnDiv";
		this.btnDiv.Size = new System.Drawing.Size(62, 46);
		this.btnDiv.TabIndex = 4;
		this.btnDiv.Text = "/";
		this.btnDiv.UseVisualStyleBackColor = true;
		this.btnDiv.Click += new System.EventHandler(BtnDiv_Click);
		this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnClear.Location = new System.Drawing.Point(138, 4);
		this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(59, 46);
		this.btnClear.TabIndex = 3;
		this.btnClear.Text = "Clear";
		this.btnClear.UseVisualStyleBackColor = true;
		this.btnClear.Click += new System.EventHandler(BtnClear_Click);
		this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnReset.Location = new System.Drawing.Point(71, 4);
		this.btnReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(59, 46);
		this.btnReset.TabIndex = 2;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = true;
		this.btnReset.Click += new System.EventHandler(BtnReset_Click);
		this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnCopy.Location = new System.Drawing.Point(4, 4);
		this.btnCopy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Size = new System.Drawing.Size(59, 46);
		this.btnCopy.TabIndex = 1;
		this.btnCopy.Text = "Copy";
		this.btnCopy.UseVisualStyleBackColor = true;
		this.btnCopy.Click += new System.EventHandler(BtnCopy_Click);
		this.btn7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btn7.Location = new System.Drawing.Point(4, 58);
		this.btn7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btn7.Name = "btn7";
		this.btn7.Size = new System.Drawing.Size(59, 46);
		this.btn7.TabIndex = 0;
		this.btn7.Text = "7";
		this.btn7.UseVisualStyleBackColor = true;
		this.btn7.Click += new System.EventHandler(BtnNum_Click);
		this.btnDecimal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDecimal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f);
		this.btnDecimal.Location = new System.Drawing.Point(138, 220);
		this.btnDecimal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnDecimal.Name = "btnDecimal";
		this.btnDecimal.Size = new System.Drawing.Size(59, 47);
		this.btnDecimal.TabIndex = 20;
		this.btnDecimal.Text = ".";
		this.btnDecimal.UseVisualStyleBackColor = true;
		this.btnDecimal.Click += new System.EventHandler(BtnDecimal_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(311, 378);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "Form1";
		this.Text = "Calculator";
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		this.tableLayoutPanel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
