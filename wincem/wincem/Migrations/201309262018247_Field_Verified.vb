Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Field_Verified
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Burials", "Field_Verified", Function(c) c.String())
            AddColumn("dbo.Burials", "Headstone_Type", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Burials", "Headstone_Type")
            DropColumn("dbo.Burials", "Field_Verified")
        End Sub
    End Class
End Namespace
