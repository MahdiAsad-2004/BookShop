namespace BookShop.Application.Authorization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredPermissionAttribute : Attribute
    {
        public readonly string _permissionName;
        public readonly string[] _permissionNames;
        public bool IsMultiple { get; private set; } 
        public RequiredPermissionAttribute(string permissionName)
        {
            _permissionName = permissionName;
        }

        public RequiredPermissionAttribute(params string[] permissionNames)
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
