<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
	Inherits System.Windows.Forms.Form

	'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

	'Requise par le Concepteur Windows Form
	Private components As System.ComponentModel.IContainer

	'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
	'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
	'Ne la modifiez pas à l'aide de l'éditeur de code.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.ButtonWmi = New System.Windows.Forms.Button()
        Me.TextBoxDisque = New System.Windows.Forms.TextBox()
        Me.ButtonDriveInfo = New System.Windows.Forms.Button()
        Me.TextBoxPerf = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonWmi
        '
        resources.ApplyResources(Me.ButtonWmi, "ButtonWmi")
        Me.ButtonWmi.Name = "ButtonWmi"
        Me.ButtonWmi.UseVisualStyleBackColor = True
        '
        'TextBoxDisque
        '
        resources.ApplyResources(Me.TextBoxDisque, "TextBoxDisque")
        Me.TextBoxDisque.Name = "TextBoxDisque"
        '
        'ButtonDriveInfo
        '
        resources.ApplyResources(Me.ButtonDriveInfo, "ButtonDriveInfo")
        Me.ButtonDriveInfo.Name = "ButtonDriveInfo"
        Me.ButtonDriveInfo.UseVisualStyleBackColor = True
        '
        'TextBoxPerf
        '
        resources.ApplyResources(Me.TextBoxPerf, "TextBoxPerf")
        Me.TextBoxPerf.Name = "TextBoxPerf"
        Me.TextBoxPerf.ReadOnly = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Sj.HDSerialNumber.IHM.My.Resources.Resources.sj250x109
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'FormMain
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TextBoxPerf)
        Me.Controls.Add(Me.ButtonDriveInfo)
        Me.Controls.Add(Me.TextBoxDisque)
        Me.Controls.Add(Me.ButtonWmi)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Name = "FormMain"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Friend WithEvents ButtonWmi As System.Windows.Forms.Button
	Friend WithEvents TextBoxDisque As System.Windows.Forms.TextBox
	Friend WithEvents ButtonDriveInfo As System.Windows.Forms.Button
	Friend WithEvents TextBoxPerf As System.Windows.Forms.TextBox
	Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

End Class
