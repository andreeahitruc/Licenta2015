﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using FlickrNet;
using AttributeRouting.Web.Http;
using Back_F.Models;

namespace Back_F.ControllersApi
{
    public class FlickrAuthController : ApiController
    {
        flickr_tablesEntities db = new flickr_tablesEntities();

        [HttpGet]
        [GET("api/GetDetailsUser/{id}")]
        public Models.UserProfile GetDetails(string id)
        {
            try
            {
                UserProfile obUser = new UserProfile();
                var ob = db.users.Where(idx => idx.UserId == id).First();
                obUser.userId = ob.UserId;
                obUser.userName = ob.UserName.ToString();
                obUser.fullName = ob.Full_Name;
                return obUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [GET("api/auth/")]
        public string ReturnSecrets()
        {
            Flickr f = Models.FlickrManager.GetInstance(); //creez o instanta Flickr-> contine apikey si secret
            if (f.OAuthAccessToken != null) f.OAuthAccessToken = null;
            if (f.OAuthAccessTokenSecret != null) f.OAuthAccessTokenSecret = null;
            OAuthRequestToken token = f.OAuthGetRequestToken("http://localhost:99/homepage/"); //cand esti pe pagina cu "ok, i will auth..." te va directiona catre url-ul dat
            string url = f.OAuthCalculateAuthorizationUrl(token.Token, AuthLevel.Read);
            /*In authentification pun token-ul necesar pentru preluarea datelor aferente userului conectat*/
            Authentication.secret = token.TokenSecret;
            Authentication.token = token.Token;
            Authentication.url = url;
            Authentication.FullToken = token; 
            return Authentication.secret + "====" + Authentication.token+ "====" + Authentication.url;
        }

        [HttpPost]
        [POST("api/VerifyToken/")]
        public Models.UserProfile GetValues(Models.User.GetUserDetails userObj)
        {
            try
            { 
                Models.UserProfile obUser = new Models.UserProfile();//we create an instance of user profile
                Models.User.GetUserDetails.auth_verifier = userObj.auth_verifierInstance;//get the auth_verifier from url
                Flickr f = Models.FlickrManager.GetInstance();
                OAuthRequestToken requestToken = Authentication.FullToken as OAuthRequestToken;
                //apelez metoda pentru a prelua datele asociate userului conectat
                OAuthAccessToken accessToken = f.OAuthGetAccessToken(requestToken, Models.User.GetUserDetails.auth_verifier);
              //  Models.UpdateComments.Update(f);
                if (!Models.User.FlickerObj.UserFl.ContainsKey(accessToken.UserId))
                Models.User.FlickerObj.UserFl.Add(accessToken.UserId, f);
                try
                {
                    var result = (from t in db.users
                                    where t.UserId == accessToken.UserId select t).SingleOrDefault();
                    if (result == null) // nu exista in DB? Insert user in DB
                    {
                        //var userProfile = f.PeopleFindByUserName(accessToken.Username);//iau datele dupa username
                        obUser.userId = accessToken.UserId;
                        obUser.userName = accessToken.Username.ToString();
                        obUser.fullName = accessToken.FullName;
                        user newUser = new user()
                        {
                            UserId = accessToken.UserId,
                            UserName = obUser.userName,
                            Full_Name = obUser.fullName
                        };
                        db.users.Add(newUser);
                        db.SaveChanges();
                        f = null;
                        return obUser;
                    }
                    else //daca exista in DB, preiau datele si le returnez
                    {
                        var ob = db.users.Where(idx => idx.UserId == accessToken.UserId).First();
                        obUser.userId = ob.UserId;
                        obUser.userName = ob.UserName.ToString();
                        obUser.fullName = ob.Full_Name;
                        f = null;
                        return obUser;
                    }
                }
                catch (Exception) {
                    var ob = db.users.Where(idx => idx.UserId == accessToken.UserId).First();
                    obUser.userId = ob.UserId;
                    obUser.userName = ob.UserName.ToString();
                    obUser.fullName = ob.Full_Name;
                    f = null;
                    return obUser;
                }
            }
            catch (Exception) { return null; }
           
        }

       
    }
}
