Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class City_Clerk_Mayor
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.City_Clerk",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Name = c.String(),
                        .Signature_Path = c.String(),
                        .Start_Date = c.DateTime(nullable := False),
                        .End_Date = c.DateTime(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ID)
            
            CreateTable(
                "dbo.Mayors",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Name = c.String(),
                        .Signature_Path = c.String(),
                        .Start_Date = c.DateTime(nullable := False),
                        .End_Date = c.DateTime(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ID)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.Mayors")
            DropTable("dbo.City_Clerk")
        End Sub
    End Class
End Namespace
