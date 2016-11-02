// Write your Javascript code.
'use strict';
var api_Exchange = "/api/ExchangeAPI/Exchange";
var api_Withdraw = "/api/ExchangeAPI/Withdraw";
var api_WithdrawUSD = "/api/ExchangeAPI/WithdrawUSD";

$(document).ready(function () {



    //Parameter View Model
    var paramViewModel = {
        AmountRMB: ko.observable(),
        CNKey: ko.observable(),
        InterKey: ko.observable(),
        CNSecret: ko.observable(),
        InterSecret: ko.observable(),
        BTCAddress: ko.observable(),
        Password: ko.observable(),
    };

    ko.applyBindings(paramViewModel, $("#HomeForm")[0]);
    //paramViewModel.AmountRMB($("#AmountRMB").val());
    paramViewModel.CNKey($("#CNKey").val()),
    paramViewModel.InterKey($("#InterKey").val()),
    paramViewModel.CNSecret($("#CNSecret").val()),
    paramViewModel.InterSecret($("#InterSecret").val()),
    paramViewModel.BTCAddress($("#BTCAddress").val()),
    paramViewModel.Password($("#Password").val());


    //Exchange Click
    $("#btnExchange").click(function () {
        paramViewModel.AmountRMB($("#AmountRMB").val());
        paramViewModel.CNKey("568e271f-e541-4a50-8bd1-a1793325cbc6"),
        paramViewModel.CNSecret("4015E70D4A0D18565382E5B51462948A"),
        paramViewModel.InterKey("2e03f3c0-8fa0-4660-8742-7558ea9519be"),
        paramViewModel.InterSecret("2C5CB3CE1A3EC34ADC68B40D63FE2B7E"),
        paramViewModel.BTCAddress("1CFDfFwguPaHZfjZy6XTwSvVjv5z77Q8Yi"),
        paramViewModel.Password("580902LY");

        $.ajax({
            type: "POST",
            dataType: "json",
            url: api_Exchange,
            data: paramViewModel,
            success: function (data) {
                //withdrawTimer.stop();
                alert("转换成功");
            },
            error: function (data) {
                //withdrawTimer.play();
                alert("转换失败");
            }
        });
 
    });

    //Withdraw Click
    $("#btnWithdraw").click(function () {
        paramViewModel.AmountRMB($("#AmountRMB").val());
        paramViewModel.CNKey("568e271f-e541-4a50-8bd1-a1793325cbc6"),
        paramViewModel.CNSecret("4015E70D4A0D18565382E5B51462948A"),
        paramViewModel.InterKey("2e03f3c0-8fa0-4660-8742-7558ea9519be"),
        paramViewModel.InterSecret("2C5CB3CE1A3EC34ADC68B40D63FE2B7E"),
        paramViewModel.BTCAddress("1CFDfFwguPaHZfjZy6XTwSvVjv5z77Q8Yi"),
        paramViewModel.Password("580902LY");


        $.ajax({
            type: "POST",
            dataType: "json",
            url: api_Withdraw,
            data: paramViewModel,
            success: function (data) {
                //withdrawTimer.stop();
                alert("转换成功");
            },
            error: function (data) {
                //withdrawTimer.play();
                alert("转换失败");
            }
        });
    });
    
});

//Get the website root path
var rootPath = function () {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    if (postPath == "/WebPage") {
        postPath = "";
    }
    return (prePath + postPath);
}