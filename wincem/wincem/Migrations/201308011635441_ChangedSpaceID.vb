Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class ChangedSpaceID
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.Burials", "Space_ID", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Burials", "Space_ID", Function(c) c.Int(nullable := False))
        End Sub
    End Class
End Namespace
