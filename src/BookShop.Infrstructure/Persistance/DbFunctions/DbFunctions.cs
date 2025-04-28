
namespace BookShop.Infrstructure.Persistance.DbFunctions
{
    public class DbFunctions
    {
        public static readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        public static readonly string FunctionsSqlFilePath = @"C:\Users\Mahdi\source\repos\My-Projects\BookShop\src\BookShop.Infrstructure\Persistance\DbFunctions\Files\";


        private readonly DbFunctionFile CalculateDiscounterPrice = new DbFunctionFile("CalculateDiscounterPrice.sql");
        
        private readonly DbFunctionFile GetValidProduct_Discounts = new DbFunctionFile("GetValidProduct_Discounts.sql");

        private readonly DbFunctionFile GetProductsWithReviewsAverageScore = new DbFunctionFile("GetProductsWithReviewsAverageScore.sql");



        public IEnumerable<DbFunctionFile> DbFunctionFiles()
        {
            yield return CalculateDiscounterPrice;
            yield return GetValidProduct_Discounts;
            yield return GetProductsWithReviewsAverageScore;
        }


     

    }


}
