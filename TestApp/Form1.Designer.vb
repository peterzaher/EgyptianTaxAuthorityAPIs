<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.SignDocumentsBtn = New System.Windows.Forms.Button()
		Me.GetDocuments = New System.Windows.Forms.Button()
		Me.ParseJsonObjectBtn = New System.Windows.Forms.Button()
		Me.SendDocBtn = New System.Windows.Forms.Button()
		Me.NumericUpDownDocNumbers = New System.Windows.Forms.NumericUpDown()
		Me.GetSubmissionStatus = New System.Windows.Forms.Button()
		Me.SubmissionUUIDTextBox = New System.Windows.Forms.TextBox()
		Me.GetDocStatus = New System.Windows.Forms.Button()
		Me.DocUuidTxtBox = New System.Windows.Forms.TextBox()
		Me.GetEtaCredBtn = New System.Windows.Forms.Button()
		CType(Me.NumericUpDownDocNumbers, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'SignDocumentsBtn
		'
		Me.SignDocumentsBtn.Location = New System.Drawing.Point(24, 292)
		Me.SignDocumentsBtn.Name = "SignDocumentsBtn"
		Me.SignDocumentsBtn.Size = New System.Drawing.Size(80, 23)
		Me.SignDocumentsBtn.TabIndex = 0
		Me.SignDocumentsBtn.Text = "Sign Docs"
		Me.SignDocumentsBtn.UseVisualStyleBackColor = True
		'
		'GetDocuments
		'
		Me.GetDocuments.Location = New System.Drawing.Point(24, 43)
		Me.GetDocuments.Name = "GetDocuments"
		Me.GetDocuments.Size = New System.Drawing.Size(107, 25)
		Me.GetDocuments.TabIndex = 1
		Me.GetDocuments.Text = "Get Recent Docs"
		Me.GetDocuments.TextAlign = System.Drawing.ContentAlignment.BottomLeft
		Me.GetDocuments.UseVisualStyleBackColor = True
		'
		'ParseJsonObjectBtn
		'
		Me.ParseJsonObjectBtn.Location = New System.Drawing.Point(24, 137)
		Me.ParseJsonObjectBtn.Name = "ParseJsonObjectBtn"
		Me.ParseJsonObjectBtn.Size = New System.Drawing.Size(107, 23)
		Me.ParseJsonObjectBtn.TabIndex = 2
		Me.ParseJsonObjectBtn.Text = "Parse Json Object"
		Me.ParseJsonObjectBtn.UseVisualStyleBackColor = True
		'
		'SendDocBtn
		'
		Me.SendDocBtn.Location = New System.Drawing.Point(24, 238)
		Me.SendDocBtn.Name = "SendDocBtn"
		Me.SendDocBtn.Size = New System.Drawing.Size(80, 23)
		Me.SendDocBtn.TabIndex = 4
		Me.SendDocBtn.Text = "Send Doc"
		Me.SendDocBtn.UseVisualStyleBackColor = True
		'
		'NumericUpDownDocNumbers
		'
		Me.NumericUpDownDocNumbers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.NumericUpDownDocNumbers.Location = New System.Drawing.Point(182, 45)
		Me.NumericUpDownDocNumbers.Name = "NumericUpDownDocNumbers"
		Me.NumericUpDownDocNumbers.Size = New System.Drawing.Size(62, 23)
		Me.NumericUpDownDocNumbers.TabIndex = 7
		Me.NumericUpDownDocNumbers.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'GetSubmissionStatus
		'
		Me.GetSubmissionStatus.Location = New System.Drawing.Point(24, 90)
		Me.GetSubmissionStatus.Name = "GetSubmissionStatus"
		Me.GetSubmissionStatus.Size = New System.Drawing.Size(133, 23)
		Me.GetSubmissionStatus.TabIndex = 8
		Me.GetSubmissionStatus.Text = "Get Submission Status"
		Me.GetSubmissionStatus.UseVisualStyleBackColor = True
		'
		'SubmissionUUIDTextBox
		'
		Me.SubmissionUUIDTextBox.AcceptsReturn = True
		Me.SubmissionUUIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.SubmissionUUIDTextBox.Location = New System.Drawing.Point(182, 91)
		Me.SubmissionUUIDTextBox.Name = "SubmissionUUIDTextBox"
		Me.SubmissionUUIDTextBox.PlaceholderText = "Submission uuid"
		Me.SubmissionUUIDTextBox.Size = New System.Drawing.Size(143, 23)
		Me.SubmissionUUIDTextBox.TabIndex = 9
		'
		'GetDocStatus
		'
		Me.GetDocStatus.Location = New System.Drawing.Point(24, 187)
		Me.GetDocStatus.Name = "GetDocStatus"
		Me.GetDocStatus.Size = New System.Drawing.Size(107, 23)
		Me.GetDocStatus.TabIndex = 10
		Me.GetDocStatus.Text = "Get Doc Status"
		Me.GetDocStatus.UseVisualStyleBackColor = True
		'
		'DocUuidTxtBox
		'
		Me.DocUuidTxtBox.Location = New System.Drawing.Point(182, 187)
		Me.DocUuidTxtBox.Name = "DocUuidTxtBox"
		Me.DocUuidTxtBox.PlaceholderText = "Enter Document UUID"
		Me.DocUuidTxtBox.Size = New System.Drawing.Size(143, 23)
		Me.DocUuidTxtBox.TabIndex = 11
		'
		'GetEtaCredBtn
		'
		Me.GetEtaCredBtn.Location = New System.Drawing.Point(191, 246)
		Me.GetEtaCredBtn.Name = "GetEtaCredBtn"
		Me.GetEtaCredBtn.Size = New System.Drawing.Size(118, 23)
		Me.GetEtaCredBtn.TabIndex = 12
		Me.GetEtaCredBtn.Text = "Get ETA Credential"
		Me.GetEtaCredBtn.UseVisualStyleBackColor = True
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(405, 377)
		Me.Controls.Add(Me.GetEtaCredBtn)
		Me.Controls.Add(Me.DocUuidTxtBox)
		Me.Controls.Add(Me.GetDocStatus)
		Me.Controls.Add(Me.SubmissionUUIDTextBox)
		Me.Controls.Add(Me.GetSubmissionStatus)
		Me.Controls.Add(Me.NumericUpDownDocNumbers)
		Me.Controls.Add(Me.SendDocBtn)
		Me.Controls.Add(Me.ParseJsonObjectBtn)
		Me.Controls.Add(Me.GetDocuments)
		Me.Controls.Add(Me.SignDocumentsBtn)
		Me.Name = "Form1"
		Me.Text = "Read Doc File"
		CType(Me.NumericUpDownDocNumbers, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents SignDocumentsBtn As Button
	Friend WithEvents GetDocuments As Button
	Friend WithEvents ParseJsonObjectBtn As Button
	Friend WithEvents SendDocBtn As Button
	Friend WithEvents NumericUpDownDocNumbers As NumericUpDown
	Friend WithEvents GetSubmissionStatus As Button
	Friend WithEvents SubmissionUUIDTextBox As TextBox
	Friend WithEvents GetDocStatus As Button
	Friend WithEvents DocUuidTxtBox As TextBox
	Friend WithEvents GetEtaCredBtn As Button
End Class
