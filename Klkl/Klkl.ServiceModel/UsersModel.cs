using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;
using ServiceStack.Auth;


namespace Klkl.ServiceModel
{
    [Route("/users/getlist")]
    public class GetUsers:QueryBase<UserAuth>
    {
        
    }
    [Route("/users/update")]
    public class UpdateUsers
    {
        public UserAuth UserAuth { get; set; }
    }
    [Route("/users/changepsw")]
    public class ChangePsw
    {
        public int Id { get; set; }
        public string PassWord { get; set; }
    }
    [Route("/users/del")]
    public class DelUsers
    {
        public int Id { get; set; }
    }

    [Route("/user")]
    public class GetUser
    {
        public int Id { get; set; }
    }

    public class GetUserResponse
    {
        public UserAuth User { get; set; }
        public IList<aspnet_Roles> Roles { get; set; } 
    }
}
