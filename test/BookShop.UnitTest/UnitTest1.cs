using BookShop.Domain.Entities;
using System.Reflection;

namespace BookShop.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            User user = new User();

            string typeFullName = typeof(User).FullName;
            string typeName = typeof(User).Name;
            string assemblyQualifiedName = typeof(User).AssemblyQualifiedName;

            Type type1 = Type.GetType(typeName);
            Type type2 = Type.GetType(typeFullName);
            Type type3 = Type.GetType(assemblyQualifiedName);





            var auditLog = new AuditLog()
            {
                EntityTypeFullName = typeFullName,
            };

            var x = auditLog.GetEntityType(); 

            Assert.True(user.GetType() == auditLog.GetEntityType());
        }


    }
}