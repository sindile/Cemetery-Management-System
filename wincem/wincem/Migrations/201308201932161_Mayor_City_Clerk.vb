Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Mayor_City_Clerk
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Owners", "Mayor", Function(c) c.String())
            AddColumn("dbo.Owners", "CityClerk", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Owners", "CityClerk")
            DropColumn("dbo.Owners", "Mayor")
        End Sub
    End Class
End Namespace
