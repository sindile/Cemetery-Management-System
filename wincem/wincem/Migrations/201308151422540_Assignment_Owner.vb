Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Assignment_Owner
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddForeignKey("dbo.Assignments", "Original_OwnerID", "dbo.Owners", "ID", cascadeDelete:=False)
            AddForeignKey("dbo.Assignments", "New_OwnerID", "dbo.Owners", "ID", cascadeDelete:=False)
            CreateIndex("dbo.Assignments", "Original_OwnerID")
            CreateIndex("dbo.Assignments", "New_OwnerID")
        End Sub
        
        Public Overrides Sub Down()
            DropIndex("dbo.Assignments", New String() { "New_OwnerID" })
            DropIndex("dbo.Assignments", New String() { "Original_OwnerID" })
            DropForeignKey("dbo.Assignments", "New_OwnerID", "dbo.Owners")
            DropForeignKey("dbo.Assignments", "Original_OwnerID", "dbo.Owners")
        End Sub
    End Class
End Namespace
