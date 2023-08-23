﻿using AutoMapper;
using BAL.DTOs;
using DAL;
using DAL.EF.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.Services
{
    public class AuthService
    {
        public static TokenDTO Login(string uname, string pass)
        {
            var user = DataAccessFactory.AuthData().Authenticate(uname, pass);
            if (user != null)
            {
                var token = new Token();
                token.TokenKey = Guid.NewGuid().ToString();
                token.Username = user.UserName;
                token.CreatedAt = DateTime.Now;
                token.ExpiredAt = null;
                var tk = DataAccessFactory.TokensData().Add(token);
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Token, TokenDTO>();
                });
                var mapper = new Mapper(config);
                var data = mapper.Map<TokenDTO>(tk);
                return data;
            }
            return null;
        }
        public static bool IsTokenValid(string token)
        {
            var tk = (from t in DataAccessFactory.TokensData().Get()
                      where t.TokenKey.Equals(token)
                      && t.ExpiredAt == null
                      select t).SingleOrDefault();
            if (tk != null)
            {
                return true;
            }
            return false;
        }
    }
}