//import Vue from "vue";
//import axios from "axios";
import axios from "./api";
import TokenService from "./token.service";
import { useAuthStore } from "@/stores/auth.stores.js";

axios.interceptors.request.use((request) => {
  // eslint-disable-next-line no-param-reassign
  request.config = {
    showToast: false, // may be overwritten in next line
    ...(request.config ?? {}),
    start: Date.now(),
  };

  // if (request.config.showToast) {
  //   // eslint-disable-next-line no-param-reassign
  //   request.config.requestToastId = Vue.$toast(
  //     request.config.requestToast.title,
  //   );
  // }

  return request;
});

axios.interceptors.response.use(
  (response) => {
    // const now = Date.now();
    // const request = response.config;
    // console.info(`Api Call ${request.url} took ${now - request.config.start}ms`);

    // if (request.config.requestToastId) {
    //   //Vue.$toast.dismiss(request.config.requestToastId);
    //   console.log("1");
    // }

    // if (request.config.showToast && request.config.responseToast) {
    //   //Vue.$toast(request.config.responseToast.title);
    //   console.log("2");
    // }
    const currentDate = new Date();
    if (parseInt(TokenService.getExpireToken()) * 1000 < currentDate.getTime()) {
      const user = useAuthStore.refreshToken({ token: TokenService.getRefreshToken() });
      config.headers.Authorization = `Bearer ${user.token}`;
      console.log("kepanggil lagi");
    }
    return response;
  },
  (error) => {
    console.log("error 2");
    Promise.reject(error);
  }
);

export default axios;
