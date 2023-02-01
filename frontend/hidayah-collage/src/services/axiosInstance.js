import axios from "axios";
import TokenService from "./token.service";
import { AuthService } from "./auth.service";

const api = new AuthService();

const axiosinstance = axios.create({
  baseURL: `${import.meta.env.VITE_APP_BASE_API_URL}`,
  headers: {
    "Content-Type": "application/json",
  },
});

axiosinstance.interceptors.request.use(async (config) => {
  config.headers.Authorization = `Bearer ${TokenService.getTokenAccess()}`;

  const currentDate = new Date();
  const expireDateInt = new Date(TokenService.getExpireToken());
  const isExpired = expireDateInt.getTime() < currentDate.getTime();

  if (!isExpired) return config;

  const response = await api.refreshToken({
    RefreshToken: TokenService.getRefreshToken(),
  });
  TokenService.refreshCookie(response.data.data);
  config.headers.Authorization = `Bearer ${response.data.data.token}`;
  return config;
});

// axiosinstance.interceptors.request.use(
//   (config) => {
//     const currentDate = new Date();
//     const expireDateInt = new Date(TokenService.getExpireToken());
//     //this.refreshToken = this.$cookies.get("user").refreshToken;
//     // if (parseInt(this.expire) * 1000 < currentDate.getTime()) {
//     if (expireDateInt.getTime() < currentDate.getTime()) {
//       console.log("refresh masuk expire");
//       console.log("old token " + TokenService.getTokenAccess());
//       api
//         .refreshToken({
//           RefreshToken: TokenService.getRefreshToken(),
//         })
//         .then((response) => {
//           if (response.data.status) {
//             const user = response.data.data;
//             TokenService.removeCookie();
//             TokenService.refreshCookie(user);
//             //accessToken = user.token;
//             console.log("log success " + JSON.stringify(user.token));
//             //config.headers.Authorization = `Bearer ${user.token}`;
//             config.headers.Authorization = `Bearer ${JSON.stringify(user.token)}`;
//             //config.headers["Authorization"] = `Bearer ${JSON.stringify(user.token)}`;
//             //config.headers.common["Authorization"] = token;
//             //return config;
//           } else {
//             console.log("log error then");
//           }
//         })
//         .catch((error) => {
//           console.log("log error " + error);
//         });
//     } else {
//       config.headers.Authorization = `Bearer ${TokenService.getTokenAccess()}`;
//     }
//     return config;
//   },
//   (error) => {
//     return Promise.reject(error);
//   }
// );

export default axiosinstance;
