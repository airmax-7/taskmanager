namespace TaskManagerAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_navigation_field_on_task : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "Board_Id", "dbo.Boards");
            DropIndex("dbo.Tasks", new[] { "Board_Id" });
            RenameColumn(table: "dbo.Tasks", name: "Board_Id", newName: "BoardId");
            AlterColumn("dbo.Tasks", "BoardId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "BoardId");
            AddForeignKey("dbo.Tasks", "BoardId", "dbo.Boards", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "BoardId", "dbo.Boards");
            DropIndex("dbo.Tasks", new[] { "BoardId" });
            AlterColumn("dbo.Tasks", "BoardId", c => c.Int());
            RenameColumn(table: "dbo.Tasks", name: "BoardId", newName: "Board_Id");
            CreateIndex("dbo.Tasks", "Board_Id");
            AddForeignKey("dbo.Tasks", "Board_Id", "dbo.Boards", "Id");
        }
    }
}
