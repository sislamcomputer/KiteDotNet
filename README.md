### KiteDotNet
    .Net Library for Kite Connect of Zerodha.

### Kite Connect API
    Kite Connect is a set of REST-like APIs that expose many capabilities required to build a complete 
    investment and trading platform.Execute orders in real time, manage user portfolio, stream live market data (WebSockets), 
    and more, with the simple HTTP API collection 
### About KiteDotNet Library
    This library supports all methods/functions exposed by Kite Connect API and it's a wrapper built around Kite Connect API. 
    (See the Kite Connect API documentation for the complete list of APIs, supported parameters and values, and response formats.) 
    This library can be effectively used to build Rich UI applications or Console Applications or even Build COM library that 
    can be used by external applications like excel etc. 
    KiteDotNet wraps all the Raw Http API calls and Json responses of Kite Connect in a Simple to Use Function and returns data 
    that can be directly used or analysed.(Credits:- NewtonsoftJson/James NK)
    Client needs to Just call the Functions/Methods by passing correct parameters. 

    Example:-
    To see the Holdings. 
    Just call Kite.GetHoldings() 'This will return string with comma seperated values and each record seperated by new Line. 
    You can just save it in Csv format or analyse. 
    Kite.CancelRegularOrder("160012345656") 'This will cancel the Order 
    Other then the standard methods, KiteDotNet also comes with some custom functions to make life more easy. 

    Example:- 
    Kite.GetSymbols("NSE") 'This retuns a string with all Trade symbols of NSE seperated by Comma and canbe hooked to ComboBox. 
    Kite.GetInstToken("NSE","BHEL") 'This returns the Instrument Token for the given parameters. 

    KiteDotNet also has custom Exceptions that will be raised whenever client passes wrong paramerts. 

    Example:-
    If client passes 0 in a Limit Order, 
    unlike the other where the order request is sent and exception is raised from Kite.Trade, 
    KiteDotNet will raise exception "InvalidPriceException" within the client and will not send order 
    request to Kite.Trade.This may little bit reduce load in Kite.Trade.

    Another good example is 
    In a Buy SL order, if the client passes Trigger Price higher than the Limit Price, 
    KiteDotNet will raise Exception "BuySLPriceExeption"
    Basically, these are designed to restrict invalid request to reach Kite.Trade.

    (See KiteDotNet Library documentation for complete list of Exception) 

    GetInstruments call will download instruments list in background. 
    This call will download instruments from Kite.Trade only once perday and at the first login 
    and stores the data in text files which will be used for Further login. 
    So First login of the day always take time as it downloads from Kite.Trade and download 
    time depend on Network Speed and at time you are calling. 
    Subsequents login will be super fast. 

    Tip:- Start your client early morning say 7.00 for Just downloading the instruments 
    and close. 
### WebSocket
    Websocket streaming is made very easy with KiteDotNet.(Credits:-WebSocket4Net/Kerry Jiang)
    This Library completely removes the headache of unpacking, prasing and casting raw byes into Strcut. 
    All the heavy load works are done in background and client is provided with actual data that can be readily 
    used for analysis.Supports all modes of streaming.Client can seamlessly switch between any mode. 
    Automatic reconnection of WebSocket if network lost and resubscribes to all symbols that are already subscribed. 
    Supports Index quotes as well. 
    KiteDotNet has special event named "QuoteReceivedEvent" which is raised whenever new data arrives from Kite.Trade 
    Raised with passing "QuotesReceivedEventArgs" class which has properties for all price fields supported by Kite WebSockets. 
    Client Just get the Required property and use directly. 
###LoginFlow
    This library comes with a WebBrowser control that will be used to Login and the Request token is 
    automatically set. 
    Kite.Login() 'will open WebBrowser
###  Caution:-
    Call all methods/Functions in Try....Catch...End Try blcok to catch exceptions raised and for further debugging. 
### Requirements
    .Net Framework 4.0 or above
#### [KiteDotNet Library Documentation] (http://www.howutrade.in/docs/kitedotnet/)
#### [Kite API Reference] (https://kite.trade/docs/connect/v1/)
#### [Kite Forum] (https://kite.trade/forum/)
#### [HowUTrade] (https://www.howutrade.in/)


### A Simple Usage of KiteDotNet
```
'Below is the quick sample code to get started.

'Here We create new Instance of Kite
'Login To Kite
'Get Access Token
'Download Instruments
'Add a Handler to Receive Quotes
'Run a Simple Trading System
'Place Orders when Signal Comes
'Logout
'Cleanup

'We will Create 3 Procedures
'Login_Kite - For Login Flows and Instruments download
'Quotes_Received - To Receive Quotes
'Simple_TradingSystem - Run a Sample Trade System

'Download the KiteDotNet.dll , WebServ.dll, Websocket4Net.dll and Newtonsoft.Json.Dll from the Library
'Create a New Project SampleKite in Visual Studio
'Note:- Visual Studio 2012 Express for Desktop is free version and Get it from Microsoft Webite
'Copy all those files to Bin\Debug of Your Project

'Add Reference to KiteDotNet.dll Only. Other Dll's need not reference, but should be placed in your App directory

'Import KiteDotNet Namespace

Imports KiteDotNet
Imports System.IO

Public Class SampleKite

    'Create a New Instance of Kite Class globally
    Dim Kite As Kite = New Kite

    'We will declare a Dictionay to save Ltp Quotes for using in Trading System
    Dim DictLtp As New Dictionary(Of String, Double)

    Public Sub Main()

        Login_Kite()

        Simple_TradingSystem()

    End Sub

    Private Sub Login_Kite()
        Try
            'Set API Key
            Kite.Api_Key = "sgsg458xxxxx"

            'Set Access Secret
            Kite.Api_Secret = "sxfshshxxxxxx"

            'Set Stream Mode
            Kite.Stream_Mode = Mode.Full 'Default is Full.Only Set if Required

            'Login to Kite
            'This will open a WebBrowser for login
            'On Sucessful Login, Request Token will be automatically extracted and set in Kite
            Kite.Login()

            'After successful login, We request for Access Token
            Kite.GetAccessToken()

            'Once the access token is received
            'We will first download the Instruments
            'Below call will download instruments from Kite.Trade
            'Downloading is done in background
            Kite.GetInstruments()

            'We should wait until the Download finishes for any further call
            'as further functions depends on Trade symbol and Instrument Tokens
            'Kite.Instruments_Download_Status will give the download status

            Do
                Threading.Thread.Sleep(1000)
            Loop While Kite.Instruments_Download_Status = False

            'Once the download is finished then Kite.Instruments_Download_Status will become true and our waiting loop will end
            'At this time, WebSocket connection automatically established after downloading is finished.
            'We now add a Handler to receive Quotes

            AddHandler Kite.QuotesReceivedEvent, AddressOf Quotes_Received
            'The above will enable Quotes_Received procedure to handle Kite.QuotesReceivedEvent as and when raised

            'Now we will set up a simple Trading System for ICICIBANK - NSE
            'and Fire Order when signal comes
            'Buy when Prices crosses 0.75% above and Book Profit when the price moves above by 2points
            'or Book loss when price moves below by 2points

        Catch ex As Exception
            MsgBox(ex.Message) 'Display any Exceptions raised
        End Try
    End Sub

    Private Sub Simple_TradingSystem()
        Try
            'We will first add a Dict Key for ICICIBANK to Store Ltp
            If Not DictLtp.TryGetValue("ICICIBANK", 0) Then 'Check whether Key Exists or Not
                DictLtp.Add("ICICIBANK", 0)
            End If

            'We will now Subscribe for Quotes
            Kite.SubscribeQuotes("NSE", "ICICIBANK")
            'The above call starts receiving quotes

            Dim ExitTradingSystem As Boolean = False
            Dim IsBought As Boolean = False
            Dim IsSold As Boolean = False

            'We will construct the Open-High-Low from Ltp
            'But this fields are already available in Quotes_Received
            'For Demo we are not going to use that instead will use Ltp to construct Open-High-Low
            'Assume that you are running from start of the Market Hours

            Dim Open As Double = 0
            Dim High As Double = 0
            Dim Low As Double = 0

            Dim BuyPrice As Double = 0
            Dim BuyTgtPrice As Double = 0
            Dim BuySlPrice As Double = 0

            Do
                Threading.Thread.Sleep(1000) ' Loop Interval 1Sec
                Dim ltp As Double = DictLtp("ICICIBANK") ' Get the Ltp From the Key.This Key will be keep on updated by Quotes_Received
                If ltp = 0 Then ' If Ltp is Zero, Continue
                    Continue Do
                End If

                If Open = 0 Then ' This will happen only once at Start
                    Open = ltp
                    High = ltp
                    Low = ltp
                End If

                High = If(ltp > High, ltp, High) 'If ltp is larger that High then new High will be Ltp
                Low = If(ltp < Low, ltp, Low) 'If ltp is larger that High then new High will be Ltp

                If Not IsBought Then 'Only Execute if you have no Buy Signal earlier
                    BuyPrice = Open + (Open * 0.0075) '0.75% above Open
                    If ltp >= BuyPrice Then ' Yes You got signal..Next Place order
                        IsBought = True 'If you not set this true, then it will keep on firing Orders
                        Kite.PlaceRegularOrder("NSE", "ICICIBANK", "BUY", "MARKET", 150, "MIS") 'Buy Qty 150 at Market
                        'Order Placed successfully
                    End If
                End If

                If IsBought Then 'Execute Only if Bought
                    BuyTgtPrice = BuyPrice + 2 '2points from buy price
                    BuySlPrice = BuyPrice - 2

                    If ltp >= BuyTgtPrice Then ' If Tgt Hits.Place Sell Order
                        IsSold = True
                        ExitTradingSystem = True
                        Kite.PlaceRegularOrder("NSE", "ICICIBANK", "SELL", "MARKET", 150, "MIS") 'Buy Qty 150 at Market
                        'Order Placed successfully
                        'You Booked Profit of 150 * 2 = 300
                        'You have closed Position & Exit Trading System
                    End If
                    If ltp <= BuySlPrice Then ' IfStoploss Hits.Place Sell Order
                        IsSold = True
                        ExitTradingSystem = True
                        Kite.PlaceRegularOrder("NSE", "ICICIBANK", "SELL", "MARKET", 150, "MIS") 'Buy Qty 150 at Market
                        'Order Placed successfully
                        'You Booked loss of 150 * 2 = -300
                        'You have closed Position & Exit Trading System
                    End If
                End If
            Loop While ExitTradingSystem = False

            'Now we will Logout and close
            'Remove the Handler
            RemoveHandler Kite.QuotesReceivedEvent, AddressOf Quotes_Received

            'Logout from Kite
            Kite.Logout()

            Kite = Nothing 'This will Dispose the Kite Instance and releases all resources held by it.

            'Note:-
            'If your are building UI clients, then donot use Do...Loop in main thread
            'this will make UI unresponsive instead use background threads
            'Also some Functions like GetOrderBook etc will take time as it receives long data
            'from Kite.Trade. So call all those functions in background thread and Invoke when finished

        Catch ex As Exception
            MsgBox(ex.Message) 'Display any Exceptions raised
        End Try
    End Sub

    Private Sub Quotes_Received(sender As Object, e As KiteDotNet.QuotesReceivedEventArgs)
        Try
            'Here we will Receive Quotes
            'We will get all Price Fields and Market Depth

            'For demo we are going to use only Ltp
            'store the ltp value of ICICIBANK in the Dictionary
            If e.TrdSym = "ICICIBANK" Then 'Only Do With ICICIBANK Quotes rest ignore
                If DictLtp.TryGetValue("ICICIBANK", 0) Then 'Check whether Key Exists or Not
                    DictLtp.Item("ICICIBANK") = e.Ltp
                End If
            End If

            'You can do anything with the data
            'For example, below code will write all quotes to text file in the app dir
            Dim ddtt As String = DateTime.Now.ToString("dd-MMM-yy HH:mm:ss")
            Dim WriteStr As String = ddtt & "," & e.Exch & "," & e.TrdSym & "," & e.Ltq & "," & e.Ltq & "," & e.Open & "," & _
                e.High & "," & e.Low & "," & e.Close & "," & e.Volume & "," & e.TotalBuyQty & "," & e.TotalSellQty
            Using sw As New StreamWriter("Quotes.txt", True)
                sw.Write(WriteStr)
            End Using
            'the above will write all quotes received in Quotes.txt file
            'Like this
            '23-Oct-16 09:30:35,NSE,ICICIBANK,215.55,12,214.10,215.75,214.05,214.5,5689756,25689,56899

        Catch ex As Exception
            MsgBox(ex.Message) 'Display any Exceptions raised
        End Try
    End Sub
End Class

```
