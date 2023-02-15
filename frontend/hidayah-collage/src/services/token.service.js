import { useCookies } from "vue3-cookies";
import VueJwtDecode from "vue-jwt-decode";

const { cookies } = useCookies();
class TokenService {
  // getLocalRefreshToken() {
  //   const user = JSON.parse(localStorage.getItem("user"));
  //   return user?.refreshToken;
  // }

  // getLocalAccessToken() {
  //   const user = JSON.parse(localStorage.getItem("user"));
  //   return user?.accessToken;
  // }

  // updateLocalAccessToken(token) {
  //   let user = JSON.parse(localStorage.getItem("user"));
  //   user.accessToken = token;
  //   localStorage.setItem("user", JSON.stringify(user));
  // }

  // getUser() {
  //   return JSON.parse(localStorage.getItem("user"));
  // }

  // getExpireToken() {
  //   return cookies.get("user").expireDate;
  // }

  getRefreshToken() {
    return cookies.get("user").refreshToken;
  }

  getTokenAccess() {
    return cookies.get("user").token;
  }

  // setUser(user) {
  //   console.log(JSON.stringify(user));
  //   localStorage.setItem("user", JSON.stringify(user));
  // }

  // removeUser() {
  //   localStorage.removeItem("user");
  // }

  removeCookie() {
    //this.$cookies.keys().forEach((cookie) => this.$cookies.remove(cookie));
    cookies.remove("user");
  }

  refreshCookie(user) {
    cookies.set("user", user);
  }

  getIdDecode(token) {
    const decode = VueJwtDecode.decode(token).id;
    return decode;
  }

  getNameDecode(token) {
    const decode = VueJwtDecode.decode(token).name;
    return decode;
  }

  getExpDecode(token) {
    const decode = VueJwtDecode.decode(token).exp;
    return decode;
  }

  getEmailDecode(token) {
    const decode = VueJwtDecode.decode(token).email;
    return decode;
  }

  getIsExpiredToken(expTokenTimeStamp) {
    const currentDate = new Date().getTime() / 1000;
    const currentDateTimeStamp = Math.ceil(currentDate);

    const isExpired = expTokenTimeStamp < currentDateTimeStamp;
    return isExpired;
  }
}

export default new TokenService();
