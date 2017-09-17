Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.WindowsAPICodePack.Dialogs

Class MainWindow

    Declare Sub RtlCopyMemory Lib "ntdll.dll" (dest As IntPtr, src As IntPtr, length As IntPtr)

    Dim image1, image2 As String
    Private Sub ImgImage_Drop(sender As Object, e As DragEventArgs) Handles ImgImage1.Drop, ImgImage2.Drop
        Dim path$ = e.Data.GetData(DataFormats.FileDrop)(0)
        DirectCast(sender, Shapes.Rectangle).Fill = New ImageBrush(New BitmapImage(New Uri(path, UriKind.Absolute)))
        If sender Is ImgImage1 Then
            image1 = path
        Else
            image2 = path
        End If
        If image1 IsNot Nothing AndAlso image2 IsNot Nothing Then
            Using imgBlend As New ImageDiffBlend
                Dim imgOut As Bitmap = imgBlend.Invoke(New Bitmap(image1), New Bitmap(image2))
                imgOut.Save(IO.Path.Combine(IO.Path.GetDirectoryName(image1), "test.png"), ImageFormat.Png)
                Dim resultBmpWpf As New WriteableBitmap(imgOut.Width, imgOut.Height, 96, 96,
                                                        PixelFormats.Bgra32, Nothing)
                Dim bmpData = imgOut.LockBits(New Rectangle(0, 0, imgOut.Width, imgOut.Height),
                                              ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
                resultBmpWpf.Lock()
                RtlCopyMemory(resultBmpWpf.BackBuffer, bmpData.Scan0, bmpData.Stride * bmpData.Height)
                resultBmpWpf.AddDirtyRect(New Int32Rect(0, 0, imgOut.Width, imgOut.Height))
                resultBmpWpf.Unlock()
                imgOut.UnlockBits(bmpData)
                ImgImage3.Fill = New ImageBrush(resultBmpWpf)
            End Using
        End If
    End Sub

End Class
