using System;
using Microsoft.AspNetCore.Identity;
using TestEShopMacNet7.Models.Account;

namespace TestEShopMacNet7.Models.Role
{
    public class RoleViewModel
    {
        private UserManager<ExtendedUser> userManager;

        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public RoleViewModel(UserManager<ExtendedUser> userManager)
        {
            this.userManager = userManager;
        }

        public async IAsyncEnumerable<ExtendedUser> Members()
        {
            var temp = userManager.Users.ToArray();
            for (int i = 0; i < temp.Count(); i++)
                if (await userManager.IsInRoleAsync(temp[i], Name ?? ""))
                    yield return temp[i];
        }

        public async IAsyncEnumerable<ExtendedUser> NonMembers()
        {
            var temp = userManager.Users.ToArray();
            for (int i = 0; i < temp.Count(); i++)
                if (!await userManager.IsInRoleAsync(temp[i], Name ?? ""))
                    yield return temp[i];
        }

    }
}

