﻿Imports System.ComponentModel.DataAnnotations
Imports System.Drawing
Imports System.IO
Imports Nukepayload2.ConsoleFramework

Public Class App
    <EntryMethod()>
    Public Sub StartUp(
        <Display(Name:="src", ShortName:="s", Description:="要比较的原图的文件夹。")>
        srcDir1$,
        <Display(Name:="cmp", ShortName:="c", Description:="要比较的新图的文件夹。")>
        srcDir2$,
        <Display(Name:="dst", ShortName:="d", Description:="输出比较结果的文件夹。")>
        destDir$,
        <Display(Name:="nogpu", ShortName:="ng", Description:="不要使用 OpenCL。")>
        Optional useOpenCL As Boolean = True,
        <Display(Name:="raw", Description:="使用 OpenCL 时不要用 Imaging 写法，而是用直接处理图像后缓冲区的写法。如果不使用 OpenCL, 这个标签会被忽略。")>
        Optional useImaging As Boolean = True,
        <Display(Name:="repeat", ShortName:="r", Description:="指定重复次数, 用于评估当前计算机采取哪种计算模式效率更高。")>
        Optional repeatCount As Integer = 1
    )
        Console.WriteLine($"图片比较器")
        Console.WriteLine($"版本: {FileVersionInfo.GetVersionInfo(GetType(Program).Assembly.Location).FileVersion}")
        Console.WriteLine("版权所有 (C) 2017 Nukepayload2。保留所有权利。")
        Dim fileCount = 0
        Dim timer As New Stopwatch
        If useOpenCL Then
            Console.Write("使用 OpenCL ")
            If useImaging Then
                Console.WriteLine("成像处理。")
            Else
                Console.WriteLine("直接处理像素点。")
            End If
        Else
            Console.WriteLine("使用 CPU 多线程处理。")
        End If
        Console.WriteLine("重复图片处理次数: " & repeatCount)
        timer.Start()
        Using clDelegate = If(useImaging, DirectCast(New ImageDiffBlend, IImageProc), New ImageDiffBlendCalc)
            For i = 1 To repeatCount
                For Each imageA In Directory.GetFiles(srcDir1)
                    Dim fn = Path.GetFileName(imageA)
                    Dim imageB = Path.Combine(srcDir2, fn)
                    If File.Exists(imageB) Then
                        Using bmpA As New Bitmap(imageA)
                            Using bmpB As New Bitmap(imageB)
                                If bmpA.Width = bmpB.Width AndAlso bmpA.Height = bmpB.Height Then
                                    Using bmpC = If(useOpenCL, clDelegate.Invoke(bmpA, bmpB), clDelegate.NoHardwareAcceleration(bmpA, bmpB))
                                        bmpC.Save(Path.Combine(destDir, fn), Imaging.ImageFormat.Png)
                                    End Using
                                End If
                            End Using
                        End Using
                    End If
                    fileCount += 1
                    If fileCount Mod 25 = 0 Then
                        Console.Write(".")
                    End If
                Next
            Next
        End Using
        timer.Stop()
        Console.WriteLine($"处理结束。处理了 {fileCount} 个文件, 用时 {timer.ElapsedMilliseconds} 毫秒。")
    End Sub
End Class
