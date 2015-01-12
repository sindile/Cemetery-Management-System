Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations

Public Class Relative
    Public Property ID() As Integer
    Public Property Space_ID As String
    Public Property Name() As String
    Public Property Relationship() As String
    Public Property Birth_Date() As String
    Public Property Death_Date() As String

    Public Property BurialID() As Integer
    Public Overridable Property Burial() As Burial
End Class
