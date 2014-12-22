﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;


namespace Back_F.Models
{
    public class Authentification
    {
        private static string Secret = "049b6cb6525d5d9b";
        private static string ConsumerKey = "33f3c09ec1f1baa63a45dfa6b72f5043";
        private static string request_token = "";

        public string authMethod(){
            string requestString = "http://www.flickr.com/services/oauth/request_token";
            string nonce = new Random().Next(99999).ToString();
            string timestamp = GetTimestamp();
            //create the parameter string in alphabetical order
            string parameters = "oauth_callback=" + UrlHelper.Encode("http://www.example.com");
            parameters += "&oauth_consumer_key=" + ConsumerKey;
            parameters += "&oauth_nonce=" + nonce;
            parameters += "&oauth_signature_method=HMAC-SHA1";
            parameters += "&oauth_timestamp=" + timestamp;
            parameters += "&oauth_version=1.0";

            //generate a signature base on the current requeststring and parameters
            string signature = generateSignature("GET", requestString, parameters);

            //add the parameters and signature to the requeststring
            string url = requestString + "?" + parameters + "&oauth_signature=" + signature;

            //test the request
            WebClient web = new WebClient();
            string result = web.DownloadString(url);
            return result;
        } 
        
        private static string generateSignature(string httpMethod, string ApiEndpoint, string parameters)
        {
            //url encode the API endpoint and the parameters 

            //IMPORTANT NOTE:
            //encoded text should contain uppercase characters: '=' => %3D !!! (not %3d )
            //the HtmlUtility.UrlEncode creates lowercase encoded tags!
            //Here I use a urlencode class by Ian Hopkins
            string encodedUrl = UrlHelper.Encode(ApiEndpoint);
            string encodedParameters = UrlHelper.Encode(parameters);

            //generate the basestring
            string basestring = httpMethod + "&" + encodedUrl + "&";
            parameters = UrlHelper.Encode(parameters);
            basestring = basestring + parameters;

            //hmac-sha1 encryption:

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            //create key (request_token can be an empty string)
            string key = Secret + "&" + request_token;
            byte[] keyByte = encoding.GetBytes(key);

            //create message to encrypt
            byte[] messageBytes = encoding.GetBytes(basestring);

            //encrypt message using hmac-sha1 with the provided key
            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            //signature is the base64 format for the genarated hmac-sha1 hash
            string signature = System.Convert.ToBase64String(hashmessage);

            //encode the signature to make it url safe and return the encoded url
            return UrlHelper.Encode(signature);

        }



        public static String GetTimestamp()
        {
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            return epoch.ToString();
        } 
    }
}