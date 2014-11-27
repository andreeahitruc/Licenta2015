 $(document).ready(function (e) {
      $('#loading').show();
      $('#loading').attr('src', "../img/loading.gif");
        var data = {
            auth_verifierInstance: location.href.split('oauth_verifier=')[1]
        }
        console.log(location.href.split('oauth_verifier=')[1]);

        if (localStorage["logare"] == "progress") {
                console.log('in');
                var data = {
                    auth_verifierInstance: location.href.split('oauth_verifier=')[1]
                }
                console.log(location.href.split('oauth_verifier=')[1]);
                $.ajax({
                    type: 'POST',
                    url: apiBaseURL + 'VerifyToken/',//I have to check the ver_token to get the user ID
                    dataType: 'json',
                    data: data,
                    success: function (dataReturned) {
                        if (dataReturned == null)
                            window.location.replace("../index.html");
                        console.log(dataReturned);
                        var datax = dataReturned.userId + delimiter + dataReturned.fullName;
                        createCookie('UserProfile', datax);
                        localStorage.removeItem("logare");
                        $('.userName').html('Hello, ' + readCookie('UserProfile')[1].split('%20')[0] + ' ' + readCookie('UserProfile')[1].split('%20')[1]);
                        GetUserFriends(dataReturned.userId);
                    }
                })
            }
            else if (readCookie('UserProfile') != null) {
                $.ajax({
                    type: 'GET',
                    url: apiBaseURL + 'GetDetailsUser/' + readCookie('UserProfile')[0],
                    dataType: 'json',
                    success: function (data) {
                        var datax = data.userId + delimiter + data.fullName;
                        createCookie('UserProfile', datax);
                        $('.userName').html('Hello, ' + readCookie('UserProfile')[1].split('%20')[0] + ' ' + readCookie('UserProfile')[1].split('%20')[1]);
                        GetUserFriends(data.userId);
                    }
                })
            } else {
                window.location.replace("../index.html");
            }
        
        
        $(document).on('click', '#logOut', function () {
            deleteCookie('UserProfile');
            window.location.replace("../index.html");
        })
    });
 function GetUserFriends(idUser)
    {
        $.ajax({
            type: 'GET',
            url: apiBaseURL + '/GetFriends/' + idUser,//get all friends
            dataType: 'json',
            success: function (data) {
                $.ajax({
                    type: 'GET',
                    url: apiBaseURL + '/CreateList/' + idUser,
                    dataType: 'jsonp',
                    success: function (datax) {
                        if (datax) { alert("Graph created..."); }
                    }
                });
                $('.list').html("");
                var table = '<table><th>Your friends</th><tr>'//</table>
                var contor = 0;
                $.each(data, function (index, value) {
                    if (contor % 3 == 0) {
                        table = table + '</tr><tr><td><div class="friendTab"><a href="' + "https://secure.flickr.com/photos/" + data[index]["UserId"] + '" class="linkFriend" target="_blank"><img src="' + data[index]["PathAlias"] + '"></img></a><p class="friendName">' + data[index]["UserName"] + '</p><p class="descriptionText">' + data[index]["IconServer"] + '</p></div></td><td style="width:3px;"><td>';
                    } else {
                        table = table + '<td><div class="friendTab"><a href="' + "https://secure.flickr.com/photos/" + data[index]["UserId"] + '" class="linkFriend" target="_blank"><img src="' + data[index]["PathAlias"] + '"></img></a><p class="friendName">' + data[index]["UserName"] + '</p><p class="descriptionText">' + data[index]["IconServer"] + '</p></div></td><td style="width:3px;"><td>';
                    }
                    contor++;
                    //table = table + '<tr><td><img src="' + data[index]["PathAlias"] + '"></img></td><td>' + data[index]["UserName"] + '</td></tr>';
                });
                table = table + '</tr></table>';
                $('.list').append(table);
                $('#graph').show();
            }
        })
    }