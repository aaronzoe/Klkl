using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;

namespace Klkl.ServiceInterface
{
    public class UsersService:Service
    {
        public IAutoQueryDb AutoQuery { get; set; }
        public object Post(GetUsers request)
        {
            var query = AutoQuery.CreateQuery(request, Request.GetRequestParams());
            return AutoQuery.Execute(request, query);
        }

        public object Post(UpdateUsers request)
        {
            if (request.UserAuth.Id > 0)
            {
                if (request.UserAuth.LockedDate==DateTime.MinValue)
                {
                    request.UserAuth.LockedDate = null;
                }
                Db.Update(request.UserAuth);
                return request.UserAuth.Id;
            }
            else
            {
            //    IAuthRepository authRepository = new OrmLiteAuthRepository(DbFactory);
            //    IUserAuthRepository userAuthRepository = authRepository.AsUserAuthRepository(GetResolver());
                var user = AuthRepository.CreateUserAuth(request.UserAuth, "123456");
                return user.Id;
            }
        }

        public object Post(GetUser request)
        {
            GetUserResponse response=new GetUserResponse();
            response.User= Db.SingleById<UserAuth>(request.Id);
            response.Roles = Db.Select<aspnet_Roles>();
           return response;
        }
     //   public IAuthRepository AuthRepository { get; set; }
        public object Post(ChangePsw request)
        {
      //      IAuthRepository authRepository = new OrmLiteAuthRepository(DbFactory);
        //    IUserAuthRepository userAuthRepository = AuthRepository.auth.AsUserAuthRepository(GetResolver());
            var user = AuthRepository.GetUserAuth(request.Id.ToString());
            AuthRepository.UpdateUserAuth(user, user, request.PassWord);

      
            return true;
        }

        public object Post(DelUsers request)
        {
         //   IAuthRepository authRepository = new OrmLiteAuthRepository(DbFactory);
         //   IUserAuthRepository userAuthRepository = authRepository.AsUserAuthRepository(GetResolver());
            var user = AuthRepository.GetUserAuth(request.Id.ToString());
            if (user.Roles.Contains("Admin"))
            {
                throw new Exception("不能删除管理员");
            }
            AuthRepository.DeleteUserAuth(request.Id.ToString());
            return true;
        }
    }
}
