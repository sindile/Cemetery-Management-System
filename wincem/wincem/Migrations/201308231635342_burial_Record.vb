Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class burial_Record
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Burials", "Ordered_BY", Function(c) c.String())
            AddColumn("dbo.Burials", "Ordered_By_Address", Function(c) c.String())
            AddColumn("dbo.Burials", "Ordered_By_Phone", Function(c) c.String())
            AddColumn("dbo.Burials", "Burial_Record_Type", Function(c) c.String())
            AddColumn("dbo.Burials", "Burial_Date", Function(c) c.DateTime())
            AddColumn("dbo.Burials", "Burial_Time", Function(c) c.DateTime())
            AddColumn("dbo.Burials", "Type_of_Service", Function(c) c.String())
            AddColumn("dbo.Burials", "Receipt_Number", Function(c) c.String())
            AddColumn("dbo.Burials", "Fees", Function(c) c.String())
            AddColumn("dbo.Burials", "Pre_Need_Payment", Function(c) c.String())
            AddColumn("dbo.Burials", "Space_Purchase_1", Function(c) c.String())
            AddColumn("dbo.Burials", "Space_Purchase_2", Function(c) c.String())
            AddColumn("dbo.Burials", "Amount_Paid", Function(c) c.String())
            DropColumn("dbo.Burials", "Mortuary")
            DropColumn("dbo.Burials", "Mortuary_Phone")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Burials", "Mortuary_Phone", Function(c) c.String())
            AddColumn("dbo.Burials", "Mortuary", Function(c) c.String())
            DropColumn("dbo.Burials", "Amount_Paid")
            DropColumn("dbo.Burials", "Space_Purchase_2")
            DropColumn("dbo.Burials", "Space_Purchase_1")
            DropColumn("dbo.Burials", "Pre_Need_Payment")
            DropColumn("dbo.Burials", "Fees")
            DropColumn("dbo.Burials", "Receipt_Number")
            DropColumn("dbo.Burials", "Type_of_Service")
            DropColumn("dbo.Burials", "Burial_Time")
            DropColumn("dbo.Burials", "Burial_Date")
            DropColumn("dbo.Burials", "Burial_Record_Type")
            DropColumn("dbo.Burials", "Ordered_By_Phone")
            DropColumn("dbo.Burials", "Ordered_By_Address")
            DropColumn("dbo.Burials", "Ordered_BY")
        End Sub
    End Class
End Namespace
