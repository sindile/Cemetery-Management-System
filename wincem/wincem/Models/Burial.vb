Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Spatial
Imports System.Net
Imports System.Runtime.Serialization
Imports System.Net.Mail

Public Class Burial
    Public Property ID() As Integer
    <Display(Name:="Space ID")> _
    Public Property Space_ID() As String
    <Display(Name:="Burial Position")> _
    Public Property Burial_Position As String
    <Display(Name:="Ordered By")> _
    Public Property Ordered_BY() As String
    <Display(Name:="Address")> _
    Public Property Ordered_By_Address() As String
    <Display(Name:="Phone Number")> _
    Public Property Ordered_By_Phone() As String
    <Display(Name:="Cremation ?")> _
    Public Property Cremation() As String
    <Display(Name:="Garden ?")> _
    Public Property Garden() As String
    <Display(Name:="Container")> _
    Public Property Container() As String
    <Display(Name:="First Name")> _
    Public Property First_Name() As String
    <Display(Name:="Last Name")> _
    Public Property Last_Name() As String
    <Display(Name:="Gender")> _
    Public Property Gender() As String
    <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}", ApplyFormatInEditMode:=True)> _
    <Display(Name:="Date of Death")> _
    Public Property Date_of_Death() As DateTime?
    <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}", ApplyFormatInEditMode:=True)> _
    <Display(Name:="Date of Birth")> _
    Public Property Date_of_Birth() As DateTime?
    <Display(Name:="Permit Number")> _
    Public Property Permit_Number() As String
    <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}", ApplyFormatInEditMode:=True)> _
    <Display(Name:="Work Order Date")> _
    Public Property Work_Order_Date() As DateTime?
    <Display(Name:="Remarks")> _
    Public Property Remarks() As String
    <Display(Name:="Photo")> _
    Public Property Photo() As String
    <Display(Name:="Birth City, State")> _
    Public Property Birth_City_State() As String
    <Display(Name:="Birth County")> _
    Public Property Birth_County() As String
    <Display(Name:="Death City, State")> _
    Public Property Death_City_State() As String
    <Display(Name:="Death County")> _
    Public Property Death_County() As String
    <Display(Name:="Other Information")> _
    Public Property Other_Information() As String
    <Display(Name:="Military Service")> _
    Public Property Military_Service() As String
    <Display(Name:="Deed Number")> _
    Public Property LF_Burial_Record() As String
    <Display(Name:="Burial Record Type")> _
    Public Property Burial_Record_Type() As String
    <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}", ApplyFormatInEditMode:=True)> _
    <Display(Name:="Burial Date")> _
    Public Property Burial_Date() As DateTime?
    <DisplayFormat(DataFormatString:="{0:HH:mm:ss}", ApplyFormatInEditMode:=True)> _
    <DataType(DataType.Time)> _
    <Display(Name:="Burial Time")> _
    Public Property Burial_Time() As DateTime?
    <Display(Name:="Type of Service")> _
    Public Property Type_of_Service() As String
    <Display(Name:="Receipt Number")> _
    Public Property Receipt_Number() As String
    <Display(Name:="Fees (001-4462)")> _
    Public Property Fees() As Double?
    <Display(Name:="Pre-Need Payment (135-47464)")> _
    Public Property Pre_Need_Payment() As Double?
    <Display(Name:="Space Purchase (001-4461 (2/3))")> _
    Public Property Space_Purchase_1() As Double?
    <Display(Name:="(135-4461 (1/3))")> _
    Public Property Space_Purchase_2() As Double?
    <Display(Name:="Amount Paid")> _
    Public Property Amount_Paid() As Double?
    <Display(Name:="Field Verified")> _
    Public Property Field_Verified() As String
    <Display(Name:="Headstone Type")> _
    Public Property Headstone_Type() As String
    <Display(Name:="Footstone Type")> _
    Public Property Footstone_Type() As String



    Public Property OwnerID() As Integer
    Public Overridable Property Owner As Owner

    Public Overridable Property Relatives As List(Of Relative)

    Public Sub New()
        Me.Cremation = 0
        Me.Garden = 0
        Me.Gender = "Male"
        Me.Burial_Position = "No_Burial"
    End Sub

    Public Function total() As Double
        Dim fee_Total As Double = 0.0

        fee_Total = If(Me.Fees.HasValue, Me.Fees, 0.0) + If(Me.Pre_Need_Payment.HasValue, Me.Pre_Need_Payment, 0.0) + If(Me.Space_Purchase_1.HasValue, Me.Space_Purchase_1, 0.0) + If(Me.Space_Purchase_2.HasValue, Me.Space_Purchase_2, 0.0)

        Return fee_Total
    End Function

    Public Function age() As Integer
        If Date_of_Birth.HasValue And Date_of_Death.HasValue Then
            Return Math.Floor(DateDiff(DateInterval.Day, Date_of_Birth.Value, Date_of_Death.Value) / 365.25)
        Else
            Return 0
        End If
    End Function

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

    Public Function gis_space() As Feature
        Dim request As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=SPACE_ID='" + Me.Space_ID + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
        Dim ws As WebResponse = request.GetResponse()
        Dim jsonSerializer As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
        Dim spaces As GIS_Spaces = jsonSerializer.ReadObject(ws.GetResponseStream())
        Dim space As Feature = spaces.features(0)
        Return space
    End Function
    Public Function gis_burial() As GIS_Burials.Feature
        Dim request As WebRequest = WebRequest.Create(My.Settings.burials + "/query?where=Burial_ID='" + Me.ID.ToString + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
        Dim ws As WebResponse = request.GetResponse()
        Dim jsonSerializer As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Burials.GIS_Burials))
        Dim gburials As GIS_Burials.GIS_Burials = jsonSerializer.ReadObject(ws.GetResponseStream())
        Dim gburial As New GIS_Burials.Feature
        If gburials.features.Count > 0 Then
            gburial = gburials.features(0)
        End If
        Return gburial
    End Function

    Public Sub Email(ByVal emailAddress As String, ByVal owner As Owner)
        'Try
        Dim toEmailAddress = New MailAddress(emailAddress)

        Dim email As MailMessage

        email = New MailMessage(New MailAddress(My.Settings.SMTPUserName), toEmailAddress)

        email.IsBodyHtml = True
        email.Body = "Name: " + Me.First_Name + " " + Me.Last_Name + _
            "<br/>Burial Record Type: " + Me.Burial_Record_Type + _
            "<br/>Ordered by:  " + Me.Ordered_BY + _
            "<br/>Address/Phone: " + Me.Ordered_By_Address + " / " + Me.Ordered_By_Phone + _
            "<br/>Owner: " + owner.Owner_Name + _
            "<br/>Date of Burial: " + If(Me.Burial_Date.HasValue, Me.Burial_Date.Value.ToLongDateString, "00/00/0000") + _
            "<br/><a href='" + My.Settings.baseURL + "Burial/burialRecord/" + Me.ID.ToString + "'>Open Burial Permit PDF</a>" + _
            "<br/><a href='" + My.Settings.baseURL + "Burial/Details/" + Me.ID.ToString + "'>Open Burial Permit Details</a>"
        email.Subject = "New Burial Permit"
        Dim smtp As New SmtpClient(My.Settings.SMTPClient)
        smtp.UseDefaultCredentials = False
        smtp.Credentials = New System.Net.NetworkCredential(My.Settings.SMTPUserName, My.Settings.SMTPPassword)
        smtp.Send(email)
        'Catch ex As Exception

        'End Try

    End Sub

End Class
