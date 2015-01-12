Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Burial_Position2
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Burials", "Burial_Position", Function(c) c.String())
            DropColumn("dbo.Burials", "Burail_Postion")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Burials", "Burail_Postion", Function(c) c.String())
            DropColumn("dbo.Burials", "Burial_Position")
        End Sub
    End Class
End Namespace
