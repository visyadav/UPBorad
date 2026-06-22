namespace API.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PermissionModuleAttribute : Attribute
{
    public string ModuleName { get; }

    public PermissionModuleAttribute(string moduleName)
    {
        ModuleName = moduleName;
    }
}