Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class headstone
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Burials", "Footstone_Type", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Burials", "Footstone_Type")
        End Sub
    End Class
End Namespace
