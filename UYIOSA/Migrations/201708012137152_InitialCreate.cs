namespace UYIOSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MatNo = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        Status = c.String(),
                        Role = c.String(),
                        AdminRegistrationTime = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.StudentDetails", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.StudentDetails",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        StudentSchoolName = c.String(),
                        StudentFirstName = c.String(nullable: false),
                        StudentLastName = c.String(nullable: false),
                        StudentMatNo = c.String(nullable: false),
                        StudentPhoneNo = c.String(nullable: false),
                        StudentSex = c.String(nullable: false),
                        StudentUserName = c.String(nullable: false),
                        StudentPassword = c.String(nullable: false),
                        StudentStatus = c.String(),
                        StudentRegisteredTime = c.String(),
                        StudentIdCardStatus = c.String(),
                        StudentSpace = c.String(),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "dbo.SchoolDetails",
                c => new
                    {
                        SchoolId = c.Int(nullable: false, identity: true),
                        SchoolName = c.String(nullable: false),
                        SchoolLocation = c.String(nullable: false),
                        SchoolCoordinatorMatNo = c.String(nullable: false),
                        SchoolCoordinatorPhoneNo = c.String(nullable: false, maxLength: 11),
                        NumberOfStudents = c.Int(nullable: false),
                        RegistrationAmount = c.Long(nullable: false),
                        SchoolRegistrationStatus = c.String(),
                        SchoolRegisteredBy = c.String(),
                        SchoolRegisteredTime = c.String(),
                        SchoolProgressCounter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SchoolId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "StudentId", "dbo.StudentDetails");
            DropIndex("dbo.Files", new[] { "StudentId" });
            DropTable("dbo.SchoolDetails");
            DropTable("dbo.StudentDetails");
            DropTable("dbo.Files");
            DropTable("dbo.AdminDetails");
        }
    }
}
