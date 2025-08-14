namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Content = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        AuthorId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.ArticleTags",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ArticleId = c.Long(nullable: false),
                        TagId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Body = c.String(nullable: false, maxLength: 1000),
                        CreatedAt = c.DateTime(nullable: false),
                        AuthorId = c.Long(nullable: false),
                        ArticleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: false)
                .Index(t => t.AuthorId)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ArticleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FollowerId = c.Long(nullable: false),
                        FollowingId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FollowerId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.FollowingId, cascadeDelete: false)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "FollowingId", "dbo.Users");
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.Users");
            DropForeignKey("dbo.Favorites", "UserId", "dbo.Users");
            DropForeignKey("dbo.Favorites", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.Comments", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.ArticleTags", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "AuthorId", "dbo.Users");
            DropIndex("dbo.Follows", new[] { "FollowingId" });
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            DropIndex("dbo.Favorites", new[] { "ArticleId" });
            DropIndex("dbo.Favorites", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "ArticleId" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropIndex("dbo.Tags", new[] { "Name" });
            DropIndex("dbo.ArticleTags", new[] { "TagId" });
            DropIndex("dbo.ArticleTags", new[] { "ArticleId" });
            DropIndex("dbo.Articles", new[] { "AuthorId" });
            DropTable("dbo.Follows");
            DropTable("dbo.Favorites");
            DropTable("dbo.Comments");
            DropTable("dbo.Tags");
            DropTable("dbo.ArticleTags");
            DropTable("dbo.Articles");
        }
    }
}
