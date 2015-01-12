Imports System.Data.Entity

Namespace wincem
    Public Class AssignmentController
        Inherits System.Web.Mvc.Controller

        Private db As New cemeteryContext

        '
        ' GET: /Assignment/

        Function Index() As ActionResult
            Dim assignments = db.Assignments.Include(Function(a) a.Owned_Space)
            Return View(assignments.ToList())
        End Function

        '
        ' GET: /Assignment/Details/5

        Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim assignment As Assignment = db.Assignments.Find(id)
            If IsNothing(assignment) Then
                Return HttpNotFound()
            End If
            Return View(assignment)
        End Function

        '
        ' GET: /Assignment/Create
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Create(ByVal owned_space_id As Integer, ByVal owner_id As Integer) As ActionResult
            ViewBag.Owned_SpaceID = New SelectList(db.Owned_Spaces, "ID", "Space_ID")
            Dim assignment As New Assignment
            assignment.Owned_SpaceID = owned_space_id
            assignment.Owned_Space = db.Owned_Spaces.Find(owned_space_id)
            assignment.Original_Owner = db.Owners.Find(owner_id)
            assignment.Original_OwnerID = owner_id
            Return View(assignment)
        End Function

        '
        ' POST: /Assignment/Create
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        Function Create(ByVal assignment As Assignment) As ActionResult
            If ModelState.IsValid Then
                Dim space = db.Owned_Spaces.Find(assignment.Owned_SpaceID)
                space.OwnerID = assignment.New_OwnerID
                space.Owner = assignment.New_Owner
                db.Entry(space).State = EntityState.Modified
                db.Assignments.Add(assignment)
                db.SaveChanges()
                Return RedirectToAction("Details", "Owner_Space", New With {.id = assignment.Owned_SpaceID})
            End If

            ViewBag.Owned_SpaceID = New SelectList(db.Owned_Spaces, "ID", "Space_ID", assignment.Owned_SpaceID)
            Return View(assignment)
        End Function

        '
        ' GET: /Assignment/Edit/5
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim assignment As Assignment = db.Assignments.Find(id)
            If IsNothing(assignment) Then
                Return HttpNotFound()
            End If
            ViewBag.Owned_SpaceID = New SelectList(db.Owned_Spaces, "ID", "Space_ID", assignment.Owned_SpaceID)
            Return View(assignment)
        End Function

        '
        ' POST: /Assignment/Edit/5
        <Authorize(Roles:="Cem_Admins, Cem_Deed_Editors")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        Function Edit(ByVal assignment As Assignment) As ActionResult
            If ModelState.IsValid Then
                Dim space = db.Owned_Spaces.Find(assignment.Owned_SpaceID)
                space.OwnerID = assignment.New_OwnerID
                space.Owner = assignment.New_Owner
                db.Entry(space).State = EntityState.Modified
                db.Entry(assignment).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If

            ViewBag.Owned_SpaceID = New SelectList(db.Owned_Spaces, "ID", "Space_ID", assignment.Owned_SpaceID)
            Return View(assignment)
        End Function

        '
        ' GET: /Assignment/Delete/5
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
            Dim assignment As Assignment = db.Assignments.Find(id)
            If IsNothing(assignment) Then
                Return HttpNotFound()
            End If
            Return View(assignment)
        End Function

        '
        ' POST: /Assignment/Delete/5
        <Authorize(Roles:="Cem_Admins")> _
        <OutputCache(NoStore:=True, Duration:=0, VaryByParam:="*")> _
        <HttpPost()> _
        <ActionName("Delete")> _
        Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
            Dim assignment As Assignment = db.Assignments.Find(id)
            db.Assignments.Remove(assignment)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class
End Namespace