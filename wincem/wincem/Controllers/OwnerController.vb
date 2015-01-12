Option Infer On

Imports System.Data.Entity
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports System.IO
Imports PdfSharp.Drawing.Layout
Imports System.Net
Imports System.Runtime.Serialization
Imports System.Linq.Expressions
Imports LinqKit



Namespace wincem
    Public Class OwnerController
        Inherits System.Web.Mvc.Controller

        Private db As New cemeteryContext

        '
        ' GET: /Owner/

        Function Index(Optional searchString As String = "", Optional number_of_records As Integer = 50) As ActionResult
            Dim deeds = From d In db.Owners
                        Select d
            If Not String.IsNullOrEmpty(searchString) Then
                deeds = deeds.Where(Function(s) s.Owner_Name.Contains(searchString) _
                                        Or s.Deed_Name.Contains(searchString) _
                                        Or s.Deed_No.Contains(searchString) _
                                        )
            End If
            Return View(deeds.ToList().Take(number_of_records))
        End Function

        Function deedTotals(Optional startDate As Date? = Nothing, Optional endDate As Date? = Nothing) As ActionResult
            Dim d As Date = Date.Now()
            Dim ds As String = "1/1/" + d.Year.ToString
            If startDate Is Nothing Then
                ds = "1/1/" + Now.Year.ToString
                ViewBag.startDate = "1/1/" + Now.Year.ToString
            Else
                ds = startDate
                ViewBag.startDate = startDate.Value.ToShortDateString
            End If

            If endDate Is Nothing Then
                d = Date.Now()
                ViewBag.endDate = Date.Now()
            Else
                d = endDate
                ViewBag.endDate = endDate.Value.ToShortDateString
            End If
            Dim highland As Integer = 0
            Dim union As Integer = 0
            Dim graham As Integer = 0
            Dim st_marys As Integer = 0

            Dim deeds = db.Owners.Where(Function(w) w.Date_of_Purchase > ds And w.Date_of_Purchase < d)

            For Each item In deeds.ToList
                If item.cemeteryList.Count > 0 Then
                    If item.cemeteryList.Item(0) = "Highland" Then
                        highland += 1
                    ElseIf item.cemeteryList.Item(0) = "Union" Then
                        union += 1
                    ElseIf item.cemeteryList.Item(0) = "Graham" Then
                        graham += 1
                    ElseIf item.cemeteryList.Item(0) = "St Mary's" Then
                        st_marys += 1
                    End If
                End If
            Next

            ViewBag.Highland = highland
            ViewBag.Union = union
            ViewBag.Graham = graham
            ViewBag.St_Marys = st_marys
            ViewBag.Total = ViewBag.Highland + ViewBag.Union + ViewBag.Graham + ViewBag.St_Marys

            Return PartialView()
        End Function

        '
        ' GET: /Permits/SearchDeeds
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function SearchDeeds(Optional searchString As String = "", Optional number_of_records As Integer = 50, Optional deed_number As Boolean = True, _
                            Optional Owner_Name As Boolean = True, Optional Deed_Name As Boolean = True, Optional Date_of_Purchase As Boolean = True) As PartialViewResult

            Dim pred = PredicateBuilder.True(Of Owner)()

            Dim d As Date

            If Not String.IsNullOrEmpty(searchString) Then
                pred = PredicateBuilder.False(Of Owner)()
                If deed_number Then
                    pred = pred.Or(Function(s As Owner) s.Deed_No.Contains(searchString))
                End If
                If Owner_Name Then
                    pred = pred.Or(Function(s As Owner) s.Owner_Name.Contains(searchString))
                End If
                If Deed_Name Then
                    pred = pred.Or(Function(s As Owner) s.Deed_Name.Contains(searchString))
                End If
                If Date_of_Purchase And Date.TryParse(searchString, d) Then
                    Dim dateString As String = d.ToShortDateString
                    pred = pred.Or(Function(s As Owner) s.Date_of_Purchase = dateString)
                End If
            End If
            Return PartialView(db.Owners.AsExpandable().Where(pred).Take(number_of_records).ToList)
        End Function

        '
        ' GET: /Permits/SearchDeedsMap
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function SearchDeedsMap(Optional searchString As String = "", Optional number_of_records As Integer = 50, Optional deed_number As Boolean = True, _
                            Optional Owner_Name As Boolean = True, Optional Deed_Name As Boolean = True, Optional Date_of_Purchase As Boolean = True) As PartialViewResult

            Dim pred = PredicateBuilder.True(Of Owner)()

            Dim d As Date

            If Not String.IsNullOrEmpty(searchString) Then
                pred = PredicateBuilder.False(Of Owner)()
                If deed_number Then
                    pred = pred.Or(Function(s As Owner) s.Deed_No.Contains(searchString))
                End If
                If Owner_Name Then
                    pred = pred.Or(Function(s As Owner) s.Owner_Name.Contains(searchString))
                End If
                If Deed_Name Then
                    pred = pred.Or(Function(s As Owner) s.Deed_Name.Contains(searchString))
                End If
                If Date_of_Purchase And Date.TryParse(searchString, d) Then
                    Dim dateString As String = d.ToShortDateString
                    pred = pred.Or(Function(s As Owner) s.Date_of_Purchase = dateString)
                End If
            End If
            Return PartialView(db.Owners.AsExpandable().Where(pred).Take(number_of_records).ToList)
        End Function

        Function DetailsMap(ByVal Space_ID As String) As ActionResult
            Dim spacequery = db.Owned_Spaces.Where(Function(s) s.Space_ID.Equals(Space_ID))
            Dim owner As Owner
            If spacequery.Count > 0 Then
                owner = db.Owners.Find(spacequery.First().OwnerID)
                ViewBag.space_id = Space_ID
                ViewBag.owned_space_id = spacequery.First().ID
                Return PartialView(owner)
            Else
                ViewBag.space_id = Space_ID
                Return PartialView()
            End If
        End Function
        '
        ' GET: /Owner/Details/5

        Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owner As Owner = db.Owners.Find(id)
            If IsNothing(owner) Then
                Return HttpNotFound()
            End If
            Dim spaceList As String = ""
            For Each s In owner.Spaces
                spaceList = spaceList + "'" + s.Space_ID + "',"
            Next
            spaceList = spaceList.TrimEnd(",")
            ViewBag.SpaceList = spaceList
            Return View(owner)
        End Function

        Function deedReport(Optional cemetery As String = "ALL") As ActionResult
            Dim stream As New MemoryStream

            ' Create a new PDF document
            Dim document As PdfDocument = New PdfDocument
            document.Info.Title = "Deed Report"

            ' Create an empty page
            'Dim page As PdfPage = document.AddPage

            ' Get an XGraphics object for drawing
            Dim gfx As XGraphics
            Dim tf As XTextFormatter

            ' Create a font
            Dim font As XFont = New XFont("Arial", 8, XFontStyle.Regular)
            Dim TitleFont As XFont = New XFont("Times New Roman", 11, XFontStyle.BoldItalic)
            Dim TitleFontBold As XFont = New XFont("Times New Roman", 20, XFontStyle.BoldItalic)
            Dim pen As XPen = New XPen(XColor.FromArgb(0, 0, 0))

            Dim col1 As Double = 8 * 7.2
            Dim col2 As Double = 35 * 7.2
            Dim col3 As Double = 39 * 7.2
            Dim col4 As Double = 49.5 * 7.2
            Dim col5 As Double = 57 * 7.2
            Dim col6 As Double = 66 * 7.2
            Dim col7 As Double = 73 * 7.2
            Dim col8 As Double = 77.5 * 7.2

            Dim deedRecords As List(Of Owner)
            If cemetery = 0 Then
                deedRecords = db.Owners.Where(Function(w) Not w.Owner_Name = "").OrderBy(Function(o) o.Deed_No).OrderBy(Function(o) o.Owner_Name).ToList
            Else
                deedRecords = db.Owners.Where(Function(w) Not w.Owner_Name = "").OrderBy(Function(o) o.Deed_No).OrderBy(Function(o) o.Owner_Name).ToList
            End If
            Dim records_per_page As Integer = 46
            Dim totalPages As Integer = Math.Ceiling(deedRecords.Count / (records_per_page))
            Dim x As Integer = 0
            Dim y1 As Double = 10 * 7.2
            Dim y2 As Double = 11.25 * 7.2
            Dim verticleSpacing As Double = 2.0 * 7.2
            Dim firstVertileSpacing As Double = 2.25 * 7.2
            Dim currentPage As Integer = 0
            For Each d In deedRecords
                Dim page As PdfPage
                If x = 0 Or (x Mod records_per_page = 0) Then
                    y1 = 10 * 7.2
                    y2 = 11.25 * 7.2
                    page = document.AddPage
                    gfx = XGraphics.FromPdfPage(page)
                    tf = New XTextFormatter(gfx)
                    tf.Alignment = XParagraphAlignment.Left
                    tf.DrawString("Owner", TitleFont, XBrushes.Maroon, _
                                    New XRect(col1, y1, col2 - col1, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Deed#", TitleFont, XBrushes.Maroon, _
                                    New XRect(col2, y1, col3 - col2, y2 - y1), XStringFormats.TopLeft)
                    tf.Alignment = XParagraphAlignment.Center
                    tf.DrawString("Purch. Date", TitleFont, XBrushes.Maroon, _
                                    New XRect(col3, y1, col4 - col3, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Division", TitleFont, XBrushes.Maroon, _
                                    New XRect(col4, y1, col5 - col4, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Block/Section", TitleFont, XBrushes.Maroon, _
                                    New XRect(col5, y1, col6 - col5, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Lot/Row", TitleFont, XBrushes.Maroon, _
                                    New XRect(col6, y1, col7 - col6, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Space", TitleFont, XBrushes.Maroon, _
                                    New XRect(col7, y1, col8 - col7, y2 - y1), XStringFormats.TopLeft)
                    tf.Alignment = XParagraphAlignment.Left
                    gfx.DrawLine(New XPen(XColors.Maroon, 2), New XPoint(col1, y2 + 5), New XPoint(col8, y2 + 5))
                    gfx.DrawLine(New XPen(XColors.Gray, 2), New XPoint(col1, page.Height.Point - 5.75 * 7.2), New XPoint(col8, page.Height.Point - 5.75 * 7.2))
                    tf.DrawString(Date.Now().ToLongDateString, TitleFont, XBrushes.Maroon, _
                                   New XRect(col1, page.Height.Point - 5.5 * 7.2, col8 - col1, y2 - y1), XStringFormats.TopLeft)
                    currentPage += 1
                    tf.Alignment = XParagraphAlignment.Right
                    tf.DrawString("Page " + currentPage.ToString + " of " + totalPages.ToString, TitleFont, XBrushes.Maroon, _
                                   New XRect(col1, page.Height.Point - 5.5 * 7.2, col8 - col1, y2 - y1), XStringFormats.TopLeft)
                    tf.Alignment = XParagraphAlignment.Center
                    If cemetery = 0 Then
                        tf.DrawString("Complete Cemetery Deed Report", TitleFont, XBrushes.Maroon, _
                                   New XRect(col1, page.Height.Point - 5.5 * 7.2, col8 - col1, y2 - y1), XStringFormats.TopLeft)
                    Else
                        tf.DrawString(d.cemeteryList.Item(0) + " Cemetery Deed Report", TitleFont, XBrushes.Maroon, _
                                       New XRect(col1, page.Height.Point - 5.5 * 7.2, col8 - col1, y2 - y1), XStringFormats.TopLeft)
                    End If
                    tf.Alignment = XParagraphAlignment.Left
                    If currentPage = 1 Then
                        tf.Alignment = XParagraphAlignment.Left
                        If cemetery = 0 Then
                            tf.DrawString("Complete Cemetery Deed Report", TitleFontBold, XBrushes.Maroon, _
                                       New XRect(col1, 6 * 7.2, col8 - col1, 3 * 7.2), XStringFormats.TopLeft)
                        Else
                            tf.DrawString(d.cemeteryList.Item(0) + " Cemetery Burial Report", TitleFontBold, XBrushes.Maroon, _
                                           New XRect(col1, 6 * 7.2, col8 - col1, 3 * 7.2), XStringFormats.TopLeft)
                        End If
                        'records_per_page = 46
                        'x += 2
                    End If

                    y1 += firstVertileSpacing
                    y2 += firstVertileSpacing
                End If
                Try
                    If cemetery = 0 Then
                        tf.Alignment = XParagraphAlignment.Left
                        tf.DrawString(d.Owner_Name + " - " + d.cemeteryList.Item(0), font, XBrushes.Black, _
                                                            New XRect(col1, y1, col2 - col1, y2 - y1), XStringFormats.TopLeft)
                        tf.Alignment = XParagraphAlignment.Center
                        tf.DrawString(d.Deed_No, font, XBrushes.Black, _
                                        New XRect(col2, y1, col3 - col2, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(If(d.Date_of_Purchase.HasValue, d.Date_of_Purchase.Value.ToShortDateString, ""), font, XBrushes.Black, _
                                        New XRect(col3, y1, col4 - col3, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.divisionList.Item(0), font, XBrushes.Black, _
                                        New XRect(col4, y1, col5 - col4, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.blockList.Item(0), font, XBrushes.Black, _
                                        New XRect(col5, y1, col6 - col5, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.lotRowList.Item(0), font, XBrushes.Black, _
                                        New XRect(col6, y1, col7 - col6, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.spaceList.Item(0), font, XBrushes.Black, _
                                        New XRect(col7, y1, col8 - col7, y2 - y1), XStringFormats.TopLeft)
                    Else
                        tf.Alignment = XParagraphAlignment.Left
                        tf.DrawString(d.Owner_Name + " - " + d.cemeteryList.Item(0), font, XBrushes.Black, _
                                                            New XRect(col1, y1, col2 - col1, y2 - y1), XStringFormats.TopLeft)
                        tf.Alignment = XParagraphAlignment.Center
                        tf.DrawString(d.Deed_No, font, XBrushes.Black, _
                                        New XRect(col2, y1, col3 - col2, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(If(d.Date_of_Purchase.HasValue, d.Date_of_Purchase.Value.ToShortDateString, ""), font, XBrushes.Black, _
                                        New XRect(col3, y1, col4 - col3, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.divisionList.Item(0), font, XBrushes.Black, _
                                        New XRect(col4, y1, col5 - col4, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.blockList.Item(0), font, XBrushes.Black, _
                                        New XRect(col5, y1, col6 - col5, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.lotRowList.Item(0), font, XBrushes.Black, _
                                        New XRect(col6, y1, col7 - col6, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(d.spaceList.Item(0), font, XBrushes.Black, _
                                        New XRect(col7, y1, col8 - col7, y2 - y1), XStringFormats.TopLeft)
                    End If
                    tf.Alignment = XParagraphAlignment.Left
                Catch ex As Exception

                End Try


                y1 += verticleSpacing
                y2 += verticleSpacing

                x += 1
            Next

            document.Save(stream, False)
            'stream.Flush()
            'stream.Position = 0
            Return File(stream, "application/pdf")
        End Function

        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function DeedCard(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owner As Owner = db.Owners.Find(id)
            If IsNothing(owner) Then
                Return HttpNotFound()
            End If
            Dim stream As New MemoryStream

            ' Create a new PDF document
            Dim document As PdfDocument = New PdfDocument
            document.Info.Title = "Certificate of Ownership"

            ' Create an empty page
            Dim page1 As PdfPage = document.AddPage
            page1.Width = New XUnit(6, XGraphicsUnit.Inch)
            page1.Height = New XUnit(4.0, XGraphicsUnit.Inch)

            ' Get an XGraphics object for drawing
            Dim gfx1 As XGraphics = XGraphics.FromPdfPage(page1)
            Dim tf1 As New XTextFormatter(gfx1)

            ' Create an empty page
            Dim page2 As PdfPage = document.AddPage
            page2.Width = New XUnit(6.0, XGraphicsUnit.Inch)
            page2.Height = New XUnit(4.0, XGraphicsUnit.Inch)

            ' Get an XGraphics object for drawing
            Dim gfx2 As XGraphics = XGraphics.FromPdfPage(page2)
            Dim tf2 As New XTextFormatter(gfx2)

            Dim font As XFont = New XFont("Arial", 10, XFontStyle.Regular)
            Dim TitleFontBold As XFont = New XFont("Arial", 12, XFontStyle.Bold)
            Dim pen As XPen = New XPen(XColor.FromArgb(0, 0, 0))
            Dim graypen As XPen = New XPen(XColor.FromArgb(128, 128, 128))

            ' Draw the text
            Dim x1 As Double = 4 * 7.2
            Dim y1 As Double = 4 * 7.2
            Dim y2 As Double = 5.25 * 7.2
            Dim x2 As Double = page1.Width.Point - 4 * 7.2
            Dim verticleSpace As Double = 2.5 * 7.2

            tf1.Alignment = XParagraphAlignment.Left
            tf1.DrawString("Amount Paid : " + Format(owner.Amount_Paid, "$#,##0.00"), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf1.Alignment = XParagraphAlignment.Right
            tf1.DrawString("Deed # : " + owner.Deed_No, font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            tf1.Alignment = XParagraphAlignment.Left
            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf1.DrawString("Owner : " + If(String.IsNullOrEmpty(owner.Deed_Name), owner.Owner_Name, owner.Deed_Name), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf1.DrawString("Address : " + owner.Owner_Address, font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf1.DrawString("Phone Number : " + owner.Phone_Number, font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf1.Alignment = XParagraphAlignment.Right
            tf1.DrawString("Date : " + If(owner.Date_of_Purchase.HasValue, owner.Date_of_Purchase.Value.ToShortDateString, "00/00/0000"), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf1.Alignment = XParagraphAlignment.Center
            tf1.DrawString("PURCHASES", TitleFontBold, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            tf1.DrawString("Cemetery", font, XBrushes.Black, New XRect(4 * 7.2, 16 * 7.2, 8 * 7.2, y2 - y1))
            tf1.DrawString("Division", font, XBrushes.Black, New XRect(12 * 7.2, 16 * 7.2, 14 * 7.2, y2 - y1))
            tf1.DrawString("Block", font, XBrushes.Black, New XRect(26 * 7.2, 16 * 7.2, 7 * 7.2, y2 - y1))
            tf1.DrawString("Lot/Row", font, XBrushes.Black, New XRect(33 * 7.2, 16 * 7.2, 6.5 * 7.2, y2 - y1))
            tf1.DrawString("Space No.", font, XBrushes.Black, New XRect(39.5 * 7.2, 16 * 7.2, x2 - 39.5 * 7.2, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf1.DrawString(owner.cemeteryList.Item(0), font, XBrushes.Black, New XRect(4 * 7.2, 18.5 * 7.2, 8 * 7.2, y2 - y1))
            tf1.DrawString(owner.divisionList.Item(0), font, XBrushes.Black, New XRect(12 * 7.2, 18.5 * 7.2, 14 * 7.2, y2 - y1))
            tf1.DrawString(owner.blockList.Item(0), font, XBrushes.Black, New XRect(26 * 7.2, 18.5 * 7.2, 7 * 7.2, y2 - y1))
            tf1.DrawString(owner.lotRowList.Item(0), font, XBrushes.Black, New XRect(33 * 7.2, 18.5 * 7.2, 6.5 * 7.2, y2 - y1))
            tf1.DrawString(owner.spaceList.Item(0), font, XBrushes.Black, New XRect(39.5 * 7.2, 18.5 * 7.2, x2 - 39.5 * 7.2, 2 * (y2 - y1)))

            gfx1.DrawLine(pen, New XPoint(x1, 18 * 7.2), New XPoint(x2, 18 * 7.2))
            gfx1.DrawLine(pen, New XPoint(x1, 20.5 * 7.2), New XPoint(x2, 20.5 * 7.2))

            gfx1.DrawLine(pen, New XPoint(4 * 7.2, 18 * 7.2), New XPoint(4 * 7.2, 20.5 * 7.2))
            gfx1.DrawLine(pen, New XPoint(12 * 7.2, 18 * 7.2), New XPoint(12 * 7.2, 20.5 * 7.2))
            gfx1.DrawLine(pen, New XPoint(26 * 7.2, 18 * 7.2), New XPoint(26 * 7.2, 20.5 * 7.2))
            gfx1.DrawLine(pen, New XPoint(33 * 7.2, 18 * 7.2), New XPoint(33 * 7.2, 20.5 * 7.2))
            gfx1.DrawLine(pen, New XPoint(39.5 * 7.2, 18 * 7.2), New XPoint(39.5 * 7.2, 20.5 * 7.2))
            gfx1.DrawLine(pen, New XPoint(x2, 18 * 7.2), New XPoint(x2, 20.5 * 7.2))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf1.Alignment = XParagraphAlignment.Center
            tf1.DrawString("Assignments", TitleFontBold, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace

            tf1.DrawString("Assn No", font, XBrushes.Black, New XRect(4 * 7.2, y1, 7 * 7.2, y2 - y1))
            tf1.DrawString("Cemetery", font, XBrushes.Black, New XRect(11 * 7.2, y1, 7 * 7.2, y2 - y1))
            tf1.DrawString("Division", font, XBrushes.Black, New XRect(18 * 7.2, y1, 14 * 7.2, y2 - y1))
            tf1.DrawString("Block", font, XBrushes.Black, New XRect(32 * 7.2, y1, 7 * 7.2, y2 - y1))
            tf1.DrawString("Lot/Row", font, XBrushes.Black, New XRect(39 * 7.2, y1, 6.5 * 7.2, y2 - y1))
            tf1.DrawString("Space No.", font, XBrushes.Black, New XRect(45.5 * 7.2, y1, x2 - 45.5 * 7.2, y2 - y1))
            gfx1.DrawLine(pen, New XPoint(x1, y2 + verticleSpace / 4), New XPoint(x2, y2 + verticleSpace / 4))
            Dim assignments = db.Assignments.Where(Function(w) w.Original_OwnerID = owner.ID).ToList
            Dim assignment As String = ""
            For Each a In assignments
                If assignment.IndexOf(a.New_Owner.Deed_No) = -1 Then
                    assignment = assignment + a.New_Owner.Deed_No + ","
                    y1 = y1 + verticleSpace
                    y2 = y2 + verticleSpace

                    tf1.DrawString(a.New_Owner.Deed_No, font, XBrushes.Black, New XRect(4 * 7.2, y1, 7 * 7.2, y2 - y1))
                    tf1.DrawString(a.New_Owner.cemeteryList.Item(0), font, XBrushes.Black, New XRect(11 * 7.2, y1, 7 * 7.2, y2 - y1))
                    tf1.DrawString(a.New_Owner.divisionList.Item(0), font, XBrushes.Black, New XRect(18 * 7.2, y1, 14 * 7.2, y2 - y1))
                    tf1.DrawString(a.New_Owner.blockList.Item(0), font, XBrushes.Black, New XRect(32 * 7.2, y1, 7 * 7.2, y2 - y1))
                    tf1.DrawString(a.New_Owner.lotRowList.Item(0), font, XBrushes.Black, New XRect(39 * 7.2, y1, 6.5 * 7.2, y2 - y1))
                    tf1.DrawString(a.New_Owner.spaceList.Item(0), font, XBrushes.Black, New XRect(45.5 * 7.2, y1, x2 - 45.5 * 7.2, y2 - y1))
                    gfx1.DrawLine(pen, New XPoint(x1, y2 + verticleSpace / 4), New XPoint(x2, y2 + verticleSpace / 4))
                    gfx1.DrawLine(pen, New XPoint(4 * 7.2, y2 + verticleSpace / 4), New XPoint(4 * 7.2, y1 - verticleSpace / 4))
                    gfx1.DrawLine(pen, New XPoint(11 * 7.2, y2 + verticleSpace / 4), New XPoint(11 * 7.2, y1 - verticleSpace / 4))
                    gfx1.DrawLine(pen, New XPoint(18 * 7.2, y2 + verticleSpace / 4), New XPoint(18 * 7.2, y1 - verticleSpace / 4))
                    gfx1.DrawLine(pen, New XPoint(32 * 7.2, y2 + verticleSpace / 4), New XPoint(32 * 7.2, y1 - verticleSpace / 4))
                    gfx1.DrawLine(pen, New XPoint(39 * 7.2, y2 + verticleSpace / 4), New XPoint(39 * 7.2, y1 - verticleSpace / 4))
                    gfx1.DrawLine(pen, New XPoint(45.5 * 7.2, y2 + verticleSpace / 4), New XPoint(45.5 * 7.2, y1 - verticleSpace / 4))
                    gfx1.DrawLine(pen, New XPoint(x2, y2 + verticleSpace / 4), New XPoint(x2, y1 - verticleSpace / 4))
                End If
            Next

            'gfx2.DrawLine(pen, New XPoint(4 * 7.2, 4 * 7.2), New XPoint(page2.Width.Point - (4 * 7.2), 4 * 7.2))
            'gfx2.DrawLine(pen, New XPoint(4 * 7.2, 4 * 7.2), New XPoint(4 * 7.2, page2.Height.Point - (4 * 7.2)))
            'gfx2.DrawLine(pen, New XPoint(4 * 7.2, page2.Height.Point - (4 * 7.2)), New XPoint(page2.Width.Point - (4 * 7.2), page2.Height.Point - (4 * 7.2)))
            'gfx2.DrawLine(pen, New XPoint(page2.Width.Point - (4 * 7.2), 4 * 7.2), New XPoint(page2.Width.Point - (4 * 7.2), page2.Height.Point - (4 * 7.2)))
            'gfx2.DrawLine(pen, New XPoint(page2.Width.Point / 2, 4 * 7.2), New XPoint(page2.Width.Point / 2, page2.Height.Point - (4 * 7.2)))

            'Dim spaceHeight = (page2.Height.Point - 2 * (4 * 7.2)) / 5

            'gfx2.DrawLine(pen, New XPoint(4 * 7.2, 4 * 7.2 + (spaceHeight * 1)), New XPoint(page2.Width.Point - (4 * 7.2), 4 * 7.2 + (spaceHeight * 1)))
            'gfx2.DrawLine(pen, New XPoint(4 * 7.2, 4 * 7.2 + (spaceHeight * 2)), New XPoint(page2.Width.Point - (4 * 7.2), 4 * 7.2 + (spaceHeight * 2)))
            'gfx2.DrawLine(pen, New XPoint(4 * 7.2, 4 * 7.2 + (spaceHeight * 3)), New XPoint(page2.Width.Point - (4 * 7.2), 4 * 7.2 + (spaceHeight * 3)))
            'gfx2.DrawLine(pen, New XPoint(4 * 7.2, 4 * 7.2 + (spaceHeight * 4)), New XPoint(page2.Width.Point - (4 * 7.2), 4 * 7.2 + (spaceHeight * 4)))

            Dim spaces_in As String = ""

            For Each item In owner.Spaces
                spaces_in += "'" + item.Space_ID + "',"
            Next
            spaces_in = "(" + spaces_in.TrimEnd(",") + ")"
            'Dim label As String = ""


            Dim request As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=SPACE_ID+IN+" + spaces_in + "&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
            Dim ws As WebResponse = request.GetResponse()
            Dim jsonSerializer As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
            Dim spaces As GIS_Spaces = jsonSerializer.ReadObject(ws.GetResponseStream())
            Dim bottom_left_x As Double = 0
            Dim bottom_left_y As Double = 0

            Dim top_right_x As Double = 0
            Dim top_right_y As Double = 0

            For Each item In spaces.features
                For Each x In item.geometry.rings(0)
                    If top_right_x = 0 Or x.Item(0) > top_right_x Then
                        top_right_x = x.Item(0)
                    End If
                    If top_right_y = 0 Or x.Item(1) > top_right_y Then
                        top_right_y = x.Item(1)
                    End If
                    If bottom_left_x = 0 Or x.Item(0) < bottom_left_x Then
                        bottom_left_x = x.Item(0)
                    End If
                    If bottom_left_y = 0 Or x.Item(1) < bottom_left_y Then
                        bottom_left_y = x.Item(1)
                    End If
                Next
            Next
            Dim requestenv As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=&text=&objectIds=&time=&geometry=" + bottom_left_x.ToString + "%2C" + bottom_left_y.ToString + "%2C" + top_right_x.ToString + "%2C" + top_right_y.ToString + "&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
            Dim wsenv As WebResponse = requestenv.GetResponse()
            Dim jsonSerializerenv As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
            Dim spacesenv As GIS_Spaces = jsonSerializer.ReadObject(wsenv.GetResponseStream())
            Dim scale As String = ""
            For Each item In spacesenv.features
                For Each x In item.geometry.rings(0)
                    If top_right_x = 0 Or x.Item(0) > top_right_x Then
                        top_right_x = x.Item(0)
                    End If
                    If top_right_y = 0 Or x.Item(1) > top_right_y Then
                        top_right_y = x.Item(1)
                    End If
                    If bottom_left_x = 0 Or x.Item(0) < bottom_left_x Then
                        bottom_left_x = x.Item(0)
                    End If
                    If bottom_left_y = 0 Or x.Item(1) < bottom_left_y Then
                        bottom_left_y = x.Item(1)
                    End If
                Next
            Next

            Dim extraBurials As String = ""
            Dim i As Integer = 0

            For Each item In spacesenv.features
                i = 0
                If owner.Spaces.Where(Function(w) w.Space_ID = item.attributes.Space_ID).Count > 0 Then
                    'Dim sp As Owned_Space = owner.Spaces.Where(Function(w) w.Space_ID = item.attributes.Space_ID).First
                    Dim bp = db.Burials.Where(Function(w) w.Space_ID = item.attributes.Space_ID)

                    Dim label As String = ""
                    For Each b In bp.ToArray
                        If i < 2 Or spacesenv.features.Count < 7 Then
                            label = label + "PN:" + b.Permit_Number + " | SN:" + b.Space_ID.Substring(11, 3).TrimStart("0") + " | " + If(b.Work_Order_Date.HasValue, b.Work_Order_Date.Value.ToShortDateString, "00/00/0000") + vbNewLine _
                        + b.First_Name + " " + b.Last_Name + vbNewLine + If(b.Date_of_Birth.HasValue, "DOB:" + b.Date_of_Birth.Value.ToShortDateString + " | ", "DOB:00/00/0000 | ") + If(b.Date_of_Death.HasValue, "DOB:" + b.Date_of_Death.Value.ToShortDateString, "DOB:00/00/0000") + vbNewLine
                        Else
                            extraBurials = extraBurials + "PN:" + b.Permit_Number + " | SN:" + b.Space_ID.Substring(11, 3).TrimStart("0") + " | " + If(b.Work_Order_Date.HasValue, b.Work_Order_Date.Value.ToShortDateString, "00/00/0000") + vbNewLine _
                        + b.First_Name + " " + b.Last_Name + vbNewLine + If(b.Date_of_Birth.HasValue, "DOB:" + b.Date_of_Birth.Value.ToShortDateString + " | ", "DOB:00/00/0000 | ") + If(b.Date_of_Death.HasValue, "DOB:" + b.Date_of_Death.Value.ToShortDateString, "DOB:00/00/0000") + vbNewLine + "___________________" + vbNewLine
                        End If
                        i += 1
                    Next
                    If label = "" Then
                        Dim os = db.Owned_Spaces.Where(Function(w) w.Space_ID = item.attributes.Space_ID)
                        For Each o In os.ToArray
                            label = label + "Owner: " + o.Owner.Owner_Name + vbNewLine + "SN: " + o.Space_ID.Substring(11, 3).TrimStart("0") + vbNewLine
                        Next
                    End If

                    drawSpace(gfx2, label, item.geometry.rings, XBrushes.White, bottom_left_x, bottom_left_y, top_right_x, top_right_y)
                Else

                    'Dim sp As Owned_Space = db.Owned_Spaces.Where(Function(w) w.Space_ID = item.attributes.Space_ID).First
                    Dim bp = db.Burials.Where(Function(w) w.Space_ID = item.attributes.Space_ID)

                    Dim label As String = ""
                    For Each b In bp.ToArray
                        label = label + "PN:" + b.Permit_Number + " | SN:" + b.Space_ID.Substring(11, 3).TrimStart("0") + " | " + If(b.Work_Order_Date.HasValue, b.Work_Order_Date.Value.ToShortDateString, "00/00/0000") + vbNewLine _
                        + b.First_Name + " " + b.Last_Name + vbNewLine
                    Next
                    If label = "" Then
                        Dim os = db.Owned_Spaces.Where(Function(w) w.Space_ID = item.attributes.Space_ID)
                        For Each o In os.ToArray
                            label = label + "Owner: " + o.Owner.Owner_Name + vbNewLine + "SN: " + o.Space_ID.Substring(11, 3).TrimStart("0") + vbNewLine
                        Next
                    End If

                    If label = "" Then
                        'Dim sp As Owned_Space = db.Owned_Spaces.Where(Function(w) w.Space_ID = item.attributes.Space_ID).First
                        'label = "Owner: " + sp.Owner.Owner_Name + vbNewLine + "SN: " + sp.Space_ID.Substring(11, 3).TrimStart("0")
                        label = item.attributes.Space_ID.Substring(11, 3).TrimStart("0") + " - Unowned"
                    End If
                    drawSpace(gfx2, label, item.geometry.rings, XBrushes.LightGray, bottom_left_x, bottom_left_y, top_right_x, top_right_y)
                End If
            Next

            If extraBurials <> "" Then
                drawExtraSpaceLabel(gfx2, extraBurials, bottom_left_x, bottom_left_y, top_right_x, top_right_y)
            End If

            page1.Width = New XUnit(8.66, XGraphicsUnit.Inch)
            page2.Width = New XUnit(8.66, XGraphicsUnit.Inch)
            document.Save(stream, False)

            Return File(stream, "application/pdf")
        End Function

        Private Sub drawSpace(ByRef gfx As XGraphics, ByVal label As String, ByVal ring As List(Of List(Of List(Of Double))), ByVal brush As XBrush, ByVal xmin As Double, ByVal ymin As Double, ByVal xmax As Double, ByVal ymax As Double)

            Dim points As New List(Of XPoint)
            Dim margin As Double = 7.2 * 4
            Dim yscale As Double = (gfx.PdfPage.Height.Point - 2 * margin) / (ymax - ymin)
            Dim xscale As Double = (gfx.PdfPage.Width.Point - 2 * margin) / (xmax - xmin)
            Dim scale As Double = 0
            If xscale > yscale Then
                scale = yscale
            Else
                scale = xscale
            End If
            For Each point In ring.Item(0)
                points.Add(New XPoint(margin + scale * (point.Item(0) - xmin), gfx.PdfPage.Height.Point - margin - scale * (point.Item(1) - ymin)))
            Next
            Dim top_right_x As Double = 0
            Dim top_right_y As Double = 0
            Dim bottom_left_x As Double = 0
            Dim bottom_left_y As Double = 0
            For Each point In points
                If top_right_x = 0 Or point.X > top_right_x Then
                    top_right_x = point.X
                End If
                If top_right_y = 0 Or point.Y < top_right_y Then
                    top_right_y = point.Y
                End If
                If bottom_left_x = 0 Or point.X < bottom_left_x Then
                    bottom_left_x = point.X
                End If
                If bottom_left_y = 0 Or point.Y > bottom_left_y Then
                    bottom_left_y = point.Y
                End If
            Next
            'Dim brush As New XBrushes
            Dim tf As New XTextFormatter(gfx)
            gfx.DrawPolygon(brush, points.ToArray, XFillMode.Winding)
            gfx.DrawPolygon(New XPen(XColor.FromArgb(0, 0, 0)), points.ToArray)
            tf.DrawString(label, New XFont("Arial", 6, XFontStyle.Regular), XBrushes.Black, New XRect(bottom_left_x + 0.25 * scale, top_right_y + 0.25 * scale, top_right_x - bottom_left_x, bottom_left_y - top_right_y), XStringFormats.TopLeft)
        End Sub

        Private Sub drawSpaceAssignOfInterest(ByRef gfx As XGraphics, ByVal label As String, ByVal ring As List(Of List(Of List(Of Double))), ByVal brush As XBrush, ByVal xmin As Double, ByVal ymin As Double, ByVal xmax As Double, ByVal ymax As Double)

            Dim points As New List(Of XPoint)
            Dim margin As Double = 7.2 * 4
            Dim yscale As Double = (195 - 2 * margin) / (ymax - ymin)
            Dim xscale As Double = (225 - 2 * margin) / (xmax - xmin)
            Dim scale As Double = 0
            If xscale > yscale Then
                scale = yscale
            Else
                scale = xscale
            End If
            For Each point In ring.Item(0)
                points.Add(New XPoint(margin + 28 + scale * (point.Item(0) - xmin), gfx.PdfPage.Height.Point - 110 - margin - scale * (point.Item(1) - ymin)))
            Next
            Dim top_right_x As Double = 0
            Dim top_right_y As Double = 0
            Dim bottom_left_x As Double = 0
            Dim bottom_left_y As Double = 0
            For Each point In points
                If top_right_x = 0 Or point.X > top_right_x Then
                    top_right_x = point.X
                End If
                If top_right_y = 0 Or point.Y < top_right_y Then
                    top_right_y = point.Y
                End If
                If bottom_left_x = 0 Or point.X < bottom_left_x Then
                    bottom_left_x = point.X
                End If
                If bottom_left_y = 0 Or point.Y > bottom_left_y Then
                    bottom_left_y = point.Y
                End If
            Next
            'Dim brush As New XBrushes
            Dim tf As New XTextFormatter(gfx)
            gfx.DrawPolygon(brush, points.ToArray, XFillMode.Winding)
            gfx.DrawPolygon(New XPen(XColor.FromArgb(0, 0, 0)), points.ToArray)
            tf.DrawString(label, New XFont("Arial", 6, XFontStyle.Regular), XBrushes.Black, New XRect(bottom_left_x + 0.25 * scale, top_right_y + 0.25 * scale, top_right_x - bottom_left_x, bottom_left_y - top_right_y), XStringFormats.TopLeft)
        End Sub

        Private Sub drawExtraSpaceLabel(ByRef gfx As XGraphics, ByVal label As String, ByVal xmin As Double, ByVal ymin As Double, ByVal xmax As Double, ByVal ymax As Double)
            Dim margin As Double = 7.2 * 4
            Dim yscale As Double = (gfx.PdfPage.Height.Point - 2 * margin) / (ymax - ymin)
            Dim xscale As Double = (gfx.PdfPage.Width.Point - 2 * margin) / (xmax - xmin)
            Dim scale As Double = 0
            If xscale > yscale Then
                scale = yscale
            Else
                scale = xscale
            End If
            'Dim brush As New XBrushes
            Dim tf As New XTextFormatter(gfx)
            tf.DrawString(label, New XFont("Arial", 7, XFontStyle.Regular), XBrushes.Black, New XRect(margin + scale * (xmax - xmin) + 10, margin, gfx.PdfPage.Width.Point - (margin + scale * (xmax - xmin) + 10), gfx.PdfPage.Height.Point - (2 * margin)), XStringFormats.TopLeft)
        End Sub

        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function certificateOfOwnership(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owner As Owner = db.Owners.Find(id)
            If IsNothing(owner) Then
                Return HttpNotFound()
            End If
            Dim stream As New MemoryStream

            ' Create a new PDF document
            Dim document As PdfDocument = New PdfDocument
            document.Info.Title = "Certificate of Ownership"

            ' Create an empty page
            Dim page As PdfPage = document.AddPage

            ' Get an XGraphics object for drawing
            Dim gfx As XGraphics = XGraphics.FromPdfPage(page)
            Dim tf As New XTextFormatter(gfx)

            ' Create a font
            Dim font As XFont = New XFont("Arial", 10, XFontStyle.Regular)
            Dim deedFont As XFont = New XFont("Times New Roman", 11, XFontStyle.Regular)
            Dim deedFontItalic As XFont = New XFont("Times New Roman", 11, XFontStyle.Italic)
            Dim TitleFontBold As XFont = New XFont("Times New Roman", 16, XFontStyle.Bold)
            Dim pen As XPen = New XPen(XColor.FromArgb(0, 0, 0))

            tf.Alignment = XParagraphAlignment.Center

            tf.DrawString("Certificate of Ownership", TitleFontBold, XBrushes.Black, _
            New XRect(0, 39.6, page.Width.Point, 50.4))

            ' Draw the text
            Dim x1 As Double = 36
            Dim y1 As Double = 12.5 * 7.2
            Dim y2 As Double = 14 * 7.2
            Dim x2 As Double = 16.5 * 7.2
            Dim verticleSpace As Double = 3 * 7.2

            tf.Alignment = XParagraphAlignment.Left
            tf.DrawString("Deed No.:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString("Amount Paid: " + Format(owner.Amount_Paid, "$#,##0.00"), font, XBrushes.Black, New XRect(x1, y1, page.Width.Point - (2 * x1), y2 - y1))

            tf.Alignment = XParagraphAlignment.Left
            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Issued To:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString("Address:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString("Space No.:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.DrawString("Remarks:", font, XBrushes.Black, New XRect(x1 + 33 * 7.2, y1, x2 - x1, y2 - y1))


            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Lot/Row:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Block:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Division:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Cemetery:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Phone:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Date of Purchase:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))


            x1 = 16.5 * 7.2
            y1 = 12.5 * 7.2
            y2 = 14 * 7.2
            x2 = page.Width.Point - 36

            tf.DrawString(If(String.IsNullOrEmpty(owner.Deed_No), "", owner.Deed_No), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.Deed_Name), owner.Owner_Name, owner.Deed_Name), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, 2 * (y2 - y1)))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.Owner_Address), "", owner.Owner_Address), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, 2 * (y2 - y1)))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.spaceList.Item(0)), "", owner.spaceList().Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.DrawString(If(String.IsNullOrEmpty(owner.Remarks), "", owner.Remarks), font, XBrushes.Black, New XRect(45 * 7.2, y1, page.Width.Point - 45 * 7.2 - 36, 20 * 7.2))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.lotRowList.Item(0)), "", owner.lotRowList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.blockList.Item(0)), "", owner.blockList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.divisionList.Item(0)), "", owner.divisionList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.cemeteryList.Item(0)), "", owner.cemeteryList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.Phone_Number), "", owner.Phone_Number), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(owner.Date_of_Purchase.HasValue, owner.Date_of_Purchase.Value.ToShortDateString, "00/00/0000"), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            gfx.DrawLine(pen, New XPoint(36, 50 * 7.2), New XPoint(page.Width.Point - 36, 50 * 7.2))
            gfx.DrawLine(pen, New XPoint(36, page.Height.Point - 36), New XPoint(page.Width.Point - 36, page.Height.Point - 36))
            gfx.DrawLine(pen, New XPoint(36, 50 * 7.2), New XPoint(36, page.Height.Point - 36))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36, 50 * 7.2), New XPoint(page.Width.Point - 36, page.Height.Point - 36))

            gfx.DrawLine(pen, New XPoint(36 + 14.5, 50 * 7.2 + 14.5), New XPoint(page.Width.Point - 36 - 14.5, 50 * 7.2 + 14.5))
            gfx.DrawLine(pen, New XPoint(36 + 14.5, page.Height.Point - 36 - 14.5), New XPoint(page.Width.Point - 36 - 14.5, page.Height.Point - 36 - 14.5))
            gfx.DrawLine(pen, New XPoint(36 + 14.5, 50 * 7.2 + 14.5), New XPoint(36 + 14.5, page.Height.Point - 36 - 14.5))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36 - 14.5, 50 * 7.2 + 14.5), New XPoint(page.Width.Point - 36 - 14.5, page.Height.Point - 36 - 14.5))

            gfx.DrawLine(pen, New XPoint(36 + 14.5, 50 * 7.2 + 14.5), New XPoint(36, 50 * 7.2))
            gfx.DrawLine(pen, New XPoint(36 + 14.5, page.Height.Point - 36 - 14.5), New XPoint(36, page.Height.Point - 36))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36, 50 * 7.2), New XPoint(page.Width.Point - 36 - 14.5, 50 * 7.2 + 14.5))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36, page.Height.Point - 36), New XPoint(page.Width.Point - 36 - 14.5, page.Height.Point - 36 - 14.5))

            gfx.DrawLine(pen, New XPoint(11 * 7.2, page.Height.Point - 10.5 * 7.2), New XPoint(32 * 7.2, page.Height.Point - 10.5 * 7.2))
            tf.DrawString("City Clerk", deedFont, XBrushes.Black, New XRect(11 * 7.2, page.Height.Point - 10.5 * 7.2, 21 * 7.2, 1.5 * 7.2))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 32 * 7.2, page.Height.Point - 10.5 * 7.2), New XPoint(page.Width.Point - 11 * 7.2, page.Height.Point - 10.5 * 7.2))
            tf.DrawString("Mayor of " + My.Settings.OrganizationName_Short, deedFont, XBrushes.Black, New XRect(page.Width.Point - 32 * 7.2, page.Height.Point - 10.5 * 7.2, 21 * 7.2, 1.5 * 7.2))

            tf.DrawString("Deed No.  " + owner.Deed_No, deedFont, XBrushes.Black, New XRect(36 + 21.7, 50 * 7.2 + 21.5, page.Width.Point - ((36 + 21.7) * 2), 1.5 * 7.2))
            tf.Alignment = XParagraphAlignment.Center

            tf.DrawString("Certificate of Ownership", TitleFontBold, XBrushes.Black, _
            New XRect(0, 414, page.Width.Point, 50.4))
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString("Amount Paid:  " + Format(owner.Amount_Paid, "$#,###.00"), deedFont, XBrushes.Black, New XRect(36 + 21.7, 50 * 7.2 + 21.5, page.Width.Point - ((36 + 21.7) * 2), 1.5 * 7.2))
            tf.DrawString(My.Settings.OrganizationName_Short + ", " + My.Settings.State + ", " + If(owner.Date_of_Purchase.HasValue, owner.Date_of_Purchase.Value.ToString("MMMM d, yyyy"), ""), deedFontItalic, XBrushes.Black, New XRect(36 + 21.7, 58.25 * 7.2 + 21.5, page.Width.Point - ((36 + 21.7) * 2), 1.5 * 7.2))
            tf.Alignment = XParagraphAlignment.Left
            Dim text As String = "THIS IS TO CERTIFY, That " + If(String.IsNullOrEmpty(owner.Deed_Name), owner.Owner_Name, owner.Deed_Name) + " has purchased of the " + My.Settings.OrganizationName + ", " + My.Settings.State + ", Space No. " + owner.spaceList().Item(0) + " in Lot/Row No. " + owner.lotRowList().Item(0) + _
        " in Block No. " + owner.blockList().Item(0) + ", " + owner.divisionList().Item(0) + " Division or Re-Plat, " + owner.cemeteryList().Item(0) + " Cemetery, of THE " + My.Settings.OrganizationName_Short.ToUpper + " PUBLIC CEMETERIES, for the sum " + _
        "of " + Format(owner.Amount_Paid, "$#,###.00") + " Dollars, the receipt of which is hereby acknowledged: and it is further acknowledged that said purchase is the owner of said " + _
        "property for the purpose of internment only." + _
        vbNewLine + _
        vbNewLine + _
        "And the " + My.Settings.OrganizationName + " obligates itself to forever warrant and defend the title and peaceable possession of the same to the said " + If(String.IsNullOrEmpty(owner.Deed_Name), owner.Owner_Name, owner.Deed_Name) + ", the heirs and assigns, " + _
        "on condition that said land be used only for burying the dead, under the provisions of the Ordinances of said City relating to Cemeteries, assignment of any portion of " + _
        "interest being valid only upon the written consent of the " + My.Settings.OrganizationName + "." + _
        vbNewLine + _
        vbNewLine + _
        "IN WITNESS WHEREOF, The " + My.Settings.OrganizationName + " has, this " + addsuffix(owner.Date_of_Purchase.Value.Day) + " day of " + owner.Date_of_Purchase.Value.ToString("MMMM, yyyy") + ", caused this certificate to be signed by its Mayor and City Clerk." + _
        vbNewLine + _
        vbNewLine + _
        "Attest:" + _
        vbNewLine + _
        vbNewLine

            tf.DrawString(text, deedFont, XBrushes.Black, New XRect(36 + 21.7, 62 * 7.2 + 14.5, page.Width.Point - ((36 + 21.7) * 2), 45 * 7.2))

            Dim cc As City_Clerk = db.City_Clerks.Where(Function(w) w.Start_Date <= owner.Date_of_Purchase And w.End_Date >= owner.Date_of_Purchase).First
            Dim image As XImage = XImage.FromFile(cc.Signature_Path)
            gfx.DrawImage(image, New XPoint(80, 685))

            Dim mayor As Mayor = db.Mayors.Where(Function(w) w.Start_Date <= owner.Date_of_Purchase And w.End_Date >= owner.Date_of_Purchase).First
            image = XImage.FromFile(mayor.Signature_Path)
            gfx.DrawImage(image, New XPoint(380, 685))
            image.Dispose()

            document.Save(stream, False)

            Return File(stream, "application/pdf")
        End Function

        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
<OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function assignmentOfInterest(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owner As Owner = db.Owners.Find(id)

            If IsNothing(owner) Then
                Return HttpNotFound()
            End If
            Dim original_owner As Owner = db.Owners.Find(owner.Spaces.First.Assignments.Where(Function(w) w.Assignment_Date = owner.Date_of_Purchase).First.Original_OwnerID)
            Dim stream As New MemoryStream

            ' Create a new PDF document
            Dim document As PdfDocument = New PdfDocument
            document.Info.Title = "Assignment of Interest"

            ' Create an empty page
            Dim page As PdfPage = document.AddPage

            ' Get an XGraphics object for drawing
            Dim gfx As XGraphics = XGraphics.FromPdfPage(page)
            Dim tf As New XTextFormatter(gfx)

            ' Create a font
            Dim font As XFont = New XFont("Arial", 10, XFontStyle.Regular)
            Dim deedFont As XFont = New XFont("Times New Roman", 11, XFontStyle.Regular)
            Dim deedFontItalic As XFont = New XFont("Times New Roman", 11, XFontStyle.Italic)
            Dim TitleFontBold As XFont = New XFont("Times New Roman", 16, XFontStyle.Bold)
            Dim TitleFontBoldSmall As XFont = New XFont("Times New Roman", 13, XFontStyle.Bold)
            Dim pen As XPen = New XPen(XColor.FromArgb(0, 0, 0))

            tf.Alignment = XParagraphAlignment.Center

            tf.DrawString("ASSIGNMENT RECORD", TitleFontBold, XBrushes.Black, _
            New XRect(0, 39.6, page.Width.Point, 50.4))

            ' Draw the text
            Dim x1 As Double = 36
            Dim y1 As Double = 12.5 * 7.2
            Dim y2 As Double = 14 * 7.2
            Dim x2 As Double = 18 * 7.2
            Dim verticleSpace As Double = 3 * 7.2

            tf.Alignment = XParagraphAlignment.Left
            tf.DrawString("Assignment No.:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString("Amount Paid: " + Format(owner.Amount_Paid, "$#,##0.00"), font, XBrushes.Black, New XRect(x1, y1, page.Width.Point - (2 * x1), y2 - y1))

            tf.Alignment = XParagraphAlignment.Left
            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Present Owner:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString("Assigned to:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString("Space No.:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.DrawString("Address:", font, XBrushes.Black, New XRect(x1 + 33 * 7.2, y1, x2 - x1, y2 - y1))


            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Lot/Row:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Block:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Division:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Cemetery:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.DrawString("Remarks:", font, XBrushes.Black, New XRect(x1 + 33 * 7.2, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Phone:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString("Date of Assignment:", font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))


            x1 = 18 * 7.2
            y1 = 12.5 * 7.2
            y2 = 14 * 7.2
            x2 = page.Width.Point - 36

            tf.DrawString(If(String.IsNullOrEmpty(owner.Deed_No), "", owner.Deed_No), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(original_owner.Deed_Name), original_owner.Owner_Name, original_owner.Deed_Name), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, 2 * (y2 - y1)))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.Deed_Name), owner.Owner_Name, owner.Deed_Name), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, 2 * (y2 - y1)))

            y1 = y1 + 2 * verticleSpace
            y2 = y2 + 2 * verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.spaceList.Item(0)), "", owner.spaceList().Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.DrawString(If(String.IsNullOrEmpty(owner.Owner_Address), "", owner.Owner_Address), font, XBrushes.Black, New XRect(45 * 7.2, y1, page.Width.Point - 45 * 7.2 - 36, 20 * 7.2))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.lotRowList.Item(0)), "", owner.lotRowList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.blockList.Item(0)), "", owner.blockList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.divisionList.Item(0)), "", owner.divisionList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.cemeteryList.Item(0)), "", owner.cemeteryList.Item(0)), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))
            tf.DrawString(If(String.IsNullOrEmpty(owner.Remarks), "", owner.Remarks), font, XBrushes.Black, New XRect(45 * 7.2, y1, page.Width.Point - 45 * 7.2 - 36, 20 * 7.2))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(String.IsNullOrEmpty(owner.Phone_Number), "", owner.Phone_Number), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            y1 = y1 + verticleSpace
            y2 = y2 + verticleSpace
            tf.DrawString(If(owner.Date_of_Purchase.HasValue, owner.Date_of_Purchase.Value.ToShortDateString, "00/00/0000"), font, XBrushes.Black, New XRect(x1, y1, x2 - x1, y2 - y1))

            gfx.DrawLine(pen, New XPoint(36, 50 * 7.2), New XPoint(page.Width.Point - 36, 50 * 7.2))
            gfx.DrawLine(pen, New XPoint(36, page.Height.Point - 36), New XPoint(page.Width.Point - 36, page.Height.Point - 36))
            gfx.DrawLine(pen, New XPoint(36, 50 * 7.2), New XPoint(36, page.Height.Point - 36))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36, 50 * 7.2), New XPoint(page.Width.Point - 36, page.Height.Point - 36))

            gfx.DrawLine(pen, New XPoint(36 + 14.5, 50 * 7.2 + 14.5), New XPoint(page.Width.Point - 36 - 14.5, 50 * 7.2 + 14.5))
            gfx.DrawLine(pen, New XPoint(36 + 14.5, page.Height.Point - 36 - 14.5), New XPoint(page.Width.Point - 36 - 14.5, page.Height.Point - 36 - 14.5))
            gfx.DrawLine(pen, New XPoint(36 + 14.5, 50 * 7.2 + 14.5), New XPoint(36 + 14.5, page.Height.Point - 36 - 14.5))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36 - 14.5, 50 * 7.2 + 14.5), New XPoint(page.Width.Point - 36 - 14.5, page.Height.Point - 36 - 14.5))

            gfx.DrawLine(pen, New XPoint(36 + 14.5, 50 * 7.2 + 14.5), New XPoint(36, 50 * 7.2))
            gfx.DrawLine(pen, New XPoint(36 + 14.5, page.Height.Point - 36 - 14.5), New XPoint(36, page.Height.Point - 36))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36, 50 * 7.2), New XPoint(page.Width.Point - 36 - 14.5, 50 * 7.2 + 14.5))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 36, page.Height.Point - 36), New XPoint(page.Width.Point - 36 - 14.5, page.Height.Point - 36 - 14.5))

            gfx.DrawLine(pen, New XPoint(11 * 7.2, page.Height.Point - 10.5 * 7.2), New XPoint(32 * 7.2, page.Height.Point - 10.5 * 7.2))
            tf.DrawString("City Clerk", deedFont, XBrushes.Black, New XRect(11 * 7.2, page.Height.Point - 10.5 * 7.2, 21 * 7.2, 1.5 * 7.2))
            gfx.DrawLine(pen, New XPoint(page.Width.Point - 32 * 7.2, page.Height.Point - 10.5 * 7.2), New XPoint(page.Width.Point - 11 * 7.2, page.Height.Point - 10.5 * 7.2))
            tf.DrawString("Mayor of " + My.Settings.OrganizationName_Short, deedFont, XBrushes.Black, New XRect(page.Width.Point - 32 * 7.2, page.Height.Point - 10.5 * 7.2, 21 * 7.2, 1.5 * 7.2))

            gfx.DrawLine(pen, New XPoint(page.Width.Point - 32 * 7.2, page.Height.Point - 32 * 7.2), New XPoint(page.Width.Point - 11 * 7.2, page.Height.Point - 32 * 7.2))
            tf.DrawString("(Owner)", deedFont, XBrushes.Black, New XRect(page.Width.Point - 32 * 7.2, page.Height.Point - 32 * 7.2, 21 * 7.2, 1.5 * 7.2))

            tf.DrawString("Assignment No.  " + owner.Deed_No, deedFont, XBrushes.Black, New XRect(36 + 21.7, 50 * 7.2 + 21.5, page.Width.Point - ((36 + 21.7) * 2), 1.5 * 7.2))
            tf.Alignment = XParagraphAlignment.Center

            tf.DrawString("Assignment of Interest by Owner", TitleFontBold, XBrushes.Black, _
            New XRect(0, 414, page.Width.Point, 50.4))
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString("Amount Paid:  " + Format(owner.Amount_Paid, "$#,###.00"), deedFont, XBrushes.Black, New XRect(36 + 21.7, 50 * 7.2 + 21.5, page.Width.Point - ((36 + 21.7) * 2), 1.5 * 7.2))
            tf.DrawString(My.Settings.OrganizationName_Short + ", " + My.Settings.State + ", " + If(owner.Date_of_Purchase.HasValue, owner.Date_of_Purchase.Value.ToString("MMMM d, yyyy"), ""), deedFontItalic, XBrushes.Black, New XRect(36 + 21.7, 58.25 * 7.2 + 21.5, page.Width.Point - ((36 + 21.7) * 2), 1.5 * 7.2))
            tf.Alignment = XParagraphAlignment.Left
            Dim text As String = "     The interest of " + If(String.IsNullOrEmpty(original_owner.Deed_Name), original_owner.Owner_Name, original_owner.Deed_Name) + " (as owner of the property covered by Certificate of Ownership Number  " + original_owner.Deed_No + ") is hereby assigned to - " + If(String.IsNullOrEmpty(owner.Deed_Name), owner.Owner_Name, owner.Deed_Name) + _
                ", subject to the consent of the " + My.Settings.OrganizationName + ", this " + addsuffix(owner.Date_of_Purchase.Value.Day) + " day of " + owner.Date_of_Purchase.Value.ToString("MMMM, yyyy") + " on the portions hereinafter described - Space(s) " + owner.spaceList().Item(0) + " in " + owner.cemeteryList().Item(0) + " Cemetery, Division " + owner.divisionList().Item(0) + _
                ", Block " + owner.blockList().Item(0) + ", Lot/Row " + owner.lotRowList().Item(0) + ", " + My.Settings.OrganizationName_Short + ", " + My.Settings.State + "." + _
                vbNewLine + vbNewLine + vbNewLine + vbNewLine + vbNewLine + vbNewLine + vbNewLine + vbNewLine + vbNewLine + vbNewLine + _
                vbNewLine + _
                vbNewLine + _
                vbNewLine + _
                "Attest:" + _
                vbNewLine + _
                vbNewLine
            Dim text2 As String = "     The " + My.Settings.OrganizationName + ", " + My.Settings.State + ", hereby consents that the interest of " + If(String.IsNullOrEmpty(original_owner.Deed_Name), original_owner.Owner_Name, original_owner.Deed_Name) + " (as owner of the property covered by Certificate of Ownership Number  " + original_owner.Deed_No + ") be assigned to " + _
                If(String.IsNullOrEmpty(owner.Deed_Name), owner.Owner_Name, owner.Deed_Name) + " on the portions above designated, on this " + addsuffix(owner.Date_of_Purchase.Value.Day) + " day of " + owner.Date_of_Purchase.Value.ToString("MMMM, yyyy") + "."

            tf.DrawString(text, deedFont, XBrushes.Black, New XRect(36 + 21.7, 62 * 7.2 + 14.5, page.Width.Point - ((36 + 21.7) * 2), 45 * 7.2))
            tf.DrawString(text2, deedFont, XBrushes.Black, New XRect(230, 609, 332, 120))
            tf.Alignment = XParagraphAlignment.Center
            tf.DrawString("Consent by " + My.Settings.OrganizationName + " to Assignment of Interest", TitleFontBoldSmall, XBrushes.Black, _
            New XRect(230, 580, 332, 21))
            tf.Alignment = XParagraphAlignment.Left
            Dim cc As City_Clerk = db.City_Clerks.Where(Function(w) w.Start_Date <= owner.Date_of_Purchase And w.End_Date >= owner.Date_of_Purchase).First
            Dim image As XImage = XImage.FromFile(cc.Signature_Path)
            gfx.DrawImage(image, New XPoint(80, 685))

            Dim mayor As Mayor = db.Mayors.Where(Function(w) w.Start_Date <= owner.Date_of_Purchase And w.End_Date >= owner.Date_of_Purchase).First
            image = XImage.FromFile(mayor.Signature_Path)
            gfx.DrawImage(image, New XPoint(380, 685))
            image.Dispose()


            Dim spaces_in As String = ""

            For Each item In owner.Spaces
                spaces_in += "'" + item.Space_ID + "',"
            Next
            spaces_in = "(" + spaces_in.TrimEnd(",") + ")"
            'Dim label As String = ""


            Dim request As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=SPACE_ID+IN+" + spaces_in + "&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
            Dim ws As WebResponse = request.GetResponse()
            Dim jsonSerializer As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
            Dim spaces As GIS_Spaces = jsonSerializer.ReadObject(ws.GetResponseStream())
            Dim bottom_left_x As Double = 0
            Dim bottom_left_y As Double = 0

            Dim top_right_x As Double = 0
            Dim top_right_y As Double = 0

            For Each item In spaces.features
                For Each x In item.geometry.rings(0)
                    If top_right_x = 0 Or x.Item(0) > top_right_x Then
                        top_right_x = x.Item(0)
                    End If
                    If top_right_y = 0 Or x.Item(1) > top_right_y Then
                        top_right_y = x.Item(1)
                    End If
                    If bottom_left_x = 0 Or x.Item(0) < bottom_left_x Then
                        bottom_left_x = x.Item(0)
                    End If
                    If bottom_left_y = 0 Or x.Item(1) < bottom_left_y Then
                        bottom_left_y = x.Item(1)
                    End If
                Next
            Next
            Dim requestenv As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=&text=&objectIds=&time=&geometry=" + bottom_left_x.ToString + "%2C" + bottom_left_y.ToString + "%2C" + top_right_x.ToString + "%2C" + top_right_y.ToString + "&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
            Dim wsenv As WebResponse = requestenv.GetResponse()
            Dim jsonSerializerenv As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
            Dim spacesenv As GIS_Spaces = jsonSerializer.ReadObject(wsenv.GetResponseStream())
            Dim scale As String = ""
            For Each item In spacesenv.features
                For Each x In item.geometry.rings(0)
                    If top_right_x = 0 Or x.Item(0) > top_right_x Then
                        top_right_x = x.Item(0)
                    End If
                    If top_right_y = 0 Or x.Item(1) > top_right_y Then
                        top_right_y = x.Item(1)
                    End If
                    If bottom_left_x = 0 Or x.Item(0) < bottom_left_x Then
                        bottom_left_x = x.Item(0)
                    End If
                    If bottom_left_y = 0 Or x.Item(1) < bottom_left_y Then
                        bottom_left_y = x.Item(1)
                    End If
                Next
            Next

            Dim i As Integer = 0

            For Each item In spacesenv.features
                i = 0
                If owner.Spaces.Where(Function(w) w.Space_ID = item.attributes.Space_ID).Count > 0 Then
                    Dim label As String = item.attributes.Space_ID
                    drawSpaceAssignOfInterest(gfx, label, item.geometry.rings, XBrushes.White, bottom_left_x, bottom_left_y, top_right_x, top_right_y)
                Else
                    Dim label As String = item.attributes.Space_ID
                    drawSpaceAssignOfInterest(gfx, label, item.geometry.rings, XBrushes.LightGray, bottom_left_x, bottom_left_y, top_right_x, top_right_y)
                End If
            Next

            document.Save(stream, False)

            Return File(stream, "application/pdf")
        End Function

        '
        ' GET: /Owner/Create
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Create(Optional Space_ID As String = "", Optional spaceList As String = "", Optional deedType As String = "") As ActionResult
            If Not String.IsNullOrEmpty(Space_ID) Then
                ViewBag.Space_ID = Space_ID
            End If
            If Not String.IsNullOrEmpty(spaceList) Then
                ViewBag.spaceList = spaceList
            End If
            If Not String.IsNullOrEmpty(deedType) Then
                ViewBag.deedType = deedType
                Dim owner As New Owner
                owner.Deed_Type = deedType
                Return View(owner)
            Else
                Dim owner As New Owner
                owner.Deed_Type = "D"
                Return View(owner)
            End If

        End Function

        '
        ' POST: /Owner/Create
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        Function Create(ByVal owner As Owner, Optional Space_ID As String = "", Optional spaceList As String = "", Optional deedType As String = "") As ActionResult
            If ModelState.IsValid Then
                Dim nextOwnerID As Integer
                If db.Owners.OrderByDescending(Function(o) o.ID).Count > 0 Then
                    nextOwnerID = db.Owners.OrderByDescending(Function(o) o.ID).First().ID + 1
                Else
                    nextOwnerID = 1
                End If

                owner.Deed_No = owner.Deed_Type + Format(nextOwnerID, "000000")
                If Not String.IsNullOrEmpty(Space_ID) Then
                    Dim os As New Owned_Space
                    os.Owner = owner
                    os.OwnerID = nextOwnerID
                    os.Space_ID = Space_ID
                    os.updateSpaceAvailability("No")
                    db.Owned_Spaces.Add(os)
                End If
                If Not String.IsNullOrEmpty(spaceList) Then
                    If deedType = "D" Then
                        For Each s In spaceList.Split(",")
                            Dim os As New Owned_Space
                            os.Owner = owner
                            os.OwnerID = nextOwnerID
                            os.Space_ID = s
                            os.updateSpaceAvailability("No")
                            db.Owned_Spaces.Add(os)
                        Next
                    ElseIf deedType = "A" Then
                        For Each s In spaceList.Split(",")
                            Dim previous_Owned_space As New Owned_Space
                            Dim assignment As New Assignment
                            previous_Owned_space = db.Owned_Spaces.Where(Function(w) w.Space_ID = s).First()

                            assignment.Assignment_Date = owner.Date_of_Purchase
                            assignment.Owned_Space = previous_Owned_space
                            assignment.Owned_SpaceID = previous_Owned_space.ID
                            assignment.Original_Owner = previous_Owned_space.Owner
                            assignment.Original_OwnerID = previous_Owned_space.OwnerID
                            assignment.New_Owner = owner
                            assignment.New_OwnerID = owner.ID

                            previous_Owned_space.OwnerID = owner.ID
                            previous_Owned_space.Owner = owner
                            db.Entry(previous_Owned_space).State = EntityState.Modified
                            db.Assignments.Add(assignment)
                        Next
                    End If
                End If
                db.Owners.Add(owner)
                db.SaveChanges()
                Return RedirectToAction("Edit", New With {.id = owner.ID})
            End If

            Return View(owner)
        End Function

        '
        ' GET: /Owner/Edit/5
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owner As Owner = db.Owners.Find(id)
            If IsNothing(owner) Then
                Return HttpNotFound()
            End If
            Dim spaceList As String = ""
            For Each s In owner.Spaces
                spaceList = spaceList + "'" + s.Space_ID + "',"
            Next
            spaceList = spaceList.TrimEnd(",")
            ViewBag.SpaceList = spaceList
            Return View(owner)
        End Function

        '
        ' POST: /Owner/Edit/5
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        Function Edit(ByVal owner As Owner) As ActionResult
            If ModelState.IsValid Then
                If owner.Deed_No.Substring(0, 1) = "A" Or owner.Deed_No.Substring(0, 1) = "D" Then
                    owner.Deed_No = owner.Deed_Type + Format(CType(owner.Deed_No.Substring(1).TrimStart("0"), Integer), "000000")
                ElseIf owner.Deed_No.Last = "A" Then
                    owner.Deed_Type = "A"
                    owner.Deed_No = owner.Deed_Type + Format(CType(owner.Deed_No.TrimStart("0").TrimEnd("A"), Integer), "000000")
                Else
                    owner.Deed_No = owner.Deed_Type + Format(CType(owner.Deed_No.TrimStart("0"), Integer), "000000")
                End If

                db.Entry(owner).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Details", New With {.id = owner.ID})
            End If

            Return View(owner)
        End Function

        '
        ' GET: /Owner/Delete/5
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owner As Owner = db.Owners.Find(id)
            If IsNothing(owner) Then
                Return HttpNotFound()
            End If
            Return View(owner)
        End Function

        '
        ' POST: /Owner/Delete/5
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        <ActionName("Delete")> _
        Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
            Dim owner As Owner = db.Owners.Find(id)
            db.Owners.Remove(owner)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            db.Dispose()
            MyBase.Dispose(disposing)
        End Sub
        Private Function addsuffix(ByVal num As Integer) As String
            Dim suff As String
            If num < 0 Then Return "Error"
            If num < 20 Then
                Select Case num
                    Case 1 : suff = "st"
                    Case 2 : suff = "nd"
                    Case 3 : suff = "rd"
                    Case 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 : suff = "th"
                End Select
            Else
                Select Case Convert.ToString(num).Chars(Convert.ToString(num).Length - 1)
                    Case "1" : suff = "st"
                    Case "2" : suff = "nd"
                    Case "3" : suff = "rd"
                    Case Else : suff = "th"
                End Select
            End If
            addsuffix = Convert.ToString(num) + suff
        End Function

        Function GetImage(ByVal URL As String) As System.Drawing.Image
            'You would require a Request object to send the
            'request to the web server holding this image.
            'Let us call it 'Request', juz for simplicity.
            Dim Request As System.Net.HttpWebRequest
            'And, ofcourse, a Response object to hold the
            'return chunk of data from the web server
            Dim Response As System.Net.HttpWebResponse


            'Now we need to actually ask the web server
            'for the image we want.
            Try
                'Creating the Request object is the first step of 'asking'
                Request = System.Net.WebRequest.Create(URL)
                'Actually this line can be split into two.
                'The first part is Request.GetResponse
                'which actually go about requesting for the
                'resource pointed to by the URL
                'The second step is casting it to the proper
                'type and assigning it to our Response object.
                Response = CType(Request.GetResponse, System.Net.WebResponse)


                If Request.HaveResponse Then 'Did we really get a response?
                    If Response.StatusCode = Net.HttpStatusCode.OK Then 'Is the status code 200? (You can check for more)
                        'So we have a response and it is an OK response.
                        'Now we can go about loading it to the output
                        'We know that the response (if OK) would contain
                        'an image. So load it to the function return, by
                        'using the static conversion function FromStream
                        'of the Image class. FromStream creates an Image out
                        'of a stream. Isn't that nice of FromStream? ;)
                        GetImage = System.Drawing.Image.FromStream(Response.GetResponseStream)
                    End If
                End If
                'What follows is the bad part... catching exceptions.
                'May his soul rest in peace if code flow comes to this part!
            Catch e As System.Net.WebException
                MsgBox("A web exception has occured [" & URL & "]." & vbCrLf & " System returned: " & e.Message, MsgBoxStyle.Exclamation, "Error!")
                Exit Try
            Catch e As System.Net.ProtocolViolationException
                MsgBox("A protocol violation has occured [" & URL & "]." & vbCrLf & "  System returned: " & e.Message, MsgBoxStyle.Exclamation, "Error!")
                Exit Try
            Catch e As System.Net.Sockets.SocketException
                MsgBox("Socket error [" & URL & "]." & vbCrLf & "  System returned: " & e.Message, MsgBoxStyle.Exclamation, "Error!")
                Exit Try
            Catch e As System.IO.EndOfStreamException
                MsgBox("An IO stream exception has occured. System returned: " & e.Message, MsgBoxStyle.Exclamation, "Error!")
                Exit Try
            Finally
            End Try
        End Function

        Public Function WebRequestinWebForm(url As String, postData As String) As String
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

        Private Sub updateSpaceAvailability(ByVal spaceID As String)
            Dim request As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=SPACE_ID='" + spaceID + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
            Dim ws As WebResponse = Request.GetResponse()
            Dim jsonSerializer As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
            Dim spaces As GIS_Spaces = jsonSerializer.ReadObject(ws.GetResponseStream())
            Dim space As Feature = spaces.features(0)
            space.attributes.Available = "No"
            Dim stream1 = New MemoryStream
            Dim ser As New Json.DataContractJsonSerializer(GetType(Feature))
            ser.WriteObject(stream1, Space)
            stream1.Position = 0
            Dim sr As StreamReader = New StreamReader(stream1)
            WebRequestinWebForm(My.Settings.SpacesFeatureService + "updateFeatures", "features=[" + sr.ReadToEnd() + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
        End Sub

    End Class
End Namespace