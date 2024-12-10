using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace BookShop.Infrstructure.Persistance.DbFunctions
{
    public class DbFunctionFile
    {
        private string FileName { get; set; }
        public DbFunctionFile(string fileName)
        {
            FileName = fileName;
        }

        public string GetFileContent()
        {
            string filePath = Path.Combine(DbFunctions.FunctionsSqlFilePath, FileName);
            return File.ReadAllText(filePath);
        }

        public void AddToMigration(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(GetFileContent());
        } 


    }


}
