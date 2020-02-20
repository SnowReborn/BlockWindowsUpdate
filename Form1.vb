Imports System.ServiceProcess
Public Class Form1

    'Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vkey As System.Windows.Forms.Keys) As Short //for hotkey

    Private Sub TimToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TimToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Dim WindowsUpdateService As New System.ServiceProcess.ServiceController("wuauserv")
            Dim ST As String = WindowsUpdateService.StartType
            Label2.Text = "Disabled"
            Label2.ForeColor = Color.ForestGreen
            Label3.Text = WindowsUpdateService.Status
            If ST IsNot "4" Or WindowsUpdateService.Status = System.ServiceProcess.ServiceControllerStatus.Running Then
                Shell("sc stop wuauserv", AppWinStyle.Hide)
                Shell("sc config wuauserv start= disabled", AppWinStyle.Hide)
            End If
        Catch ex As Exception
            MsgBox("Attempt to disable Update Failed", MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim aPath As String = Application.ExecutablePath
            Dim RegKey As String = "powershell $Sta = New-ScheduledTaskAction -Execute " + "'" + aPath + "'" + " ; $Stt = New-ScheduledTaskTrigger -AtLogOn ; $STPrin = New-ScheduledTaskPrincipal -GroupId " + """" + "BUILTIN\Administrators" + """" + " -RunLevel Highest ;  $Stset = New-ScheduledTaskSettingsSet -ExecutionTimeLimit 2 -AllowStartIfOnBatteries -DisallowHardTerminate ;Register-ScheduledTask WinUpdateDisabler -Action $Sta -Settings $Stset -Principal $STPrin -Trigger $Stt"
            Shell(RegKey, AppWinStyle.Hide)
        Catch ex As Exception
            MsgBox("Error Creating Tasks", MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim aPath As String = Application.ExecutablePath
            Dim UnRegKey As String = "powershell Unregister-ScheduledTask -TaskName " + """" + "WinUpdateDisabler" + """" + " -Confirm:$False"
            Shell(UnRegKey, AppWinStyle.Hide)
        Catch ex As Exception
            MsgBox("Error Creating Tasks", MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Timer1.Enabled = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Timer1.Enabled = False
        Label2.Text = "Unblocked"
        Label2.ForeColor = Color.Orange
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Show()
            Me.WindowState = FormWindowState.Normal
            Me.BringToFront()
        Else
            Me.Hide()
            Me.WindowState = FormWindowState.Minimized
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        NotifyIcon1.Visible = False
        NotifyIcon1.Visible = True
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Timer1.Enabled = False
        Label2.Text = "Running"
        Label2.ForeColor = Color.Red
        Shell("sc config wuauserv start=demand", AppWinStyle.Hide)
        Shell("sc start wuauserv", AppWinStyle.Hide)
    End Sub

    Private Sub ShowInterfaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowInterfaceToolStripMenuItem.Click
        Me.Show()
    End Sub


End Class
