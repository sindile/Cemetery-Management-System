Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Added_Remarks
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Owners", "Remarks", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Owners", "Remarks")
        End Sub
    End Class
End Namespace
