Imports System.Net
Imports System.Net.Sockets
Public Class steamquery

    Public Function arraytostring(ByRef array() As Byte) As String
        Dim Encoding As System.Text.Encoding = System.Text.Encoding.Default
        Return Encoding.GetString(array)
    End Function

    Public Function Query(IP, port)
        Dim A2S_INFO As String = "ÿÿÿÿTSource Engine Query" & Chr(0)
        Dim endPoint As New IPEndPoint(IPAddress.Parse(IP), port)
        Dim client As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        Dim resp

        client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000)
        client.SendTo(System.Text.Encoding.[Default].GetBytes(A2S_INFO), endPoint)

        Dim Buffer(1400) As Byte

        Try
            client.ReceiveFrom(Buffer, endPoint)

            resp = arraytostring(Buffer).Remove(0, 6)
            resp = (resp.Replace(Chr(&H0), "\x00")).Replace("\x00\x00", "")
        Catch ex As SocketException
            Console.WriteLine("Server Down")
            resp = False
        End Try

        Return resp
    End Function

    Private Sub steamquery_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim Text = Query("0.0.0.0", "27015")

        Dim split As String() = Text.Split(New Char() {"\x00"})

        Dim s As String
        For Each s In split
            Console.WriteLine(s.Replace("x00", ""))
        Next
    End Sub

End Class
