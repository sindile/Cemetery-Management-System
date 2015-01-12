Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports System.Net
Imports System.Runtime.Serialization
Imports System.IO

Public Class Owned_Space
    Public Property ID() As Integer
    Public Property Space_ID As String

    Public Property OwnerID As Integer
    Public Overridable Property Owner As Owner

    Public Overridable Property Assignments As List(Of Assignment)

    Public Sub New()
        Me.Space_ID = "0-00-0-000-000"
    End Sub

    Public Function cemetery() As String
        Dim cem As String = ""
        If Me.Space_ID.Substring(0, 1) = 1 Then
            cem = "Highland"
        ElseIf Me.Space_ID.Substring(0, 1) = 2 Then
            cem = "Union"
        ElseIf Me.Space_ID.Substring(0, 1) = 3 Then
            cem = "Graham"
        ElseIf Me.Space_ID.Substring(0, 1) = 4 Then
            cem = "St Mary's"
        End If
        Return cem
    End Function

    Public Function division() As String
        Dim div As String = ""
        If Me.Space_ID.Substring(2, 2) = "00" Then
            div = "Unknown/No Division"
        ElseIf Me.Space_ID.Substring(2, 2) = "21" Then
            div = "West"
        ElseIf Me.Space_ID.Substring(2, 2) = "22" Then
            div = "Winfield View"
        ElseIf Me.Space_ID.Substring(2, 2) = "23" Then
            div = "Diamond"
        ElseIf Me.Space_ID.Substring(2, 2) = "24" Then
            div = "Grand"
        ElseIf Me.Space_ID.Substring(2, 2) = "25" Then
            div = "South"
        ElseIf Me.Space_ID.Substring(2, 2) = "26" Then
            div = "North Central"
        ElseIf Me.Space_ID.Substring(2, 2) = "27" Then
            div = "South Central"
        ElseIf Me.Space_ID.Substring(2, 2) = "28" Then
            div = "Public"
        ElseIf Me.Space_ID.Substring(2, 2) = "29" Then
            div = "Union Replat"
        ElseIf Me.Space_ID.Substring(2, 2) = "30" Then
            div = "North"
        ElseIf Me.Space_ID.Substring(2, 2) = "31" Then
            div = "Mausoleum"
        ElseIf Me.Space_ID.Substring(2, 2) = "32" Then
            div = "Highland New"
        ElseIf Me.Space_ID.Substring(2, 2) = "0A" Then
            div = "A"
        ElseIf Me.Space_ID.Substring(2, 2) = "0B" Then
            div = "B"
        ElseIf Me.Space_ID.Substring(2, 2) = "0C" Then
            div = "C"
        ElseIf Me.Space_ID.Substring(2, 2) = "0D" Then
            div = "D"
        End If
        Return div
    End Function

    Public Sub updateSpaceAvailability(ByVal availability As String)
        Dim request As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=SPACE_ID='" + Me.Space_ID + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
        Dim ws As WebResponse = request.GetResponse()
        Dim jsonSerializer As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
        Dim spaces As GIS_Spaces = jsonSerializer.ReadObject(ws.GetResponseStream())
        Dim space As Feature = spaces.features(0)
        space.attributes.Available = availability
        Dim stream1 = New MemoryStream
        Dim ser As New Json.DataContractJsonSerializer(GetType(Feature))
        ser.WriteObject(stream1, space)
        stream1.Position = 0
        Dim sr As StreamReader = New StreamReader(stream1)
        WebRequestinWebForm(My.Settings.SpacesFeatureService + "updateFeatures", "features=[" + sr.ReadToEnd() + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
    End Sub

    Private Function WebRequestinWebForm(url As String, postData As String) As String
        Dim ret As String = String.Empty
        Dim encoding As ASCIIEncoding = New ASCIIEncoding()
        Dim requestWriter As Stream
        Dim byteArray As Byte() = encoding.GetBytes(postData)

        Dim webRequest = TryCast(System.Net.WebRequest.Create(url), HttpWebRequest)
        If webRequest IsNot Nothing Then
            webRequest.Method = "POST"
            webRequest.ContentType = "application/x-www-form-urlencoded"
            webRequest.ContentLength = byteArray.Length


            'POST the data.
            requestWriter = webRequest.GetRequestStream()
            requestWriter.Write(byteArray, 0, byteArray.Length)
        End If

        Dim resp As HttpWebResponse = DirectCast(webRequest.GetResponse(), HttpWebResponse)
        Dim resStream As Stream = resp.GetResponseStream()
        Dim reader As New StreamReader(resStream)
        ret = reader.ReadToEnd()

        Return ret
    End Function
End Class
