namespace DDWA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrugType", "DrugTypeWeight", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DrugType", "DrugTypeWeight");
        }
    }
}
