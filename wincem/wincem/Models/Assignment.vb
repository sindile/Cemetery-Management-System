Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations

Public Class Assignment
    Public Property ID() As Integer

    Public Property Owned_SpaceID As Integer
    Public Property Owned_Space As Owned_Space
    <DisplayFormat(DataFormatString:="{0:yyyy-MM-dd}", ApplyFormatInEditMode:=True)> _
    Public Property Assignment_Date As DateTime?

    Public Property Original_OwnerID As Integer
    Public Overridable Property Original_Owner As Owner

    Public Property New_OwnerID As Integer
    Public Overridable Property New_Owner As Owner

End Class
