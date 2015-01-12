Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Burial_Position
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Burials", "Burail_Postion", Function(c) c.String())
            DropColumn("dbo.Burials", "Geometery")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Burials", "Geometery", Function(c) c.Geometry())
            DropColumn("dbo.Burials", "Burail_Postion")
        End Sub
    End Class
End Namespace
