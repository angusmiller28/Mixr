using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mixr.Models
{
    public partial class UserModel
    {

        public IEnumerable<AspNetUser> GetUser(string username)
        {
            Entities dbContext = new Entities();
            var query = from User in dbContext.AspNetUsers
                        where User.UserName == username
                        select User;

            IEnumerable<AspNetUser> productReviews = query.ToList();

            return productReviews;
        }

        public String[] GetAvatar(string username)
        {
            Entities dbContext = new Entities();

            string[] avatar = dbContext.AspNetUsers
                .Where(p => p.UserName == username)
                .Select(p => p.Avatar)
                .ToArray();
              

            return avatar;
        }
    }
}