namespace AEBackend.DomainModels;

public static class AppRoles
{
  public const string Administrator = "Admin";
  public const string User = "User";
  public const string VipUser = "VipUser";

  public static bool IsRoleValid(string role)
  {
    return role == Administrator || role == User || role == VipUser;
  }
}
