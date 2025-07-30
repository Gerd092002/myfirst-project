Imports System.Data.Odbc

Public Class StudentAdd
    Private isEditMode As Boolean = False
    Private editID As Integer = 0

    Public Sub SetEditMode(id As Integer, name As String, course As String, year As String)
        isEditMode = True
        editID = id

        txtID.Text = id.ToString()
        txtFullName.Text = name
        cmbCourse.Text = course
        cmbYear.Text = year

        btnSave.Text = "Update"
        txtID.Enabled = False
    End Sub

    Private Sub StudentAdd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolTip1.SetToolTip(btnSave, "Click to save or update student record.")
        ToolTip1.SetToolTip(txtID, "Enter a numeric ID.")
        ToolTip1.SetToolTip(txtFullName, "Enter the full name.")
        ToolTip1.SetToolTip(cmbCourse, "Select course.")
        ToolTip1.SetToolTip(cmbYear, "Select year level.")
        Me.KeyPreview = True
    End Sub

    Private Sub StudentAdd_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub txtFullName_TextChanged(sender As Object, e As EventArgs) Handles txtFullName.TextChanged
        Dim start = txtFullName.SelectionStart
        txtFullName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFullName.Text.ToLower())
        txtFullName.SelectionStart = start
    End Sub

    Private Sub btnSave_Click_1(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtFullName.Text = "" Or cmbCourse.Text = "" Or cmbYear.Text = "" Then
            ToolTip1.SetToolTip(btnSave, "Please fill in all fields.")
            MessageBox.Show("All fields are required.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Connect()

        If isEditMode Then
            Dim cmd As New OdbcCommand("UPDATE Students SET Full_Name = ?, Program = ?, Year_Level = ? WHERE ID = ?", con)
            cmd.Parameters.AddWithValue("@Full_Name", txtFullName.Text)
            cmd.Parameters.AddWithValue("@Program", cmbCourse.Text)
            cmd.Parameters.AddWithValue("@Year_Level", cmbYear.Text)
            cmd.Parameters.AddWithValue("@ID", editID)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Student updated successfully!")
        Else
            If Not IsNumeric(txtID.Text) Then
                MessageBox.Show("ID must be a number!", "Invalid!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Disconnect()
                Return
            End If

            Dim cmd As New OdbcCommand("INSERT INTO Students (ID, Full_Name, Program, Year_Level) VALUES (?, ?, ?, ?)", con)
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text))
            cmd.Parameters.AddWithValue("@Full_Name", txtFullName.Text)
            cmd.Parameters.AddWithValue("@Program", cmbCourse.Text)
            cmd.Parameters.AddWithValue("@Year_Level", cmbYear.Text)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Student added successfully!")
        End If

        Disconnect()

        Dim parentForm As MDIParent1 = TryCast(Me.MdiParent, MDIParent1)
        If parentForm IsNot Nothing Then
            parentForm.ShowManageStudents()
        End If

        Me.Close()
    End Sub

    Private Sub btnClear_Click_1(sender As Object, e As EventArgs) Handles btnClear.Click
        txtID.Text = ""
        txtFullName.Text = ""
        cmbCourse.SelectedIndex = -1
        cmbYear.SelectedIndex = -1
        txtID.Enabled = True
        btnSave.Text = "Save"
        isEditMode = False

    End Sub
End Class
