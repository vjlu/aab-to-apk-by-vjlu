Imports System, System.Net, System.IO, System.Text.RegularExpressions, System.Threading, System.IO.Compression

Public Class Form1
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        End
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim o As OpenFileDialog = OpenFileDialog1
        o.Title = ".AAB File"
        o.FileName = "Chose your .aab file"
        o.Filter = " aab files (*.aab)|*.aab"
        If o.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            TextBox1.Text = OpenFileDialog1.FileName.ToString
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim o As OpenFileDialog = OpenFileDialog2
        o.Title = ".keystore File"
        o.FileName = "Chose your .keystore file"
        o.Filter = " keystore files (*.keystore)|*.keystore"
        If o.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            TextBox2.Text = OpenFileDialog2.FileName.ToString
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Dim boo As Integer
        boo = MsgBox("Hi" + vbCrLf + "if you already have a keystore file you can use it" + vbCrLf + "but if you don't have one you can" + vbCrLf + "generate a new one by clicking on" + vbCrLf + "Generate keystore file or click on (Yes)" + vbCrLf + vbCrLf + vbCrLf + "Do you want to Generate new a keystore file?", MessageBoxButtons.YesNo + MessageBoxIcon.Information, ".Keystore file info")
        If boo = 6 Then
            Try
                Dim myProcess = Process.Start("cmd", "/k keytool -genkey -v -keystore debug.keystore -alias debug -keyalg RSA -keysize 2048 -validity 10000")
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try
            Dim myProcess = Process.Start("cmd", "/k keytool -genkey -v -keystore debug.keystore -alias debug -keyalg RSA -keysize 2048 -validity 10000")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox4.Text = "" Then
            TextBox4.Text = "myapp"
        End If
        Try

            Dim myProcess = Process.Start("cmd", "/c java -jar bundletool.jar build-apks --bundle=" + TextBox1.Text + " --output=" + TextBox4.Text + ".apks --mode=universal --ks=" + TextBox2.Text + " --ks-key-alias=debug")

        Catch ex As Exception

        End Try
    End Sub
    Dim t As Thread = New Thread(AddressOf Checkapk)
    Dim hh As Thread = New Thread(AddressOf extract)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        t.IsBackground = True
        t.Start()
        hh.IsBackground = True
        hh.Start()
    End Sub
    Private Sub Checkapk()
        Dim b As Boolean = File.Exists(TextBox4.Text + ".apks")
        While b = False
            Try
                b = File.Exists(TextBox4.Text + ".apks")
                If b = True Then
                    File.Copy(TextBox4.Text + ".apks", TextBox4.Text + ".zip")
                    t.Abort()
                    t.IsBackground = False

                End If
            Catch ex As Exception
            End Try
            Label4.Text += 1
            Thread.Sleep(500)
        End While
    End Sub
    Dim once As Integer = 0
    Private Sub extract()
        Dim b As Boolean = File.Exists("universal.apk")
        While b = False
            Try
                If once = 0 Then
                    ZipFile.ExtractToDirectory(Application.StartupPath + "\" + TextBox4.Text + ".zip", Application.StartupPath)
                    once += 1
                End If
                b = File.Exists("universal.apk")
                If b = True Then
                    hh.Abort()
                    hh.IsBackground = False
                End If
            Catch ex As Exception
            End Try
            Thread.Sleep(500)
        End While

    End Sub
End Class
