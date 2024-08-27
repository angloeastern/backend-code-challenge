namespace AEBackend.DomainModels;

public static class AppRoles
{
  public static readonly ApplicationRole Administrator = new ApplicationRole() { Id = "4b390270-3075-4a64-814a-6f7223e921b1", Name = "Admin", NormalizedName = "ADMIN" };
  public static readonly ApplicationRole User = new ApplicationRole() { Id = "8a5d2509-ede6-4ae6-b9f1-61aabfa719e8", Name = "User", NormalizedName = "USER" };
  public static readonly ApplicationRole VipUser = new ApplicationRole() { Id = "c4327179-d0e6-42cd-9c40-412273761507", Name = "VipUser", NormalizedName = "VIPUSER" };

  public static bool IsRoleValid(string role)
  {
    var lowerRole = role.ToLower();
    return lowerRole == Administrator.Name.ToLower() || lowerRole == User.Name.ToLower() || lowerRole == VipUser.Name.ToLower();
  }
}
