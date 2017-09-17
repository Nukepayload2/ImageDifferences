Imports System.IO
Imports Microsoft.WindowsAPICodePack.Dialogs

Class PrefPage
    Private _pref As New FileIOPerformanceCounter

    Private Sub TestFilePref(procFiles As Action(Of String))
        IsEnabled = False
        Dim dirPicker As New CommonOpenFileDialog With {.IsFolderPicker = True}
        Dim result = dirPicker.ShowDialog
        If result = CommonFileDialogResult.Ok Then
            procFiles(dirPicker.FileName)
        End If
        IsEnabled = True
    End Sub

    Private Sub TestPathIOApiPref(folder As String)
        Dim timer As New Stopwatch
        _pref.PathLoaded = 0
        timer.Start()
        Dim files = Directory.GetFiles(folder)
        For Each fn In From f In files Where f.ToLower.EndsWith(_pref.Extension)
            Using strm = File.OpenRead(fn)
                Dim buffer(strm.Length - 1) As Byte
                strm.Read(buffer, 0, strm.Length)
                _pref.PathLoaded += 1
            End Using
        Next
        timer.Stop()
        _pref.PathAPI = timer.ElapsedMilliseconds
    End Sub

    Private Sub BtnFindNextFile_Click(sender As Object, e As RoutedEventArgs) Handles BtnFindNextFile.Click
        TestFilePref(Sub(f) TestPathIOApiPref(f))
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DataContext = _pref
    End Sub
End Class
