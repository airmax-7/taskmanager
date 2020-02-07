namespace TaskManagerAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_constraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Boards", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Tasks", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "Title", c => c.String());
            AlterColumn("dbo.Boards", "Name", c => c.String());
        }
    }
}
