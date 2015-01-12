Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Add_Status
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.City_Clerk", "Status", Function(c) c.String())
            AddColumn("dbo.Mayors", "Status", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Mayors", "Status")
            DropColumn("dbo.City_Clerk", "Status")
        End Sub
    End Class
End Namespace
