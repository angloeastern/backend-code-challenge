using System;
using System.Collections.Generic;
using System.Text;

namespace AEChallenge.Application.Constants
{
    public static class UserRoles
    {
        public const string Administrator = "Administrator";

        public const string Finance = "Finance";

        public const string Operations = "Operations";

        public const string RegularUser = "User";

        public const string AllAppUsers = Administrator + "," + Finance + "," + Operations + "," + RegularUser;

        public const string HighPrivilegeRoles = Administrator + "," + Finance;
    }
}
