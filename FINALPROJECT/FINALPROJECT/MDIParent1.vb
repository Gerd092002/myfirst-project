Imports System.Data.Odbc

Public Class MDIParent1
    Public Sub LoadStudents()
        lvStudents.Items.Clear()
        Connect()
        Dim cmd As New OdbcCommand("SELECT * FROM Students", con)
        Dim dr As OdbcDataReader = cmd.ExecuteReader()
        While dr.Read()
            Dim item As New ListViewItem(dr("ID").ToString())
            item.SubItems.Add(dr("Full_Name").ToString())
            item.SubItems.Add(dr("Program").ToString())
            item.SubItems.Add(dr("Year_level").ToString())
            lvStudents.Items.Add(item)
        End While
        Disconnect()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        lvStudents.Items.Clear()
        Connect()
        Dim cmd As New OdbcCommand("SELECT * FROM Students WHERE Full_Name LIKE ?", con)
        cmd.Parameters.AddWithValue("@search", "%" & txtSearch.Text & "%")
        Dim dr As OdbcDataReader = cmd.ExecuteReader()
        While dr.Read()
            Dim item As New ListViewItem(dr("ID").ToString())
            item.SubItems.Add(dr("Full_Name").ToString())
            item.SubItems.Add(dr("Program").ToString())
            item.SubItems.Add(dr("Year_level").ToString())
            lvStudents.Items.Add(item)
        End While
        Disconnect()
    End Sub

    Private Sub ShowAddStudent(Optional editMode As Boolean = False, Optional id As Integer = 0, Optional name As String = "", Optional course As String = "", Optional year As String = "")
        txtSearch.Visible = False
        lvStudents.Visible = False


        pnlMainArea.Controls.Clear()


        For Each ctrl As Control In pnlMainArea.Controls
            If TypeOf ctrl Is StudentAdd Then
                ctrl.Dispose()
            End If
        Next

        Dim form As New StudentAdd()
        form.MdiParent = Me
        form.TopLevel = False
        form.FormBorderStyle = FormBorderStyle.None
        form.Dock = DockStyle.Fill

        If editMode Then
            form.SetEditMode(id, name, course, year)
        End If

        pnlMainArea.Controls.Add(form)
        form.Show()
    End Sub

    Public Sub ShowManageStudents()
        pnlMainArea.Controls.Clear()


        txtSearch.Visible = True
        lvStudents.Visible = True
        lblSearch.Visible = True

        pnlMainArea.Controls.Add(lblSearch)
        pnlMainArea.Controls.Add(txtSearch)
        pnlMainArea.Controls.Add(lvStudents)

        txtSearch.BringToFront()
        lvStudents.BringToFront()

        LoadStudents()
    End Sub

    Private Sub ExitToolStrip_Click(sender As Object, e As EventArgs) Handles ExitToolStrip.Click
        Me.Close()
    End Sub

    Private Sub AddStudentToolStrip_Click(sender As Object, e As EventArgs) Handles AddStudentToolStrip.Click
        ShowAddStudent()
    End Sub

    Private Sub ManageStudentToolStrip_Click(sender As Object, e As EventArgs) Handles ManageStudentToolStrip.Click
        ShowManageStudents()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.ShowDialog()
    End Sub

    Private Sub AddStudentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddStudentToolStripMenuItem.Click
        ShowAddStudent()
    End Sub

    Private Sub ManageStudentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageStudentToolStripMenuItem.Click
        ShowManageStudents()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub lvStudents_DoubleClick(sender As Object, e As EventArgs) Handles lvStudents.DoubleClick
        If lvStudents.SelectedItems.Count > 0 Then
            Dim selectedItem As ListViewItem = lvStudents.SelectedItems(0)
            Dim id As Integer = CInt(selectedItem.SubItems(0).Text)
            Dim name As String = selectedItem.SubItems(1).Text
            Dim course As String = selectedItem.SubItems(2).Text
            Dim year As String = selectedItem.SubItems(3).Text

            ShowAddStudent(True, id, name, course, year)
        End If
    End Sub

    Private Sub lvStudents_MouseClick(sender As Object, e As MouseEventArgs) Handles lvStudents.MouseClick
        If e.Button = MouseButtons.Right AndAlso lvStudents.FocusedItem IsNot Nothing AndAlso lvStudents.FocusedItem.Bounds.Contains(e.Location) Then
            ContextMenuStrip1.Show(Cursor.Position)
        End If
    End Sub

    Private Sub MDIParent1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowManageStudents()
    End Sub
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        If lvStudents.SelectedItems.Count > 0 Then
            Dim selectedItem As ListViewItem = lvStudents.SelectedItems(0)
            Dim id As Integer = CInt(selectedItem.SubItems(0).Text)
            Dim name As String = selectedItem.SubItems(1).Text
            Dim course As String = selectedItem.SubItems(2).Text
            Dim year As String = selectedItem.SubItems(3).Text

            ShowAddStudent(True, id, name, course, year)
        End If
    End Sub
    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If lvStudents.SelectedItems.Count > 0 Then
            Dim selectedItem As ListViewItem = lvStudents.SelectedItems(0)
            Dim id As Integer = CInt(selectedItem.SubItems(0).Text)

            Dim confirm = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = DialogResult.Yes Then
                Connect()
                Dim cmd As New OdbcCommand("DELETE FROM Students WHERE ID = ?", con)
                cmd.Parameters.AddWithValue("@ID", id)
                cmd.ExecuteNonQuery()
                Disconnect()

                MessageBox.Show("Student deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadStudents()
            End If
        End If
    End Sub
End Class