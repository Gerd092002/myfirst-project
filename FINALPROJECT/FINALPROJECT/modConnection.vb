Imports System.Data.Odbc

Module modDB
    Public con As New OdbcConnection("Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=C:\Users\Acer\source\repos\FINALPROJECT\FINALPROJECT\bin\Debug\Students.accdb;")

    Public Sub Connect()
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
    End Sub

    Public Sub Disconnect()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
    End Sub
End Module
