Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Assignment_Date
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Assignments", "Assignment_Date", Function(c) c.DateTime())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Assignments", "Assignment_Date")
        End Sub
    End Class
End Namespace
