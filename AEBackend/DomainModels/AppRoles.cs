namespace AEBackend.DomainModels;

public static class AppRoles
{
  public const string AdministratorRole = "Admin";
  public const string UserRole = "User";
  public const string VipUserRole = "VipUser";
  public static readonly ApplicationRole Administrator = new ApplicationRole()
  {
    Id = "4b390270-3075-4a64-814a-6f7223e921b1",
    Name = AdministratorRole,
    NormalizedName = "ADMIN"
  };
  public static readonly ApplicationRole User = new ApplicationRole()
  {
    Id = "8a5d2509-ede6-4ae6-b9f1-61aabfa719e8",
    Name = UserRole,
    NormalizedName = "USER"
  };
  public static readonly ApplicationRole VipUser = new ApplicationRole()
  {
    Id = "c4327179-d0e6-42cd-9c40-412273761507",
    Name = VipUserRole,
    NormalizedName = "VIPUSER"
  };


  public static ApplicationRole? Get(string role)
  {

    var lowerRole = role.ToLower();
    if (lowerRole.Equals(Administrator.Name, StringComparison.CurrentCultureIgnoreCase))
    {
      return Administrator;
    }
    if (lowerRole.Equals(User.Name, StringComparison.CurrentCultureIgnoreCase))
    {
      return User;
    }
    if (lowerRole.Equals(VipUser.Name, StringComparison.CurrentCultureIgnoreCase))
    {
      return VipUser;
    }

    return null;

  }
  public static bool IsRoleValid(string role)
  {
    var lowerRole = role.ToLower();
    return lowerRole.Equals(Administrator.Name, StringComparison.CurrentCultureIgnoreCase)
            || lowerRole.Equals(User.Name, StringComparison.CurrentCultureIgnoreCase)
            || lowerRole.Equals(VipUser.Name, StringComparison.CurrentCultureIgnoreCase);
  }
}
