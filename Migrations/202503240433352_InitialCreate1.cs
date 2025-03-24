namespace ToDoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserModels", newName: "UserModel");
            MoveTable(name: "dbo.UserModel", newSchema: "public");
        }
        
        public override void Down()
        {
            MoveTable(name: "public.UserModel", newSchema: "dbo");
            RenameTable(name: "dbo.UserModel", newName: "UserModels");
        }
    }
}
