Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Fees_to_double
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.Burials", "Fees", Function(c) c.Double(nullable:=True))
            AlterColumn("dbo.Burials", "Pre_Need_Payment", Function(c) c.Double(nullable:=True))
            AlterColumn("dbo.Burials", "Space_Purchase_1", Function(c) c.Double(nullable:=True))
            AlterColumn("dbo.Burials", "Space_Purchase_2", Function(c) c.Double(nullable:=True))
            AlterColumn("dbo.Burials", "Amount_Paid", Function(c) c.Double(nullable:=True))
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Burials", "Amount_Paid", Function(c) c.String())
            AlterColumn("dbo.Burials", "Space_Purchase_2", Function(c) c.String())
            AlterColumn("dbo.Burials", "Space_Purchase_1", Function(c) c.String())
            AlterColumn("dbo.Burials", "Pre_Need_Payment", Function(c) c.String())
            AlterColumn("dbo.Burials", "Fees", Function(c) c.String())
        End Sub
    End Class
End Namespace
