import axios from "axios";
import TokenService from "./token.service";
import { AuthService } from "./auth.service";
import { token } from "../models/accessToken.js";

const api = new AuthService();

let accessToken = TokenService.getTokenAccess();

const axiosinstance = axios.create({
  baseURL: `${import.meta.env.VITE_APP_BASE_API_URL}`,
});

// axiosinstance.defaults.headers.common = {
//   Authorization: `Bearer ${TokenService.getTokenAccess()}`,
// };

// axiosinstance.defaults.headers.common["leelainstance"] = "leela instance header";

axiosinstance.interceptors.request.use(
  (config) => {
    const currentDate = new Date();
    const expireDateInt = new Date(TokenService.getExpireToken());
    //this.refreshToken = this.$cookies.get("user").refreshToken;
    // if (parseInt(this.expire) * 1000 < currentDate.getTime()) {
    if (expireDateInt.getTime() < currentDate.getTime()) {
      console.log("refresh masuk expire");
      console.log("old token " + TokenService.getTokenAccess());
      api
        .refreshToken({
          RefreshToken: TokenService.getRefreshToken(),
        })
        .then((response) => {
          if (response.data.status) {
            const user = response.data.data;
            TokenService.removeCookie();
            TokenService.refreshCookie(user);
            //accessToken = user.token;
            console.log("log success " + JSON.stringify(user.token));
            //config.headers.Authorization = `Bearer ${user.token}`;
            //config.headers.Authorization = `Bearer ${TokenService.getTokenAccess()}`;
            //config.headers["Authorization"] = `Bearer ${user.token}`;
          } else {
            console.log("log error then");
          }
        })
        .catch((error) => {
          console.log("log error " + error);
        });

      //this.onRefreshToken("onReady");
      //console.log("a " + this.validToken);
    } //else {
    config.headers.Authorization = `Bearer ${TokenService.getTokenAccess()}`;
    //}
    //   else {
    //     config.headers.Authorization = `Bearer ${token}`;
    //     console.log("masuk tdk expire");
    //     //this.onSearch();
    //   }
    //const token = this.$cookies.get("user").token;

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosinstance;
