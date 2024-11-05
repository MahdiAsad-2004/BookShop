namespace BookShop.Application.Authorization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredPermission : Attribute
    {
        public readonly string _permissionName;
        public readonly string[] _permissionNames;
        public bool IsMultiple { get; private set; } 
        public RequiredPermission(string permissionName)
        {
            _permissionName = permissionName;
        }

        public RequiredPermission(string[] permissionNames)
        {
            _permissionNames = permissionNames;
        }



        public string[] GetRequiredPermissions()
        {
            if (string.IsNullOrEmpty(_permissionName))
                return _permissionNames;

            return new string[] { _permissionName };
        }


    }
}
