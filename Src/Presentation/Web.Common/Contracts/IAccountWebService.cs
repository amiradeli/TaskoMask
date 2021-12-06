﻿using TaskoMask.Application.Share.Dtos.Common.Users;
using TaskoMask.Application.Share.Dtos.Team.Members;
using TaskoMask.Application.Share.Helpers;

namespace TaskoMask.Web.Common.Contracts
{
    public  interface IAccountWebService
    {
        Task<Result<string>> Login(UserLoginDto input);
        Task<Result<CommandResult>> Register(MemberRegisterDto input);
    }
}
