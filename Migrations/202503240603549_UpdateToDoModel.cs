namespace ToDoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateToDoModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.TodoModel", "EditedBy", c => c.String());
            AddColumn("public.TodoModel", "EditedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("public.TodoModel", "EditedDate");
            DropColumn("public.TodoModel", "EditedBy");
        }
    }
}
