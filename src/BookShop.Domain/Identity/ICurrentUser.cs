
namespace BookShop.Domain.Identity
{
    public interface ICurrentUser
    {
        public Guid? Id { get; init;}
        public bool Authenticated { get; init; }
        public string? Email { get; init; }
        public string? Name { get; init; } 
        public string? Username { get; init; } 
        public string? PhoneNumber { get; init; } 


        //public string GetIdEmptyString()
        //{
        //    return Id is null ? string.Empty : Id.Value.ToString();
        //}

        public string GetId()
        {
            return Id.Value.ToString();
        }


        
    }
}
