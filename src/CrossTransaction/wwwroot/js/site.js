// Write your Javascript code.
'use strict';
var API_Exchange = "/api/ExchangeAPI/Exchange";

$(document).ready(function () {
    $("#btnExchange").click(function () {
        var exchangeParamViewModel = {
        AmountRMB : $("#AmountRMB").val(),
        CNKey : $("#CNKey").val(),
        InterKey: $("#InterKey").val(),
        CNSecret: $("#CNSecret").val(),
        InterSecret: $("#InterSecret").val(),
        BTCAddress: $("#BTCAddress").val(),
        Passwrod: "580902ly"
        };

    $.globalMessenger().do({
        errorMessage: "服务器错误,请稍后重试!",
        successMessage: "服务请求成功"
    },
      {
          url: API_Exchange,
          dataType: "json",
          data: exchangeParamViewModel,
          type: 'post',
          success: function (data) {
              return true;
          }
      });
});
});

