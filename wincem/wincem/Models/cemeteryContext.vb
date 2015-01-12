Imports System.Data.Entity

Public Class cemeteryContext
    Inherits DbContext
    Public Property Burials() As DbSet(Of Burial)
    Public Property Owners() As DbSet(Of Owner)
    Public Property Assignments() As DbSet(Of Assignment)
    Public Property Relatives() As DbSet(Of Relative)
    Public Property Owned_Spaces() As DbSet(Of Owned_Space)
    Public Property City_Clerks() As DbSet(Of City_Clerk)
    Public Property Mayors() As DbSet(Of Mayor)
    Public Property Notification_List() As DbSet(Of NotificationList)
End Class
