Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class fees
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.Burials", "Fees", Function(c) c.Double())
            AlterColumn("dbo.Burials", "Pre_Need_Payment", Function(c) c.Double())
            AlterColumn("dbo.Burials", "Space_Purchase_1", Function(c) c.Double())
            AlterColumn("dbo.Burials", "Space_Purchase_2", Function(c) c.Double())
            AlterColumn("dbo.Burials", "Amount_Paid", Function(c) c.Double())
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Burials", "Amount_Paid", Function(c) c.Double(nullable := False))
            AlterColumn("dbo.Burials", "Space_Purchase_2", Function(c) c.Double(nullable := False))
            AlterColumn("dbo.Burials", "Space_Purchase_1", Function(c) c.Double(nullable := False))
            AlterColumn("dbo.Burials", "Pre_Need_Payment", Function(c) c.Double(nullable := False))
            AlterColumn("dbo.Burials", "Fees", Function(c) c.Double(nullable := False))
        End Sub
    End Class
End Namespace
