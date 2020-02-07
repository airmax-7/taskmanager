namespace TaskManagerAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_board_and_task_tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Status = c.Short(nullable: false),
                        Board_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.Board_Id)
                .Index(t => t.Board_Id);
            
            AddColumn("dbo.AspNetUsers", "Task_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Task_Id");
            AddForeignKey("dbo.AspNetUsers", "Task_Id", "dbo.Tasks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Board_Id", "dbo.Boards");
            DropForeignKey("dbo.AspNetUsers", "Task_Id", "dbo.Tasks");
            DropIndex("dbo.AspNetUsers", new[] { "Task_Id" });
            DropIndex("dbo.Tasks", new[] { "Board_Id" });
            DropColumn("dbo.AspNetUsers", "Task_Id");
            DropTable("dbo.Tasks");
            DropTable("dbo.Boards");
        }
    }
}
