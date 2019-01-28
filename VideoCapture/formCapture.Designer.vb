<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class formCapture
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnrecord = New System.Windows.Forms.Button
        Me.capturewin = New System.Windows.Forms.Panel
        Me.pbwin = New System.Windows.Forms.PictureBox
        Me.capturewin.SuspendLayout()
        CType(Me.pbwin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnrecord
        '
        Me.btnrecord.Location = New System.Drawing.Point(108, 238)
        Me.btnrecord.Name = "btnrecord"
        Me.btnrecord.Size = New System.Drawing.Size(61, 23)
        Me.btnrecord.TabIndex = 0
        Me.btnrecord.Text = "录制"
        Me.btnrecord.UseVisualStyleBackColor = True
        Me.btnrecord.Visible = False
        '
        'capturewin
        '
        Me.capturewin.Controls.Add(Me.pbwin)
        Me.capturewin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.capturewin.Location = New System.Drawing.Point(0, 0)
        Me.capturewin.Name = "capturewin"
        Me.capturewin.Size = New System.Drawing.Size(292, 226)
        Me.capturewin.TabIndex = 1
        '
        'pbwin
        '
        Me.pbwin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbwin.Location = New System.Drawing.Point(0, 0)
        Me.pbwin.Name = "pbwin"
        Me.pbwin.Size = New System.Drawing.Size(292, 226)
        Me.pbwin.TabIndex = 0
        Me.pbwin.TabStop = False
        '
        'formCapture
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 226)
        Me.Controls.Add(Me.capturewin)
        Me.Controls.Add(Me.btnrecord)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "formCapture"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "视频窗口"
        Me.capturewin.ResumeLayout(False)
        CType(Me.pbwin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnrecord As System.Windows.Forms.Button
    Friend WithEvents capturewin As System.Windows.Forms.Panel
    Friend WithEvents pbwin As System.Windows.Forms.PictureBox
End Class
