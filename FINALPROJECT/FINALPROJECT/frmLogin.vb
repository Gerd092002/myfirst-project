Public Class frmLogin
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDate.Text = DateTime.Now.ToString("D")
        lblTime.Text = DateTime.Now.ToString("T")
    End Sub

    Private Sub cbSHOW_CheckedChanged(sender As Object, e As EventArgs) Handles cbSHOW.CheckedChanged
        If cbSHOW.Checked = True Then

            txtPassword.UseSystemPasswordChar = False
        Else

            txtPassword.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub BTNCLEAR_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtPassword.Text = ""
        txtUsername.Text = ""
        cbSHOW.Checked = False
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblDate.Text = DateTime.Now.ToString("D")
        lblTime.Text = DateTime.Now.ToString("T")
        Timer1.Start()
    End Sub

    Private Sub txtPASS_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnLogin.PerformClick()
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtUsername.Text = "admin" AndAlso txtPassword.Text = "admin123" Then
            MDIParent1.Show()
            Me.Hide()
        Else
            MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

End Class
