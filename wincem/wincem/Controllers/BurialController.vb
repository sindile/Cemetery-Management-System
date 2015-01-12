Imports System.Data.Entity
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports System.IO
Imports PdfSharp.Drawing.Layout
Imports LinqKit
Imports System.Net
Imports System.Runtime.Serialization

Namespace wincem
    Public Class BurialController
        Inherits System.Web.Mvc.Controller

        Private db As New cemeteryContext

        '
        ' GET: /Burial/

        Function Index(Optional searchString As String = "", Optional number_of_records As Integer = 50) As ActionResult
            Dim burials = From d In db.Burials
                        Select d
            If Not String.IsNullOrEmpty(searchString) Then
                burials = burials.Where(Function(s) s.First_Name.Contains(searchString) _
                                        Or s.Last_Name.Contains(searchString) _
                                        Or s.Ordered_BY.Contains(searchString) _
                                        )
            End If
            Return View(burials.ToList().Take(number_of_records))
        End Function
        Function burialTotals(Optional startDate As Date? = Nothing, Optional endDate As Date? = Nothing) As ActionResult
            Dim ds As String = "1/1/" + Now.Year.ToString
            Dim d As Date = Date.Now()
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


            ViewBag.Highland = db.Burials.Where(Function(w) w.Work_Order_Date > ds And w.Work_Order_Date < d And w.Space_ID.Substring(0, 1) = "1").Count
            ViewBag.Union = db.Burials.Where(Function(w) w.Work_Order_Date > ds And w.Work_Order_Date < d And w.Space_ID.Substring(0, 1) = "2").Count
            ViewBag.Graham = db.Burials.Where(Function(w) w.Work_Order_Date > ds And w.Work_Order_Date < d And w.Space_ID.Substring(0, 1) = "3").Count
            ViewBag.St_Marys = db.Burials.Where(Function(w) w.Work_Order_Date > ds And w.Work_Order_Date < d And w.Space_ID.Substring(0, 1) = "4").Count
            ViewBag.Total = ViewBag.Highland + ViewBag.Union + ViewBag.Graham + ViewBag.St_Marys


            Return PartialView()
        End Function
        '
        ' GET: /Permits/SearchBurials
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function SearchBurials(Optional searchString As String = "", Optional number_of_records As Integer = 50, Optional First_Name As Boolean = True, _
                               Optional Last_Name As Boolean = True, Optional Permit_Number As Boolean = True, Optional Burial_Date As Boolean = True, Optional Work_Order_Date As Boolean = True) As PartialViewResult
            Dim pred = PredicateBuilder.True(Of Burial)()

            Dim d As Date

            If Not String.IsNullOrEmpty(searchString) Then
                pred = PredicateBuilder.False(Of Burial)()
                If First_Name Then
                    pred = pred.Or(Function(s As Burial) s.First_Name.Contains(searchString))
                End If
                If Last_Name Then
                    pred = pred.Or(Function(s As Burial) s.Last_Name.Contains(searchString))
                End If
                If Last_Name And First_Name Then
                    pred = pred.Or(Function(s As Burial) (s.First_Name + " " + s.Last_Name).Contains(searchString))
                    pred = pred.Or(Function(s As Burial) (s.Last_Name + ", " + s.First_Name).Contains(searchString))
                End If
                If Permit_Number Then
                    pred = pred.Or(Function(s As Burial) s.Permit_Number.Contains(searchString))
                End If
                If Burial_Date And Date.TryParse(searchString, d) Then
                    Dim dateString As String = d.ToShortDateString
                    pred = pred.Or(Function(s As Burial) s.Burial_Date = dateString)
                End If
                If Work_Order_Date And Date.TryParse(searchString, d) Then
                    Dim dateString As String = d.ToShortDateString
                    pred = pred.Or(Function(s As Burial) s.Work_Order_Date = dateString)
                End If
            End If
            Return PartialView(db.Burials.AsExpandable().Where(pred).Take(number_of_records).ToList)
        End Function

        '
        ' GET: /Permits/SearchBurialsMap
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function SearchBurialsMap(Optional searchString As String = "", Optional number_of_records As Integer = 50, Optional First_Name As Boolean = True, _
                               Optional Last_Name As Boolean = True, Optional Permit_Number As Boolean = True, Optional Burial_Date As Boolean = True, Optional Work_Order_Date As Boolean = True) As PartialViewResult
            Dim pred = PredicateBuilder.True(Of Burial)()

            Dim d As Date

            If Not String.IsNullOrEmpty(searchString) Then
                pred = PredicateBuilder.False(Of Burial)()
                If First_Name Then
                    pred = pred.Or(Function(s As Burial) s.First_Name.Contains(searchString))
                End If
                If Last_Name Then
                    pred = pred.Or(Function(s As Burial) s.Last_Name.Contains(searchString))
                End If
                If Last_Name And First_Name Then
                    pred = pred.Or(Function(s As Burial) (s.First_Name + " " + s.Last_Name).Contains(searchString))
                    pred = pred.Or(Function(s As Burial) (s.Last_Name + ", " + s.First_Name).Contains(searchString))
                End If
                If Permit_Number Then
                    pred = pred.Or(Function(s As Burial) s.Permit_Number.Contains(searchString))
                End If
                If Burial_Date And Date.TryParse(searchString, d) Then
                    Dim dateString As String = d.ToShortDateString
                    pred = pred.Or(Function(s As Burial) s.Burial_Date = dateString)
                End If
                If Work_Order_Date And Date.TryParse(searchString, d) Then
                    Dim dateString As String = d.ToShortDateString
                    pred = pred.Or(Function(s As Burial) s.Work_Order_Date = dateString)
                End If
            End If
            Return PartialView(db.Burials.AsExpandable().Where(pred).Take(number_of_records).ToList)
        End Function
        '
        ' GET: /Burial/Details/5

        Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim burial As Burial = db.Burials.Find(id)
            If IsNothing(burial) Then
                Return HttpNotFound()
            End If
            ViewBag.owner_space_id = db.Owned_Spaces.Where(Function(w) w.Space_ID = burial.Space_ID).First.ID
            Return View(burial)
        End Function

        <Authorize(Roles:="Cem_Admins, Cem_Burial_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function burialRecord(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim burial As Burial = db.Burials.Find(id)
            If IsNothing(burial) Then
                Return HttpNotFound()
            End If
            Dim space As New Owned_Space
            space = db.Owned_Spaces.Where(Function(w) w.Space_ID = burial.Space_ID).First()
            ViewBag.cemetery = space.cemetery()
            ViewBag.division = space.division()
            ViewBag.block_section = space.Space_ID.Substring(5, 1)
            ViewBag.lot_row = space.Space_ID.Substring(7, 3)
            ViewBag.space = space.Space_ID.Substring(11, 3)
            Dim stream As New MemoryStream

            ' Create a new PDF document
            Dim document As PdfDocument = New PdfDocument
            document.Info.Title = "Burial Record"

            ' Create an empty page
            Dim page As PdfPage = document.AddPage

            ' Get an XGraphics object for drawing
            Dim gfx As XGraphics = XGraphics.FromPdfPage(page)
            Dim tf As New XTextFormatter(gfx)

            ' Create a font
            Dim font As XFont = New XFont("Arial", 12, XFontStyle.Regular)
            Dim TitleFont As XFont = New XFont("Arial", 18, XFontStyle.Regular)
            Dim TitleFontBold As XFont = New XFont("Arial", 20, XFontStyle.Bold)
            Dim pen As XPen = New XPen(XColor.FromArgb(0, 0, 0))

            gfx.DrawString(My.Settings.OrganizationName.ToUpper, TitleFont, XBrushes.Black, _
            New XRect(0, 39.6, page.Width.Point, 50.4), XStringFormats.TopCenter)
            gfx.DrawString("BURIAL RECORD", TitleFontBold, XBrushes.Black, _
            New XRect(0, 57.6, page.Width.Point, 72), XStringFormats.TopCenter)

            ' Draw the text
            Dim x1 As Double = 75.6
            Dim y1 As Double = 88.8
            Dim y2 As Double = 100.8
            Dim x2 As Double = 115
            Dim verticleSpace As Double = 28.0

            gfx.DrawString("Name", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))
            gfx.DrawString(burial.First_Name + " " + burial.Last_Name, font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (15 * 7.2)
            gfx.DrawString("Burial Record Type", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Burial_Record_Type), "", burial.Burial_Record_Type), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (9 * 7.2)
            gfx.DrawString("Ordered by", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Ordered_BY), "", burial.Ordered_BY), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (12 * 7.2)
            gfx.DrawString("Address/Phone", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Ordered_By_Address), "", burial.Ordered_By_Address) + " / " + If(String.IsNullOrEmpty(burial.Ordered_By_Phone), "", burial.Ordered_By_Phone), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (5.5 * 7.2)
            gfx.DrawString("Owner", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Owner.Deed_Name), burial.Owner.Owner_Name, burial.Owner.Deed_Name), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + If(burial.Owner.Deed_Type = "A", (11 * 7.2), (6 * 7.2))
            gfx.DrawString(If(burial.Owner.Deed_Type = "A", "Assignment #", "Deed #"), font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(burial.Owner.Deed_No, font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (17 * 7.2)
            gfx.DrawString("Date and Day of Burial", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(If(burial.Burial_Date.HasValue = False, "00/00/0000", burial.Burial_Date.Value.ToString("ddd") + ". - " + burial.Burial_Date.Value.ToShortDateString()), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (18.5 * 7.2), y2))
            gfx.DrawString("Time", font, XBrushes.Black, _
            New XRect(x1 + (46 * 7.2), y1, x2 + (33 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(If(burial.Burial_Time.HasValue = False, "00:00 AM", burial.Burial_Time.Value.ToShortTimeString), font, XBrushes.Black, _
            New XRect(x2 + (33 * 7.2), y1, x2 + (33 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (33 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (8.5 * 7.2)
            gfx.DrawString("Cremation", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(If(burial.Cremation = 0, "No", "Yes"), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (51 * 7.2), y2))
            gfx.DrawString("Garden of Remembrance", font, XBrushes.Black, _
            New XRect(x1 + (13.75 * 7.2), y1, x2 + (24 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(If(burial.Garden = 0, "No", "Yes"), font, XBrushes.Black, _
            New XRect(x2 + (25 * 7.2), y1, x2 + (25 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (25 * 7.2), y2), New XPoint(page.Width.Point - 75.6 - (26 * 7.2), y2))
            gfx.DrawString("Type of Service", font, XBrushes.Black, _
            New XRect(x1 + (38.5 * 7.2), y1, x2 + (42.5 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Type_of_Service), "", burial.Type_of_Service), font, XBrushes.Black, _
            New XRect(x2 + (42.5 * 7.2), y1, x2 + (42.5 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (42.5 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (7.5 * 7.2)
            gfx.DrawString("Container", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Container), "", burial.Container), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (7.5 * 7.2)
            gfx.DrawString("Cemetery", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(space.cemetery(), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (6.25 * 7.2)
            gfx.DrawString("Division", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(space.division(), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (43.5 * 7.2), y2))
            gfx.DrawString("Block/Section", font, XBrushes.Black, _
            New XRect(x1 + (20.75 * 7.2), y1, x2 + (25.25 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(space.Space_ID.Substring(5, 1), font, XBrushes.Black, _
            New XRect(x2 + (25.25 * 7.2), y1, x2 + (25.25 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (25.25 * 7.2), y2), New XPoint(page.Width.Point - 75.6 - (26.25 * 7.2), y2))
            gfx.DrawString("Lot/Row", font, XBrushes.Black, _
            New XRect(x1 + (38.25 * 7.2), y1, x2 + (38.75 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(space.Space_ID.Substring(7, 3), font, XBrushes.Black, _
            New XRect(x2 + (38.75 * 7.2), y1, x2 + (38.75 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (38.75 * 7.2), y2), New XPoint(page.Width.Point - 75.6 - (12.75 * 7.2), y2))
            gfx.DrawString("Space", font, XBrushes.Black, _
            New XRect(x1 + (52 * 7.2), y1, x2 + (51.5 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(space.Space_ID.Substring(11, 3), font, XBrushes.Black, _
            New XRect(x2 + (51.5 * 7.2), y1, x2 + (51.5 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (51.5 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (15.75 * 7.2)
            gfx.DrawString("Date & Day of Death", font, XBrushes.Black, _
            New XRect(x1, y1, 61.5, 28), XStringFormats.TopLeft)
            gfx.DrawString(If(burial.Date_of_Death Is Nothing, "00/00/0000", burial.Date_of_Death.Value.ToString("ddd") + ". - " + burial.Date_of_Death.Value.ToShortDateString()), font, XBrushes.Black, _
            New XRect(x2, y1, 45, 28), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (32.25 * 7.2), y2))
            gfx.DrawString("Age", font, XBrushes.Black, _
            New XRect(x1 + (32.25 * 7.2), y1, x2 + (20 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(burial.age(), font, XBrushes.Black, _
            New XRect(x2 + (20 * 7.2), y1, x2 + (20 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (20 * 7.2), y2), New XPoint(page.Width.Point - 75.6 - (22.25 * 7.2), y2))
            gfx.DrawString("Gender", font, XBrushes.Black, _
            New XRect(x1 + (42.25 * 7.2), y1, x2 + (33 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Gender), "", burial.Gender), font, XBrushes.Black, _
            New XRect(x2 + (33 * 7.2), y1, x2 + (33 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (33 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (12 * 7.2)
            Dim rect As New XRect(x1, y1, 75, 14)
            gfx.DrawString("Place of Death", font, XBrushes.Black, _
            rect, XStringFormats.TopLeft)
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Death_City_State), "", burial.Death_City_State), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (9.5 * 7.2)
            gfx.DrawString("Date of Birth", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(If(burial.Date_of_Birth Is Nothing, "00/00/0000", burial.Date_of_Birth.Value.ToShortDateString), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (37 * 7.2), y2))
            tf.Alignment = XParagraphAlignment.Left
            rect = New XRect(x1 + (28 * 7.2), y1, 75, 28)
            tf.DrawString("Place of Birth", font, XBrushes.Black, _
            rect, XStringFormats.TopLeft)
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Birth_City_State), "", burial.Birth_City_State), font, XBrushes.Black, _
            New XRect(x2 + (29 * 7.2), y1, x2 + (29 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (29 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (8.25 * 7.2)
            gfx.DrawString("Comments", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Other_Information), "", burial.Other_Information), font, XBrushes.Black, _
            New XRect(x2, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))
            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))
            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (13 * 7.2)
            gfx.DrawString("Fees (001-4462)", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString(If(burial.Fees.HasValue, burial.Fees.Value.ToString("$#,##0.00"), "$0.00"), font, XBrushes.Black, _
            New XRect(x2, y1, (page.Width.Point - 75.6 - (28.75 * 7.2)) - x2, y2 - y1))
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (28.75 * 7.2), y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (25 * 7.2)
            gfx.DrawString("Pre-Need Payment (135-4464)", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString(If(burial.Pre_Need_Payment.HasValue, burial.Pre_Need_Payment.Value.ToString("$#,##0.00"), "$0.00"), font, XBrushes.Black, _
            New XRect(x2, y1, (page.Width.Point - 75.6 - (28.75 * 7.2)) - x2, y2 - y1))
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (28.75 * 7.2), y2))
            gfx.DrawString("Receipt #", font, XBrushes.Black, _
            New XRect(x1 + (40 * 7.2), y1, x2 + (23 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Receipt_Number), "", burial.Receipt_Number), font, XBrushes.Black, _
            New XRect(x2 + (23 * 7.2), y1, x2 + (23 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (23 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))


            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (25 * 7.2)
            gfx.DrawString("Space Purchase (001-4461 (2/3))", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString(If(burial.Space_Purchase_1.HasValue, burial.Space_Purchase_1.Value.ToString("$#,##0.00"), "$0.00"), font, XBrushes.Black, _
            New XRect(x2, y1, (page.Width.Point - 75.6 - (28.75 * 7.2)) - x2, y2 - y1))
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (28.75 * 7.2), y2))
            tf.Alignment = XParagraphAlignment.Left
            tf.DrawString("Work Order Date", font, XBrushes.Black, _
            New XRect(x1 + (40 * 7.2), y1 - 13, 60, 28), XStringFormats.TopLeft)
            tf.DrawString(If(burial.Work_Order_Date Is Nothing, "00/00/0000", burial.Work_Order_Date.Value.ToShortDateString), font, XBrushes.Black, _
            New XRect(x2 + (24 * 7.2), y1, 100, 14), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (24 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (25 * 7.2)
            gfx.DrawString("(135-4461 (1/3))", font, XBrushes.Black, _
            New XRect(x1 + (12.5 * 7.2), y1, x2, y2), XStringFormats.TopLeft)
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString(If(burial.Space_Purchase_2.HasValue, burial.Space_Purchase_2.Value.ToString("$#,##0.00"), "$0.00"), font, XBrushes.Black, _
            New XRect(x2, y1, (page.Width.Point - 75.6 - (28.75 * 7.2)) - x2, y2 - y1))
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (28.75 * 7.2), y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (5 * 7.2)
            gfx.DrawString("Total", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (28.75 * 7.2), y2))
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString(burial.total().ToString("$#,##0.00"), font, XBrushes.Black, _
            New XRect(x2, y1, (page.Width.Point - 75.6 - (28.75 * 7.2)) - x2, y2 - y1))
            gfx.DrawString("Signed", font, XBrushes.Black, _
            New XRect(x1 + (40 * 7.2), y1, x2 + (42 * 7.2), y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2 + (42 * 7.2), y2), New XPoint(page.Width.Point - 75.6, y2))

            y1 += verticleSpace
            y2 += verticleSpace
            x2 = x1 + (10 * 7.2)
            gfx.DrawString("Amount Paid", font, XBrushes.Black, _
            New XRect(x1, y1, x2, y2), XStringFormats.TopLeft)
            gfx.DrawLine(pen, New XPoint(x2, y2), New XPoint(page.Width.Point - 75.6 - (28.75 * 7.2), y2))
            tf.Alignment = XParagraphAlignment.Right
            tf.DrawString(If(burial.Amount_Paid.HasValue, burial.Amount_Paid.Value.ToString("$#,##0.00"), "$0.00"), font, XBrushes.Black, _
            New XRect(x2, y1, (page.Width.Point - 75.6 - (28.75 * 7.2)) - x2, y2 - y1))
            gfx.DrawString(If(String.IsNullOrEmpty(burial.Permit_Number), "", burial.Permit_Number), TitleFontBold, XBrushes.Black, _
            New XRect(x1 + (40 * 7.2), y1, 200, 28), XStringFormats.TopCenter)

            document.Save(stream, False)
            'stream.Flush()
            'stream.Position = 0
            Return File(stream, "application/pdf")
        End Function

        Function burialReport(Optional cemetery As String = "0", Optional startDate As Date = Nothing, Optional endDate As Date = Nothing) As ActionResult
            Dim stream As New MemoryStream

            ' Create a new PDF document
            Dim document As PdfDocument = New PdfDocument
            document.Info.Title = "Burial Record"

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
            Dim col2 As Double = 33.5 * 7.2
            Dim col3 As Double = 45 * 7.2
            Dim col4 As Double = 54 * 7.2
            Dim col5 As Double = 61 * 7.2
            Dim col6 As Double = 68 * 7.2
            Dim col7 As Double = 77.5 * 7.2

            Dim burialRecords As List(Of Burial)
            If cemetery = 0 Then
                burialRecords = db.Burials.Where(Function(w) Not w.Last_Name = "" And w.Date_of_Death > startDate And w.Date_of_Death < endDate).OrderBy(Function(o) o.Last_Name).ThenBy(Function(o) o.First_Name).ToList
            Else
                burialRecords = db.Burials.Where(Function(w) w.Space_ID.Substring(0, 1) = cemetery And Not w.Last_Name = "" And ((w.Date_of_Death > startDate And w.Date_of_Death < endDate) Or w.Date_of_Death Is Nothing)).OrderBy(Function(o) o.Last_Name).ThenBy(Function(o) o.First_Name).ToList
            End If
            Dim records_per_page As Integer = 46
            Dim totalPages As Integer = Math.Ceiling(burialRecords.Count / (records_per_page))
            Dim x As Integer = 0
            Dim y1 As Double = 10 * 7.2
            Dim y2 As Double = 11.25 * 7.2
            Dim verticleSpacing As Double = 2.0 * 7.2
            Dim firstVertileSpacing As Double = 2.25 * 7.2
            Dim currentPage As Integer = 0
            For Each b In burialRecords
                Dim page As PdfPage
                If x = 0 Or (x Mod records_per_page = 0) Then
                    y1 = 10 * 7.2
                    y2 = 11.25 * 7.2
                    page = document.AddPage
                    gfx = XGraphics.FromPdfPage(page)
                    tf = New XTextFormatter(gfx)
                    tf.Alignment = XParagraphAlignment.Left
                    tf.DrawString("Deceased", TitleFont, XBrushes.Navy, _
                                    New XRect(col1, y1, col2 - col1, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Division", TitleFont, XBrushes.Navy, _
                                    New XRect(col2, y1, col3 - col2, y2 - y1), XStringFormats.TopLeft)
                    tf.Alignment = XParagraphAlignment.Center
                    tf.DrawString("Block/Section", TitleFont, XBrushes.Navy, _
                                    New XRect(col3, y1, col4 - col3, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Lot/Row", TitleFont, XBrushes.Navy, _
                                    New XRect(col4, y1, col5 - col4, y2 - y1), XStringFormats.TopLeft)
                    tf.DrawString("Space", TitleFont, XBrushes.Navy, _
                                    New XRect(col5, y1, col6 - col5, y2 - y1), XStringFormats.TopLeft)
                    tf.Alignment = XParagraphAlignment.Right
                    tf.DrawString("Date of Death", TitleFont, XBrushes.Navy, _
                                    New XRect(col6, y1, col7 - col6, y2 - y1), XStringFormats.TopLeft)
                    tf.Alignment = XParagraphAlignment.Left
                    gfx.DrawLine(New XPen(XColors.Navy, 2), New XPoint(col1, y2 + 5), New XPoint(col7, y2 + 5))
                    gfx.DrawLine(New XPen(XColors.Gray, 2), New XPoint(col1, page.Height.Point - 5.75 * 7.2), New XPoint(col7, page.Height.Point - 5.75 * 7.2))
                    tf.DrawString(Date.Now().ToLongDateString, TitleFont, XBrushes.Navy, _
                                   New XRect(col1, page.Height.Point - 5.5 * 7.2, col7 - col1, y2 - y1), XStringFormats.TopLeft)
                    currentPage += 1
                    tf.Alignment = XParagraphAlignment.Right
                    tf.DrawString("Page " + currentPage.ToString + " of " + totalPages.ToString, TitleFont, XBrushes.Navy, _
                                   New XRect(col1, page.Height.Point - 5.5 * 7.2, col7 - col1, y2 - y1), XStringFormats.TopLeft)
                    tf.Alignment = XParagraphAlignment.Center
                    If cemetery = 0 Then
                        tf.DrawString("Complete Cemetery Burial Report", TitleFont, XBrushes.Navy, _
                                   New XRect(col1, page.Height.Point - 5.5 * 7.2, col7 - col1, y2 - y1), XStringFormats.TopLeft)
                    Else
                        tf.DrawString(b.cemetery + " Cemetery Burial Report", TitleFont, XBrushes.Navy, _
                                       New XRect(col1, page.Height.Point - 5.5 * 7.2, col7 - col1, y2 - y1), XStringFormats.TopLeft)
                    End If
                    tf.Alignment = XParagraphAlignment.Left
                    If currentPage = 1 Then
                        tf.Alignment = XParagraphAlignment.Left
                        If cemetery = 0 Then
                            tf.DrawString("Complete Cemetery Burial Report " + startDate.ToShortDateString + " - " + endDate.ToShortDateString, TitleFontBold, XBrushes.Navy, _
                                       New XRect(col1, 6 * 7.2, col7 - col1, 3 * 7.2), XStringFormats.TopLeft)
                        Else
                            tf.DrawString(b.cemetery + " Cemetery Burial Report " + startDate.ToShortDateString + " - " + endDate.ToShortDateString, TitleFontBold, XBrushes.Navy, _
                                           New XRect(col1, 6 * 7.2, col7 - col1, 3 * 7.2), XStringFormats.TopLeft)
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
                        tf.DrawString(b.Last_Name + ", " + b.First_Name + " - " + b.cemetery, font, XBrushes.Black, _
                                                            New XRect(col1, y1, col2 - col1, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(b.division(), font, XBrushes.Black, _
                                        New XRect(col2, y1, col3 - col2, y2 - y1), XStringFormats.TopLeft)
                        tf.Alignment = XParagraphAlignment.Center
                        tf.DrawString(b.Space_ID.Substring(5, 1), font, XBrushes.Black, _
                                        New XRect(col3, y1, col4 - col3, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(b.Space_ID.Substring(7, 3), font, XBrushes.Black, _
                                        New XRect(col4, y1, col5 - col4, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(b.Space_ID.Substring(11, 3), font, XBrushes.Black, _
                                        New XRect(col5, y1, col6 - col5, y2 - y1), XStringFormats.TopLeft)
                        tf.Alignment = XParagraphAlignment.Right
                        tf.DrawString(If(b.Date_of_Death.HasValue, b.Date_of_Death.Value.ToShortDateString, ""), font, XBrushes.Black, _
                                        New XRect(col6, y1, col7 - col6, y2 - y1), XStringFormats.TopLeft)
                    Else
                        tf.Alignment = XParagraphAlignment.Left
                        tf.DrawString(b.Last_Name + ", " + b.First_Name, font, XBrushes.Black, _
                                                            New XRect(col1, y1, col2 - col1, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(b.division(), font, XBrushes.Black, _
                                        New XRect(col2, y1, col3 - col2, y2 - y1), XStringFormats.TopLeft)
                        tf.Alignment = XParagraphAlignment.Center
                        tf.DrawString(b.Space_ID.Substring(5, 1), font, XBrushes.Black, _
                                        New XRect(col3, y1, col4 - col3, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(b.Space_ID.Substring(7, 3), font, XBrushes.Black, _
                                        New XRect(col4, y1, col5 - col4, y2 - y1), XStringFormats.TopLeft)
                        tf.DrawString(b.Space_ID.Substring(11, 3), font, XBrushes.Black, _
                                        New XRect(col5, y1, col6 - col5, y2 - y1), XStringFormats.TopLeft)
                        tf.Alignment = XParagraphAlignment.Right
                        tf.DrawString(If(b.Date_of_Death.HasValue, b.Date_of_Death.Value.ToShortDateString, ""), font, XBrushes.Black, _
                                        New XRect(col6, y1, col7 - col6, y2 - y1), XStringFormats.TopLeft)
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

        '
        ' GET: /Burial/Create
        <Authorize(Roles:="Cem_Admins, Cem_Burial_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Create(ByVal space_id As String, ByVal owner_id As Integer) As ActionResult
            Dim burial As New Burial
            burial.Space_ID = space_id
            burial.OwnerID = owner_id
            burial.Owner = db.Owners.Find(owner_id)
            Return View(burial)
        End Function

        '
        ' POST: /Burial/Create
        <Authorize(Roles:="Cem_Admins, Cem_Burial_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        Function Create(ByVal burial As Burial, ByVal file As HttpPostedFileBase) As ActionResult
            If ModelState.IsValid Then
                Dim nextPermitNo As Integer
                If db.Burials.OrderByDescending(Function(o) o.ID).Count > 0 Then
                    nextPermitNo = db.Burials.OrderByDescending(Function(o) o.ID).First().ID + 1
                Else
                    nextPermitNo = 1
                End If
                burial.Permit_Number = "B" + Format(nextPermitNo, "000000")
                'Dim file = Request.Files(0)

                If (Not file Is Nothing) Then
                    Dim fileName = burial.Permit_Number + Path.GetExtension(file.FileName)
                    Dim path1 = Path.Combine(Server.MapPath("~/Headstone_Photos/"), fileName)
                    file.SaveAs(path1)
                    burial.Photo = "/wincem/Headstone_Photos/" + fileName
                End If
                Dim request As WebRequest = WebRequest.Create(My.Settings.spaces + "?where=SPACE_ID='" + burial.Space_ID + "'&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=*&returnGeometry=true&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&f=pjson")
                Dim ws As WebResponse = request.GetResponse()
                Dim jsonSerializer As Json.DataContractJsonSerializer = New Json.DataContractJsonSerializer(GetType(GIS_Spaces))
                Dim spaces As GIS_Spaces = jsonSerializer.ReadObject(ws.GetResponseStream())
                Dim space As Feature = spaces.features(0)

                If (burial.Burial_Position = "Cremation1" Or burial.Burial_Position = "Cremation2" Or burial.Burial_Position = "Cremation3") Then
                    WebRequestinWebForm(My.Settings.burials + "/addFeatures", "features=[" + drawCremation(burial.Burial_Position, burial.Space_ID, space, nextPermitNo) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                ElseIf (burial.Burial_Position = "Full_Burial" Or burial.Burial_Position = "Full_Burial_2_Spaces_EW" Or burial.Burial_Position = "Full_Burial_2_Spaces_NS") Then
                    WebRequestinWebForm(My.Settings.burials + "/addFeatures", "features=[" + drawCasket(burial.Burial_Position, burial.Space_ID, space, nextPermitNo) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                ElseIf (burial.Burial_Position = "Infant1" Or burial.Burial_Position = "Infant2" Or burial.Burial_Position = "Infant3" Or burial.Burial_Position = "Infant4") Then
                    WebRequestinWebForm(My.Settings.burials + "/addFeatures", "features=[" + drawInfantCasket(burial.Burial_Position, burial.Space_ID, space, nextPermitNo) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                ElseIf (burial.Burial_Position = "MemorialHeadstone") Then
                    WebRequestinWebForm(My.Settings.burials + "/addFeatures", "features=[" + drawMemorialHeadstone(burial.Burial_Position, burial.Space_ID, space, nextPermitNo) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                End If

                Dim burialCreate_ws As WebResponse = request.GetResponse()
                burialCreate_ws.GetResponseStream()
                db.Burials.Add(burial)
                db.SaveChanges()
                If burial.Work_Order_Date.HasValue Then
                    If burial.Work_Order_Date.Value >= Now.AddDays(-30) And burial.Work_Order_Date.Value <= Now.AddDays(30) Then
                        For Each person In db.Notification_List.ToList()
                            burial.Email(person.email_address, db.Owners.Find(burial.OwnerID))
                        Next
                    End If
                End If

                Return RedirectToAction("Edit", New With {.id = burial.ID})
            End If


            ViewBag.OwnerID = New SelectList(db.Owners, "ID", "Deed_No", burial.OwnerID)
            Return View(burial)
        End Function

        '
        ' GET: /Burial/Edit/5
        <Authorize(Roles:="Cem_Admins, Cem_Burial_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim burial As Burial = db.Burials.Find(id)
            If IsNothing(burial) Then
                Return HttpNotFound()
            End If
            ViewBag.OwnerID = New SelectList(db.Owners, "ID", "Deed_No", burial.OwnerID)
            Return View(burial)
        End Function

        '
        ' POST: /Burial/Edit/5
        <Authorize(Roles:="Cem_Admins, Cem_Burial_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        Function Edit(ByVal burial As Burial, ByVal File As HttpPostedFileBase) As ActionResult
            If ModelState.IsValid Then

                If (Not File Is Nothing) Then
                    Dim fileName = burial.Permit_Number + Path.GetExtension(File.FileName)
                    Dim path1 = Path.Combine(Server.MapPath("~/Headstone_Photos/"), fileName)
                    File.SaveAs(path1)
                    burial.Photo = "/wincem/Headstone_Photos/" + fileName
                End If

                Dim gburial As GIS_Burials.Feature = burial.gis_burial
                Dim space As Feature = burial.gis_space

                If (burial.Burial_Position = "Cremation1" Or burial.Burial_Position = "Cremation2" Or burial.Burial_Position = "Cremation3") Then
                    WebRequestinWebForm(If(gburial.attributes Is Nothing, My.Settings.burials + "/addFeatures", My.Settings.burials + "/updateFeatures"), "features=[" + drawCremation(burial.Burial_Position, burial.Space_ID, space, burial.ID, If(gburial.attributes Is Nothing, 0, gburial.attributes.OBJECTID)) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                ElseIf (burial.Burial_Position = "Full_Burial" Or burial.Burial_Position = "Full_Burial_2_Spaces_EW" Or burial.Burial_Position = "Full_Burial_2_Spaces_NS") Then
                    WebRequestinWebForm(If(gburial.attributes Is Nothing, My.Settings.burials + "/addFeatures", My.Settings.burials + "/updateFeatures"), "features=[" + drawCasket(burial.Burial_Position, burial.Space_ID, space, burial.ID, If(gburial.attributes Is Nothing, 0, gburial.attributes.OBJECTID)) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                ElseIf (burial.Burial_Position = "Infant1" Or burial.Burial_Position = "Infant2" Or burial.Burial_Position = "Infant3" Or burial.Burial_Position = "Infant4") Then
                    WebRequestinWebForm(If(gburial.attributes Is Nothing, My.Settings.burials + "/addFeatures", My.Settings.burials + "/updateFeatures"), "features=[" + drawInfantCasket(burial.Burial_Position, burial.Space_ID, space, burial.ID, If(gburial.attributes Is Nothing, 0, gburial.attributes.OBJECTID)) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                ElseIf (burial.Burial_Position = "MemorialHeadstone") Then
                    WebRequestinWebForm(If(gburial.attributes Is Nothing, My.Settings.burials + "/addFeatures", My.Settings.burials + "/updateFeatures"), "features=[" + drawMemorialHeadstone(burial.Burial_Position, burial.Space_ID, space, burial.ID, If(gburial.attributes Is Nothing, 0, gburial.attributes.OBJECTID)) + "]&gdbVersion=Default&rollbackOnFailure=true&f=pjson")
                End If
                db.Entry(burial).State = EntityState.Modified
                db.SaveChanges()
                If burial.Work_Order_Date.HasValue Then
                    If burial.Work_Order_Date.Value >= Now.AddDays(-30) And burial.Work_Order_Date.Value <= Now.AddDays(30) Then
                        For Each person In db.Notification_List.ToList()
                            burial.Email(person.email_address, db.Owners.Find(burial.OwnerID))
                        Next
                    End If
                End If
                Return RedirectToAction("Details", New With {.id = burial.ID})
            End If

            ViewBag.OwnerID = New SelectList(db.Owners, "ID", "Deed_No", burial.OwnerID)
            Return View(burial)
        End Function

        '
        ' GET: /Burial/Delete/5
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim burial As Burial = db.Burials.Find(id)
            If IsNothing(burial) Then
                Return HttpNotFound()
            End If
            Return View(burial)
        End Function

        '
        ' POST: /Burial/Delete/5
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        <ActionName("Delete")> _
        Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
            Dim burial As Burial = db.Burials.Find(id)
            db.Burials.Remove(burial)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

        Sub createPDF()
            Dim stream As New MemoryStream

            ' Create a new PDF document
            Dim document As PdfDocument = New PdfDocument
            document.Info.Title = "Burial Record"

            ' Create an empty page
            Dim page As PdfPage = document.AddPage

            ' Get an XGraphics object for drawing
            Dim gfx As XGraphics = XGraphics.FromPdfPage(page)


            Dim pen As XPen = New XPen(XColor.FromArgb(255, 255, 255))
            gfx.DrawLine(pen, New XPoint(75.6, 100.8), New XPoint(page.Width.Point - 75.6, 100.8))
            gfx.DrawLine(pen, New XPoint(75.6, 129.6), New XPoint(page.Width.Point - 75.6, 129.6))

            ' Draw an ellipse
            gfx.DrawEllipse(pen, 3 * page.Width.Point / 10, 3 * page.Height.Point / 10, 2 * page.Width.Point / 5, 2 * page.Height.Point / 5)

            ' Create a font
            Dim font As XFont = New XFont("Verdana", 20, XFontStyle.Bold)

            ' Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black, _
            New XRect(0, 0, page.Width.Point, page.Height.Point), XStringFormats.Center)


            document.Save(stream, False)

        End Sub

        Function drawCasket(position As String, space_ID As String, sp As Feature, burial_id As Integer, Optional object_ID As Integer = 0) As String
            Dim centroid_x As Double = 0.0
            Dim centroid_y As Double = 0.0
            getCentroid(sp, centroid_x, centroid_y)
            Dim width1 As Double = 8
            Dim width2 As Double = 4
            Dim height1 As Double = 1.5
            Dim height2 As Double = 3
            Dim oposite As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(0) - sp.geometry.rings.Item(0).Item(1).Item(0))
            Dim adjacent As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(1) - sp.geometry.rings.Item(0).Item(1).Item(1))
            Dim angle As Double = 0
            If (oposite > adjacent) Then
                angle = Math.Atan(-adjacent / oposite)
            Else
                angle = Math.Atan(-oposite / adjacent)
            End If
            If (position = "Full_Burial_2_Spaces_NS") Then
                centroid_y = centroid_y + (((2) * Math.Cos(angle)) + ((0) * Math.Sin(angle)))
            End If
            If (position = "Full_Burial_2_Spaces_EW") Then
                centroid_x = centroid_x + (((5) * Math.Cos(angle)) + ((0) * Math.Sin(angle)))
            End If

            '*************************************************
            '**********p2*************************************
            '****p1******************p3***********************
            '****p0******************p4***********************
            '**********p5*************************************
            '*************************************************

            Dim p0_x As Double = centroid_x - (((width1 / 2) * Math.Cos(angle)) + ((height1 / 2) * Math.Sin(angle)))
            Dim p0_y As Double = centroid_y - (((height1 / 2) * Math.Cos(angle)) - ((width1 / 2) * Math.Sin(angle)))

            Dim p1_x As Double = centroid_x - (((width1 / 2) * Math.Cos(angle)) - ((height1 / 2) * Math.Sin(angle)))
            Dim p1_y As Double = centroid_y + (((height1 / 2) * Math.Cos(angle)) + ((width1 / 2) * Math.Sin(angle)))

            Dim p2_x As Double = centroid_x - (((width2 / 2) * Math.Cos(angle)) - ((height2 / 2) * Math.Sin(angle)))
            Dim p2_y As Double = centroid_y + (((height2 / 2) * Math.Cos(angle)) + ((width2 / 2) * Math.Sin(angle)))

            Dim p3_x As Double = centroid_x + (((width1 / 2) * Math.Cos(angle)) + ((height1 / 2) * Math.Sin(angle)))
            Dim p3_y As Double = centroid_y + (((height1 / 2) * Math.Cos(angle)) - ((width1 / 2) * Math.Sin(angle)))

            Dim p4_x As Double = centroid_x + (((width1 / 2) * Math.Cos(angle)) - ((height1 / 2) * Math.Sin(angle)))
            Dim p4_y As Double = centroid_y - (((height1 / 2) * Math.Cos(angle)) + ((width1 / 2) * Math.Sin(angle)))

            Dim p5_x As Double = centroid_x - (((width2 / 2) * Math.Cos(angle)) + ((height2 / 2) * Math.Sin(angle)))
            Dim p5_y As Double = centroid_y - (((height2 / 2) * Math.Cos(angle)) - ((width2 / 2) * Math.Sin(angle)))

            Dim casket As String = "{" _
                        + """geometry"": {" _
                        + """rings"": [[[" + p0_x.ToString + ", " + p0_y.ToString + "], [" + p1_x.ToString + ", " + p1_y.ToString + "]," _
                        + "[" + p2_x.ToString + ", " + p2_y.ToString + "], [" + p3_x.ToString + ", " + p3_y.ToString + "], [" + p4_x.ToString + ", " + p4_y.ToString + "]," _
                        + "[" + p5_x.ToString + ", " + p5_y.ToString + "], [" _
                        + p0_x.ToString + ", " + p0_y.ToString + "]]]" _
                        + "}," _
                        + """attributes"": {" _
                        + If(object_ID <> 0, """OBJECTID"": " + object_ID.ToString + ",", "") _
                        + """Burial_ID"": " + burial_id.ToString + "," _
                        + """Space_ID"": " + space_ID _
                        + "}" _
                        + "}"

            Return casket

        End Function

        Function drawInfantCasket(position As String, space_ID As String, sp As Feature, burial_id As Integer, Optional object_ID As Integer = 0) As String
            Dim centroid_x As Double = 0.0
            Dim centroid_y As Double = 0.0
            getCentroid(sp, centroid_x, centroid_y)
            Dim width1 As Double = 4
            Dim width2 As Double = 2
            Dim height1 As Double = 0.75
            Dim height2 As Double = 1.5
            Dim oposite As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(0) - sp.geometry.rings.Item(0).Item(1).Item(0))
            Dim adjacent As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(1) - sp.geometry.rings.Item(0).Item(1).Item(1))
            Dim angle As Double = 0
            If (oposite > adjacent) Then
                angle = Math.Atan(-adjacent / oposite)

            Else
                angle = Math.Atan(-oposite / adjacent)
            End If

            If (position = "Infant2") Then
                centroid_x = centroid_x - (((2.5) * Math.Cos(angle)) - ((0) * Math.Sin(angle)))
                centroid_y = centroid_y + (((0) * Math.Cos(angle)) + ((2.5) * Math.Sin(angle)))
            ElseIf (position = "Infant3") Then
                centroid_x = centroid_x + (((2.5) * Math.Cos(angle)) - ((0) * Math.Sin(angle)))
                centroid_y = centroid_y - (((0) * Math.Cos(angle)) + ((2.5) * Math.Sin(angle)))
            ElseIf (position = "Infant4") Then
                centroid_x = centroid_x + (((-4) * Math.Cos(angle)) - ((0) * Math.Sin(angle)))
                centroid_y = centroid_y - (((0) * Math.Cos(angle)) + ((-4) * Math.Sin(angle)))
                angle = angle + (90 * (Math.PI / 180))
            End If

            '*************************************************
            '**********p2*************************************
            '****p1******************p3***********************
            '****p0******************p4***********************
            '**********p5*************************************
            '*************************************************

            Dim p0_x As Double = centroid_x - (((width1 / 2) * Math.Cos(angle)) + ((height1 / 2) * Math.Sin(angle)))
            Dim p0_y As Double = centroid_y - (((height1 / 2) * Math.Cos(angle)) - ((width1 / 2) * Math.Sin(angle)))

            Dim p1_x As Double = centroid_x - (((width1 / 2) * Math.Cos(angle)) - ((height1 / 2) * Math.Sin(angle)))
            Dim p1_y As Double = centroid_y + (((height1 / 2) * Math.Cos(angle)) + ((width1 / 2) * Math.Sin(angle)))

            Dim p2_x As Double = centroid_x - (((width2 / 2) * Math.Cos(angle)) - ((height2 / 2) * Math.Sin(angle)))
            Dim p2_y As Double = centroid_y + (((height2 / 2) * Math.Cos(angle)) + ((width2 / 2) * Math.Sin(angle)))

            Dim p3_x As Double = centroid_x + (((width1 / 2) * Math.Cos(angle)) + ((height1 / 2) * Math.Sin(angle)))
            Dim p3_y As Double = centroid_y + (((height1 / 2) * Math.Cos(angle)) - ((width1 / 2) * Math.Sin(angle)))

            Dim p4_x As Double = centroid_x + (((width1 / 2) * Math.Cos(angle)) - ((height1 / 2) * Math.Sin(angle)))
            Dim p4_y As Double = centroid_y - (((height1 / 2) * Math.Cos(angle)) + ((width1 / 2) * Math.Sin(angle)))

            Dim p5_x As Double = centroid_x - (((width2 / 2) * Math.Cos(angle)) + ((height2 / 2) * Math.Sin(angle)))
            Dim p5_y As Double = centroid_y - (((height2 / 2) * Math.Cos(angle)) - ((width2 / 2) * Math.Sin(angle)))

            Dim casket As String = "{" _
                        + """geometry"": {" _
                        + """rings"": [[[" + p0_x.ToString + ", " + p0_y.ToString + "], [" + p1_x.ToString + ", " + p1_y.ToString + "]," _
                        + "[" + p2_x.ToString + ", " + p2_y.ToString + "], [" + p3_x.ToString + ", " + p3_y.ToString + "], [" + p4_x.ToString + ", " + p4_y.ToString + "]," _
                        + "[" + p5_x.ToString + ", " + p5_y.ToString + "], [" _
                        + p0_x.ToString + ", " + p0_y.ToString + "]]]" _
                        + "}," _
                        + """attributes"": {" _
                        + If(object_ID <> 0, """OBJECTID"": " + object_ID.ToString + ",", "") _
                        + """Burial_ID"": " + burial_id.ToString + "," _
                        + """Space_ID"": " + space_ID _
                        + "}" _
                        + "}"

            Return casket

        End Function

        Function drawMemorialHeadstone(position As String, space_ID As String, sp As Feature, burial_id As Integer, Optional object_ID As Integer = 0) As String
            Dim bottom_left_x As Double = 0.0
            Dim bottom_left_y As Double = 0.0
            getbottomLeft(sp, bottom_left_x, bottom_left_y)
            bottom_left_x = bottom_left_x + 0.75
            bottom_left_y = bottom_left_y + 0.75
            Dim Ang36 As Double = Math.PI / 5.0
            Dim Ang72 As Double = 2.0 * Ang36
            Dim Sin36 As Double = Math.Sin(Ang36)
            Dim Sin72 As Double = Math.Sin(Ang72)
            Dim Cos36 As Double = Math.Cos(Ang36)
            Dim Cos72 As Double = Math.Cos(Ang72)

            Dim innerradius As Double = 0.3
            Dim outerradius As Double = 0.75

            Dim oposite As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(0) - sp.geometry.rings.Item(0).Item(1).Item(0))
            Dim adjacent As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(1) - sp.geometry.rings.Item(0).Item(1).Item(1))
            Dim angle As Double = 0
            If (oposite > adjacent) Then
                angle = Math.Atan(-adjacent / oposite)

            Else
                angle = Math.Atan(-oposite / adjacent)
            End If


            '*************************************************
            '**********p2*************************************
            '****p1******************p3***********************
            '****p0******************p4***********************
            '**********p5*************************************
            '*************************************************

            Dim p0_x As Double = bottom_left_x
            Dim p0_y As Double = bottom_left_y + outerradius

            Dim p1_x As Double = bottom_left_x + innerradius * Sin36
            Dim p1_y As Double = bottom_left_y + innerradius * Cos36

            Dim p2_x As Double = bottom_left_x + outerradius * Sin72
            Dim p2_y As Double = bottom_left_y + outerradius * Cos72

            Dim p3_x As Double = bottom_left_x + innerradius * Sin72
            Dim p3_y As Double = bottom_left_y - innerradius * Cos72

            Dim p4_x As Double = bottom_left_x + outerradius * Sin36
            Dim p4_y As Double = bottom_left_y - outerradius * Cos36

            Dim p5_x As Double = bottom_left_x
            Dim p5_y As Double = bottom_left_y - innerradius

            Dim p6_x As Double = bottom_left_x + (bottom_left_x - p4_x)
            Dim p6_y As Double = p4_y

            Dim p7_x As Double = bottom_left_x + (bottom_left_x - p3_x)
            Dim p7_y As Double = p3_y

            Dim p8_x As Double = bottom_left_x + (bottom_left_x - p2_x)
            Dim p8_y As Double = p2_y

            Dim p9_x As Double = bottom_left_x + (bottom_left_x - p1_x)
            Dim p9_y As Double = p1_y

            Dim casket As String = "{" _
                        + """geometry"": {" _
                        + """rings"": [[[" + p0_x.ToString + ", " + p0_y.ToString + "], [" + p1_x.ToString + ", " + p1_y.ToString + "]," _
                        + "[" + p2_x.ToString + ", " + p2_y.ToString + "], [" + p3_x.ToString + ", " + p3_y.ToString + "], [" + p4_x.ToString + ", " + p4_y.ToString + "]," _
                        + "[" + p5_x.ToString + ", " + p5_y.ToString + "], [" + p6_x.ToString + ", " + p6_y.ToString + "], [" + p7_x.ToString + ", " + p7_y.ToString + "], [" _
                         + p8_x.ToString + ", " + p8_y.ToString + "], [" + p9_x.ToString + ", " + p9_y.ToString + "], [" _
                        + p0_x.ToString + ", " + p0_y.ToString + "]]]" _
                        + "}," _
                        + """attributes"": {" _
                        + If(object_ID <> 0, """OBJECTID"": " + object_ID.ToString + ",", "") _
                        + """Burial_ID"": " + burial_id.ToString + "," _
                        + """Space_ID"": " + space_ID _
                        + "}" _
                        + "}"

            Return casket

        End Function

        Public Function drawCremation(position As String, space_ID As String, sp As Feature, burial_id As Integer, Optional object_ID As Integer = 0) As String
            Dim centroid_x As Double = 0.0
            Dim centroid_y As Double = 0.0
            getCentroid(sp, centroid_x, centroid_y)
            Dim width1 As Double = 1.75
            Dim width2 As Double = 1
            Dim height1 As Double = 1
            Dim height2 As Double = 1.75
            Dim oposite As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(0) - sp.geometry.rings.Item(0).Item(1).Item(0))
            Dim adjacent As Double = Math.Abs(sp.geometry.rings.Item(0).Item(0).Item(1) - sp.geometry.rings.Item(0).Item(1).Item(1))
            Dim angle As Double = 0
            If (oposite > adjacent) Then
                angle = Math.Atan(-adjacent / oposite)
            Else
                angle = Math.Atan(-oposite / adjacent)
            End If
            If (position = "Cremation1") Then
                centroid_x = centroid_x + (((-4.0) * Math.Cos(angle)) - ((0) * Math.Sin(angle)))
                centroid_y = centroid_y - (((0) * Math.Cos(angle)) + ((-4.0) * Math.Sin(angle)))
            ElseIf (position = "Cremation2") Then
                centroid_x = centroid_x + (((-2.0) * Math.Cos(angle)) - ((0) * Math.Sin(angle)))
                centroid_y = centroid_y - (((0) * Math.Cos(angle)) + ((-2.0) * Math.Sin(angle)))
            ElseIf (position = "Cremation3") Then
                centroid_x = centroid_x + (((-0.0) * Math.Cos(angle)) - ((0) * Math.Sin(angle)))
                centroid_y = centroid_y - (((0) * Math.Cos(angle)) + ((-0.0) * Math.Sin(angle)))
            End If

            '************************************************
            '********p2*p3***********************************
            '****p1*********p4*******************************
            '****p0*********p5*******************************
            '********p7*p6***********************************
            '************************************************

            Dim p0_x As Double = centroid_x - (((width1 / 2) * Math.Cos(angle)) + ((height1 / 2) * Math.Sin(angle)))
            Dim p0_y As Double = centroid_y - (((height1 / 2) * Math.Cos(angle)) - ((width1 / 2) * Math.Sin(angle)))

            Dim p1_x As Double = centroid_x - (((width1 / 2) * Math.Cos(angle)) - ((height1 / 2) * Math.Sin(angle)))
            Dim p1_y As Double = centroid_y + (((height1 / 2) * Math.Cos(angle)) + ((width1 / 2) * Math.Sin(angle)))

            Dim p2_x As Double = centroid_x - (((width2 / 2) * Math.Cos(angle)) - ((height2 / 2) * Math.Sin(angle)))
            Dim p2_y As Double = centroid_y + (((height2 / 2) * Math.Cos(angle)) + ((width2 / 2) * Math.Sin(angle)))

            Dim p3_x As Double = centroid_x + (((width2 / 2) * Math.Cos(angle)) + ((height2 / 2) * Math.Sin(angle)))
            Dim p3_y As Double = centroid_y + (((height2 / 2) * Math.Cos(angle)) - ((width2 / 2) * Math.Sin(angle)))

            Dim p4_x As Double = centroid_x + (((width1 / 2) * Math.Cos(angle)) + ((height1 / 2) * Math.Sin(angle)))
            Dim p4_y As Double = centroid_y + (((height1 / 2) * Math.Cos(angle)) - ((width1 / 2) * Math.Sin(angle)))

            Dim p5_x As Double = centroid_x + (((width1 / 2) * Math.Cos(angle)) - ((height1 / 2) * Math.Sin(angle)))
            Dim p5_y As Double = centroid_y - (((height1 / 2) * Math.Cos(angle)) + ((width1 / 2) * Math.Sin(angle)))

            Dim p6_x As Double = centroid_x + (((width2 / 2) * Math.Cos(angle)) - ((height2 / 2) * Math.Sin(angle)))
            Dim p6_y As Double = centroid_y - (((height2 / 2) * Math.Cos(angle)) + ((width2 / 2) * Math.Sin(angle)))

            Dim p7_x As Double = centroid_x - (((width2 / 2) * Math.Cos(angle)) + ((height2 / 2) * Math.Sin(angle)))
            Dim p7_y As Double = centroid_y - (((height2 / 2) * Math.Cos(angle)) - ((width2 / 2) * Math.Sin(angle)))

            Dim cremation As String = ""
            cremation = "{" _
                        + """geometry"": {" _
                        + """rings"": [[[" + p0_x.ToString + ", " + p0_y.ToString + "], [" + p1_x.ToString + ", " + p1_y.ToString + "]," _
                        + "[" + p2_x.ToString + ", " + p2_y.ToString + "], [" + p3_x.ToString + ", " + p3_y.ToString + "], [" + p4_x.ToString + ", " + p4_y.ToString + "]," _
                        + "[" + p5_x.ToString + ", " + p5_y.ToString + "], [" + p6_x.ToString + ", " + p6_y.ToString + "], [" + p7_x.ToString + ", " + p7_y.ToString + "], [" _
                        + p0_x.ToString + ", " + p0_y.ToString + "]]]" _
                        + "}," _
                        + """attributes"": {" _
                        + If(object_ID <> 0, """OBJECTID"": " + object_ID.ToString + ",", "") _
                        + """Burial_ID"": " + burial_id.ToString + "," _
                        + """Space_ID"": " + space_ID _
                        + "}" _
                        + "}"

            Return cremation

        End Function

        Private Sub getCentroid(ByVal sp As Feature, ByRef centroid_x As Double, ByRef centroid_y As Double)
            'For i = 0 To sp.geometry.rings(0).Count - 2
            '    centroid_x = centroid_x + sp.geometry.rings(0).Item(i).Item(0)
            '    centroid_y = centroid_y + sp.geometry.rings(0).Item(i).Item(1)
            'Next

            'centroid_y = centroid_y / (sp.geometry.rings(0).Count - 1)
            'centroid_x = centroid_x / (sp.geometry.rings(0).Count - 1)

            Dim bottom_left_x As Double = 0
            Dim bottom_left_y As Double = 0

            Dim top_right_x As Double = 0
            Dim top_right_y As Double = 0

            For Each x In sp.geometry.rings(0)
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

            centroid_x = (top_right_x + bottom_left_x) / 2
            centroid_y = (top_right_y + bottom_left_y) / 2
        End Sub

        Private Sub getbottomLeft(ByVal sp As Feature, ByRef bottom_left_x As Double, ByRef bottom_left_y As Double)
            For Each x In sp.geometry.rings(0)
                If bottom_left_x = 0 Or x.Item(0) < bottom_left_x Then
                    bottom_left_x = x.Item(0)
                End If
                If bottom_left_y = 0 Or x.Item(1) < bottom_left_y Then
                    bottom_left_y = x.Item(1)
                End If
            Next
        End Sub

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
    End Class
End Namespace