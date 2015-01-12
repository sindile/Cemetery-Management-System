Imports System
Imports System.Data.Entity.Migrations

Namespace Migrations
    Public Partial Class Initial
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Burials",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Space_ID = c.String(),
                        .Burial_Position = c.String(),
                        .Ordered_BY = c.String(),
                        .Ordered_By_Address = c.String(),
                        .Ordered_By_Phone = c.String(),
                        .Cremation = c.String(),
                        .Garden = c.String(),
                        .Container = c.String(),
                        .First_Name = c.String(),
                        .Last_Name = c.String(),
                        .Gender = c.String(),
                        .Date_of_Death = c.DateTime(),
                        .Date_of_Birth = c.DateTime(),
                        .Permit_Number = c.String(),
                        .Work_Order_Date = c.DateTime(),
                        .Remarks = c.String(),
                        .Photo = c.String(),
                        .Birth_City_State = c.String(),
                        .Birth_County = c.String(),
                        .Death_City_State = c.String(),
                        .Death_County = c.String(),
                        .Other_Information = c.String(),
                        .Military_Service = c.String(),
                        .LF_Burial_Record = c.String(),
                        .Burial_Record_Type = c.String(),
                        .Burial_Date = c.DateTime(),
                        .Burial_Time = c.DateTime(),
                        .Type_of_Service = c.String(),
                        .Receipt_Number = c.String(),
                        .Fees = c.Double(),
                        .Pre_Need_Payment = c.Double(),
                        .Space_Purchase_1 = c.Double(),
                        .Space_Purchase_2 = c.Double(),
                        .Amount_Paid = c.Double(),
                        .Field_Verified = c.String(),
                        .Headstone_Type = c.String(),
                        .Footstone_Type = c.String(),
                        .OwnerID = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ID) _
                .ForeignKey("dbo.Owners", Function(t) t.OwnerID, cascadeDelete := True) _
                .Index(Function(t) t.OwnerID)
            
            CreateTable(
                "dbo.Owners",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Deed_No = c.String(),
                        .Deed_Type = c.String(),
                        .Date_of_Purchase = c.DateTime(),
                        .Amount_Paid = c.Double(),
                        .Owner_Name = c.String(),
                        .Owner_Address = c.String(),
                        .Phone_Number = c.String(),
                        .Deed_Name = c.String(),
                        .LF_Deed_Card = c.String(),
                        .LF_Cert_of_Ownership = c.String(),
                        .Remarks = c.String(),
                        .Mayor = c.String(),
                        .CityClerk = c.String()
                    }) _
                .PrimaryKey(Function(t) t.ID)
            
            CreateTable(
                "dbo.Owned_Space",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Space_ID = c.String(),
                        .OwnerID = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ID) _
                .ForeignKey("dbo.Owners", Function(t) t.OwnerID, cascadeDelete := True) _
                .Index(Function(t) t.OwnerID)
            
            CreateTable(
                "dbo.Assignments",
                Function(c) New With
                    {
                        .ID = c.Int(nullable:=False, identity:=True),
                        .Owned_SpaceID = c.Int(nullable:=False),
                        .Assignment_Date = c.DateTime(),
                        .Original_OwnerID = c.Int(nullable:=False),
                        .New_OwnerID = c.Int(nullable:=False)
                    }) _
                .PrimaryKey(Function(t) t.ID) _
                .ForeignKey("dbo.Owned_Space", Function(t) t.Owned_SpaceID, cascadeDelete:=False) _
                .ForeignKey("dbo.Owners", Function(t) t.Original_OwnerID, cascadeDelete:=False) _
                .ForeignKey("dbo.Owners", Function(t) t.New_OwnerID, cascadeDelete:=False) _
                .Index(Function(t) t.Owned_SpaceID) _
                .Index(Function(t) t.Original_OwnerID) _
                .Index(Function(t) t.New_OwnerID)
            
            CreateTable(
                "dbo.Relatives",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Space_ID = c.String(),
                        .Name = c.String(),
                        .Relationship = c.String(),
                        .Birth_Date = c.String(),
                        .Death_Date = c.String(),
                        .BurialID = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ID) _
                .ForeignKey("dbo.Burials", Function(t) t.BurialID, cascadeDelete := True) _
                .Index(Function(t) t.BurialID)
            
            CreateTable(
                "dbo.City_Clerk",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Name = c.String(),
                        .Signature_Path = c.String(),
                        .Start_Date = c.DateTime(nullable := False),
                        .End_Date = c.DateTime(nullable := False),
                        .Status = c.String()
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
                        .End_Date = c.DateTime(nullable := False),
                        .Status = c.String()
                    }) _
                .PrimaryKey(Function(t) t.ID)
            
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
            DropIndex("dbo.Relatives", New String() { "BurialID" })
            DropIndex("dbo.Assignments", New String() { "New_OwnerID" })
            DropIndex("dbo.Assignments", New String() { "Original_OwnerID" })
            DropIndex("dbo.Assignments", New String() { "Owned_SpaceID" })
            DropIndex("dbo.Owned_Space", New String() { "OwnerID" })
            DropIndex("dbo.Burials", New String() { "OwnerID" })
            DropForeignKey("dbo.Relatives", "BurialID", "dbo.Burials")
            DropForeignKey("dbo.Assignments", "New_OwnerID", "dbo.Owners")
            DropForeignKey("dbo.Assignments", "Original_OwnerID", "dbo.Owners")
            DropForeignKey("dbo.Assignments", "Owned_SpaceID", "dbo.Owned_Space")
            DropForeignKey("dbo.Owned_Space", "OwnerID", "dbo.Owners")
            DropForeignKey("dbo.Burials", "OwnerID", "dbo.Owners")
            DropTable("dbo.NotificationLists")
            DropTable("dbo.Mayors")
            DropTable("dbo.City_Clerk")
            DropTable("dbo.Relatives")
            DropTable("dbo.Assignments")
            DropTable("dbo.Owned_Space")
            DropTable("dbo.Owners")
            DropTable("dbo.Burials")
        End Sub
    End Class
End Namespace
