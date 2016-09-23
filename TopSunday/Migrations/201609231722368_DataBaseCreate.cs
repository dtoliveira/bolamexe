namespace TopSunday.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataBaseCreate : DbMigration
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
                        SettingsId = c.Int(nullable: false),
                        SeasonID = c.Int(nullable: false),
                        NumGames = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Loses = c.Int(nullable: false),
                        Draws = c.Int(nullable: false),
                        TotalPoints = c.Int(nullable: false),
                        Goals = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameType", t => t.GameTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerID, cascadeDelete: true)
                .ForeignKey("dbo.Season", t => t.SeasonID, cascadeDelete: true)
                .ForeignKey("dbo.Settings", t => t.SettingsId, cascadeDelete: true)
                .Index(t => t.GameTypeID)
                .Index(t => t.PlayerID)
                .Index(t => t.SettingsId)
                .Index(t => t.SeasonID);
            
            CreateTable(
                "dbo.GameType",
                c => new
                    {
                        GameTypeID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.GameTypeID);
            
            CreateTable(
                "dbo.CurrentGame",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GameDate = c.DateTime(nullable: false),
                        WasOpen = c.Boolean(nullable: false),
                        GameTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameType", t => t.GameTypeID, cascadeDelete: true)
                .Index(t => t.GameTypeID);
            
            CreateTable(
                "dbo.GameTeams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GameDate = c.DateTime(nullable: false),
                        PlayerID = c.Int(nullable: false),
                        GameTypeID = c.Int(nullable: false),
                        SeasonID = c.Int(nullable: false),
                        Goals = c.Int(nullable: false),
                        FinalResult = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameType", t => t.GameTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerID, cascadeDelete: true)
                .ForeignKey("dbo.Season", t => t.SeasonID, cascadeDelete: true)
                .Index(t => t.PlayerID)
                .Index(t => t.GameTypeID)
                .Index(t => t.SeasonID);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Debit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlayerConfirmationGames",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GoToGame = c.Boolean(nullable: false),
                        GameDate = c.DateTime(nullable: false),
                        PlayerID = c.Int(nullable: false),
                        GameTypeID = c.Int(nullable: false),
                        SeasonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameType", t => t.GameTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerID, cascadeDelete: true)
                .ForeignKey("dbo.Season", t => t.SeasonID, cascadeDelete: true)
                .Index(t => t.PlayerID)
                .Index(t => t.GameTypeID)
                .Index(t => t.SeasonID);
            
            CreateTable(
                "dbo.Season",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SeasonDesc = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TablePositions = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Players_GameType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlayerID = c.Int(nullable: false),
                        GameTypeID = c.Int(nullable: false),
                        IsSubstitute = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GameType", t => t.GameTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerID, cascadeDelete: true)
                .Index(t => t.PlayerID)
                .Index(t => t.GameTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players_GameType", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.Players_GameType", "GameTypeID", "dbo.GameType");
            DropForeignKey("dbo.Classification", "SettingsId", "dbo.Settings");
            DropForeignKey("dbo.Classification", "SeasonID", "dbo.Season");
            DropForeignKey("dbo.Classification", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.Classification", "GameTypeID", "dbo.GameType");
            DropForeignKey("dbo.GameTeams", "SeasonID", "dbo.Season");
            DropForeignKey("dbo.GameTeams", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.PlayerConfirmationGames", "SeasonID", "dbo.Season");
            DropForeignKey("dbo.PlayerConfirmationGames", "PlayerID", "dbo.Player");
            DropForeignKey("dbo.PlayerConfirmationGames", "GameTypeID", "dbo.GameType");
            DropForeignKey("dbo.GameTeams", "GameTypeID", "dbo.GameType");
            DropForeignKey("dbo.CurrentGame", "GameTypeID", "dbo.GameType");
            DropIndex("dbo.Players_GameType", new[] { "GameTypeID" });
            DropIndex("dbo.Players_GameType", new[] { "PlayerID" });
            DropIndex("dbo.PlayerConfirmationGames", new[] { "SeasonID" });
            DropIndex("dbo.PlayerConfirmationGames", new[] { "GameTypeID" });
            DropIndex("dbo.PlayerConfirmationGames", new[] { "PlayerID" });
            DropIndex("dbo.GameTeams", new[] { "SeasonID" });
            DropIndex("dbo.GameTeams", new[] { "GameTypeID" });
            DropIndex("dbo.GameTeams", new[] { "PlayerID" });
            DropIndex("dbo.CurrentGame", new[] { "GameTypeID" });
            DropIndex("dbo.Classification", new[] { "SeasonID" });
            DropIndex("dbo.Classification", new[] { "SettingsId" });
            DropIndex("dbo.Classification", new[] { "PlayerID" });
            DropIndex("dbo.Classification", new[] { "GameTypeID" });
            DropTable("dbo.Players_GameType");
            DropTable("dbo.Settings");
            DropTable("dbo.Season");
            DropTable("dbo.PlayerConfirmationGames");
            DropTable("dbo.Player");
            DropTable("dbo.GameTeams");
            DropTable("dbo.CurrentGame");
            DropTable("dbo.GameType");
            DropTable("dbo.Classification");
        }
    }
}
