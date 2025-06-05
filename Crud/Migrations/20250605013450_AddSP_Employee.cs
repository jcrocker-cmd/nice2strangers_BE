using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crud.Migrations
{
    public partial class AddSP_Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(GetStoredProcedureScript());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_Employee");
        }

        private string GetStoredProcedureScript()
        {
            return @"
            CREATE PROCEDURE SP_Employee
                @ID INT,
                @Email NVARCHAR(200),
                @Emp_Name NVARCHAR(200),
                @Designation NVARCHAR(200),
                @type NVARCHAR(50)
            AS
            BEGIN
                SET NOCOUNT ON;

                IF (@type = 'insert')
                BEGIN
                    INSERT INTO SP_Employees (Email, Emp_Name, Designation, Created_Date)
                    VALUES (@Email, @Emp_Name, @Designation, GETDATE());
                END
                ELSE IF (@type = 'get')
                BEGIN
                    SELECT * FROM SP_Employees ORDER BY ID DESC;
                END
                ELSE IF (@type = 'getid')
                BEGIN
                    SELECT * FROM SP_Employees WHERE ID = @ID;
                END
                ELSE IF (@type = 'update')
                BEGIN
                    UPDATE SP_Employees
                    SET Email = @Email,
                        Emp_Name = @Emp_Name,
                        Designation = @Designation
                    WHERE ID = @ID;
                END
                ELSE IF (@type = 'delete')
                BEGIN
                    DELETE FROM SP_Employees WHERE ID = @ID;
                END
            END;
            ";
        }
    }

}
