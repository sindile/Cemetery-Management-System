Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class NotificationList
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.NotificationLists",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .First_Name = c.String(),
                        .Last_Name = c.String(),
                        .email_address = c.String()
                    }) _
                .PrimaryKey(Function(t) t.ID)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.NotificationLists")
        End Sub
    End Class
End Namespace
