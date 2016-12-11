Imports KiteDotNet
Imports System.IO
Imports System.Globalization

Public Class Form1
    Dim Kite As Kite = New Kite 'Initialize
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.api_key
        TextBox2.Text = My.Settings.api_secret
        RichTextBox1.Text = "Sample App Built with KiteDotNet"
        AddHandler Kite.QuotesReceivedEvent, AddressOf Quotes_Received
    End Sub

    Delegate Sub SetRichBoxCallback(ByVal msg As String)
    Public Sub SetRichBox(ByVal msg As String)
        Try
            Dim tt As String = DateTime.Now.ToString("HH:mm:ss") & ": "
            If RichTextBox1.InvokeRequired Then
                Dim d As New SetRichBoxCallback(AddressOf SetRichBox)
                Me.RichTextBox1.Invoke(d, New Object() {msg})
            Else
                RichTextBox1.Text &= Environment.NewLine & tt & msg
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MsgBox("Please Enter Api Key")
            Exit Sub
        End If

        If TextBox2.Text = "" Then
            MsgBox("Please Enter Api Secret")
            Exit Sub
        End If

        My.Settings.api_key = TextBox1.Text
        My.Settings.api_secret = TextBox2.Text
        My.Settings.Save()

        Kite.Api_Key = TextBox1.Text

        Kite.Api_Secret = TextBox2.Text

        Kite.Stream_Mode = Mode.Full

        Try
            Kite.Login()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        RichTextBox1.SelectionStart = RichTextBox1.Text.Length
        RichTextBox1.ScrollToCaret()
    End Sub

    Public Sub Quotes_Received(sender As Object, e As KiteDotNet.QuotesReceivedEventArgs)
        Try
            SetRichBox(e.TrdSym & " " & e.Ltp & " " & e.Ltq)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox3.Text = "" Then
            MsgBox("Please Enter Exch")
            Exit Sub
        End If

        If TextBox4.Text = "" Then
            MsgBox("Please Enter TrdSymbol")
            Exit Sub
        End If

        Try
            Kite.SubscribeQuotes(TextBox3.Text, TextBox4.Text)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Kite.Instruments_Download_Status Then
            MsgBox("Instruments Downloaded")
        Else
            MsgBox("Instruments not Downloaded or in Progress")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If TextBox3.Text = "" Then
            MsgBox("Please Enter Exch")
            Exit Sub
        End If

        If TextBox4.Text = "" Then
            MsgBox("Please Enter TrdSymbol")
            Exit Sub
        End If

        Try
            Kite.UnSubscribeQuotes(TextBox3.Text, TextBox4.Text)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Kite.Logout()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If Kite.Login_Status Then
            MsgBox("Logged-In")
        Else
            MsgBox("Not Logged-In")
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Kite.StartWebSocket()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            Kite.StopWebSocket()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click

        If TextBox5.Text = "" Then
            MsgBox("Please Enter OrderId")
            Exit Sub
        End If
        Try
            MsgBox("Order Status for OrderId:" & TextBox5.Text & " : " & Kite.GetOrderStatus(TextBox5.Text))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        If Kite.IsWebSockLive Then
            MsgBox("WebSocket Live")
        Else
            MsgBox("WebSocket Not Live")
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        If Kite.Logout_Status Then
            MsgBox("Logged-Out")
        Else
            MsgBox("Not Logged-Out")
        End If
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Try
            Kite.GetAccessToken()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        If Kite.Access_Token_Status Then
            MsgBox("Access Token Received")
        Else
            MsgBox("Access Token not Received")
        End If
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Try
            Kite.GetInstruments()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try
            MsgBox(Kite.GetHoldings())
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Try
            MsgBox(Kite.GetOrderBook())
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            MsgBox(Kite.GetTradeBook())
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            MsgBox(Kite.GetPositions())
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Try
            MsgBox(Kite.GetFunds())
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        Try
            Dim Exch As String = InputBox("Enter Exch", "Exch", "NSE")
            Dim Trdsym As String = InputBox("Enter TrdSymbol", "TrdSymbol", "ICICIBANK")
            Dim Trans As String = InputBox("Enter Transaction Type", "Transaction", "BUY")
            Dim QtyStr As String = InputBox("Enter Qty", "Qty", "10")
            Dim Qty As Integer = 0
            Integer.TryParse(QtyStr, Qty)
            Dim OrderId As String = Kite.PlaceRegularOrder(Exch, Trdsym, Trans, "MARKET", Qty, "MIS")
            MsgBox("Order Placed Successfully. OrderId is " & OrderId)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        Try
            Dim OrderId As String = InputBox("Enter OrderId", "OrderId", "")
            MsgBox(Kite.GetOrderDetails(OrderId))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

End Class
