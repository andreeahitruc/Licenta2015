
var apiBaseURL = 'http://localhost:49798/api/';
var delimiter = '----';
function createCookie(name, value, days, path, domain, secure) {
     if (days) {

        var date=new Date();
        date.setTime(date.getTime()+ (days*24*60*60*1000));
        var expires=date.toGMTString();

     }

     else var expires = "";

     cookieString= name + "=" + escape (value);

     if (expires)
     cookieString += "; expires=" + expires;

     if (path)
     cookieString += "; path=" + escape (path);

     if (domain)
     cookieString += "; domain=" + escape (domain);

     if (secure)
     cookieString += "; secure";

     document.cookie=cookieString;
}

function readCookie(name) {

    var nameEquals = name + "=";
    try
    {
        var whole_cookie=document.cookie.split(nameEquals)[1].split(";")[0]; // get only the value of the cookie we need 
                                                                         // (split by the name=, take everything after the name= and then split it by ";" and take everything before the ";")
    var crumbs=whole_cookie.split(delimiter); // split now our cookie in its information parts
    return crumbs; // return the information parts as an array
    }catch(Exception)
    {
        return null;
    }
    
}

function deleteCookie(name){
    createCookie(name, "", -1);
}