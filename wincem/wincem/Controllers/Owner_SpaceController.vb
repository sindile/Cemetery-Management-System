Imports System.Data.Entity

Namespace wincem
    Public Class Owner_SpaceController
        Inherits System.Web.Mvc.Controller

        Private db As New cemeteryContext

        '
        ' GET: /Owner_Space/

        Function Index() As ActionResult
            Dim owned_spaces = db.Owned_Spaces.Include(Function(o) o.Owner)
            Return View(owned_spaces.ToList().OrderBy(Function(o) o.Space_ID))
        End Function

        '
        ' GET: /Owner_Space/Details/5
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owned_space As Owned_Space = db.Owned_Spaces.Find(id)
            If IsNothing(owned_space) Then
                Return HttpNotFound()
            End If
            ViewBag.Burials = db.Burials.Where(Function(w) w.Space_ID.Equals(owned_space.Space_ID)).ToList
            Return View(owned_space)
        End Function

        Function getOwner(ByVal Space_ID As String) As ActionResult
            Dim spacequery = db.Owned_Spaces.Where(Function(s) s.Space_ID.Equals(Space_ID))
            Dim space As New Owned_Space
            If spacequery.Count > 0 Then
                space = spacequery.First()
            End If
            Dim owner As Owner = db.Owners.Find(space.OwnerID)
            'ViewBag.ownerName = owner.Owner_Name
            'ViewBag.deed_No = owner.Deed_No
            Return PartialView(space)
        End Function

        '
        ' GET: /Owner_Space/Create
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Create(ByVal Owner_ID As Integer) As ActionResult
            Dim space As New Owned_Space
            Dim owner As New Owner
            owner = db.Owners.Find(Owner_ID)
            space.Owner = owner
            space.OwnerID = owner.ID
            Return View(space)
        End Function

        '
        ' POST: /Owner_Space/Create
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        Function Create(ByVal owned_space As Owned_Space) As ActionResult
            If ModelState.IsValid Then
                Dim owner As New Owner
                owner = db.Owners.Find(owned_space.OwnerID)
                If owner.Deed_Type = "A" Then
                    Dim previous_Owned_space As New Owned_Space
                    Dim assignment As New Assignment
                    previous_Owned_space = db.Owned_Spaces.Where(Function(w) w.Space_ID = owned_space.Space_ID).First()

                    assignment.Assignment_Date = owner.Date_of_Purchase
                    assignment.Owned_Space = previous_Owned_space
                    assignment.Owned_SpaceID = previous_Owned_space.ID
                    assignment.Original_Owner = previous_Owned_space.Owner
                    assignment.Original_OwnerID = previous_Owned_space.OwnerID
                    assignment.New_Owner = owned_space.Owner
                    assignment.New_OwnerID = owned_space.OwnerID

                    previous_Owned_space.OwnerID = owned_space.OwnerID
                    previous_Owned_space.Owner = owned_space.Owner
                    db.Entry(previous_Owned_space).State = EntityState.Modified
                    db.Assignments.Add(assignment)
                Else
                    owned_space.updateSpaceAvailability("No")
                    db.Owned_Spaces.Add(owned_space)
                End If
                db.SaveChanges()
                Return RedirectToAction("Details", "Owner", New With {.id = owned_space.OwnerID})
            End If

            Return View(owned_space)
        End Function

        '
        ' GET: /Owner_Space/Edit/5
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owned_space As Owned_Space = db.Owned_Spaces.Find(id)
            If IsNothing(owned_space) Then
                Return HttpNotFound()
            End If
            ViewBag.Burials = db.Burials.Where(Function(w) w.Space_ID.Equals(owned_space.Space_ID)).ToList
            ViewBag.OwnerID = New SelectList(db.Owners, "ID", "Deed_No", owned_space.OwnerID)
            Return View(owned_space)
        End Function

        '
        ' POST: /Owner_Space/Edit/5

        <HttpPost()> _
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Edit(ByVal owned_space As Owned_Space) As ActionResult
            If ModelState.IsValid Then
                db.Entry(owned_space).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Details", "Owner", New With {.id = owned_space.OwnerID})
            End If

            ViewBag.OwnerID = New SelectList(db.Owners, "ID", "Deed_No", owned_space.OwnerID)
            Return View(owned_space)
        End Function

        '
        ' GET: /Owner_Space/Delete/5
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim owned_space As Owned_Space = db.Owned_Spaces.Find(id)
            If IsNothing(owned_space) Then
                Return HttpNotFound()
            End If
            Return View(owned_space)
        End Function

        '
        ' POST: /Owner_Space/Delete/5

        <HttpPost()> _
        <ActionName("Delete")> _
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult

            Dim owned_space As Owned_Space = db.Owned_Spaces.Find(id)
            owned_space.updateSpaceAvailability("Yes")
            db.Owned_Spaces.Remove(owned_space)
            db.SaveChanges()
            Return RedirectToAction("Details", "Owner", New With {.id = owned_space.OwnerID})
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class
End Namespace