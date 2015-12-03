namespace TopSunday.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstModulation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GameTypeID = c.Int(nullable: false),
                        PlayerID = c.Int(nullable: false),
                        NumGames = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Loses = c.Int(nullable: false),
                        Draws = c.Int(nullable: false),
                        TotalPoints = c.Int(nullable: false),
                        Resume = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameDay", t => t.GameTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerID, cascadeDelete: true)
                .Index(t => t.GameTypeID)
                .Index(t => t.PlayerID);
            
            CreateTable(
                "dbo.GameDay",
                c => new
                    {
                        GameTypeID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.GameTypeID);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TablePosition = c.Int(nullable: false),
                        GameTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameDay", t => t.GameTypeID, cascadeDelete: true)
                .Index(t => t.GameTypeID);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Debit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GoldBidon",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlayerID = c.Int(nullable: false),
                        GameTypeID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameDay", t => t.GameTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerID, cascadeDelete: true)
                .Index(t => t.PlayerID)
                .Index(t => t.GameTypeID);
            
            CreateTable(
                "dbo.MVP",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlayerID = c.Int(nullable: false),
                        GameTypeID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameDay", t => t.GameTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerID, cascadeDelete: true)
                .Index(t => t.PlayerID)
                .Index(t => t.GameTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Classification", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.MVP", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.MVP", "GameTypeID", "dbo.GameDay");
            DropForeignKey("dbo.GoldBidon", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.GoldBidon", "GameTypeID", "dbo.GameDay");
            DropForeignKey("dbo.Classification", "GameTypeID", "dbo.GameDay");
            DropForeignKey("dbo.Settings", "GameTypeID", "dbo.GameDay");
            DropIndex("dbo.MVP", new[] { "GameTypeID" });
            DropIndex("dbo.MVP", new[] { "PlayerID" });
            DropIndex("dbo.GoldBidon", new[] { "GameTypeID" });
            DropIndex("dbo.GoldBidon", new[] { "PlayerID" });
            DropIndex("dbo.Settings", new[] { "GameTypeID" });
            DropIndex("dbo.Classification", new[] { "PlayerID" });
            DropIndex("dbo.Classification", new[] { "GameTypeID" });
            DropTable("dbo.MVP");
            DropTable("dbo.GoldBidon");
            DropTable("dbo.Player");
            DropTable("dbo.Settings");
            DropTable("dbo.GameDay");
            DropTable("dbo.Classification");
        }
    }
}
