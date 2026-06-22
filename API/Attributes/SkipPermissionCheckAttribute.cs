namespace API.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class SkipPermissionCheckAttribute : Attribute
{
}