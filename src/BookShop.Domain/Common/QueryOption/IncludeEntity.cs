namespace BookShop.Domain.Common.QueryOption
{
    public class IncludeEntity
    {
        public readonly string EntityName;
        public bool Included { get; private set; }

        public IncludeEntity(string entityName)
        {
            EntityName = entityName;
            Included = false;
        }


        public void Include()
        {
            Included = true;
        }

    }
}
