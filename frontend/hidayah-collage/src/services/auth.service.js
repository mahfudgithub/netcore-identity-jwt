import api from "./api";
import TokenService from "./token.service";
import { User } from "@/models/register.js";

const user = new User();
export class AuthService {
  login({ Email, Password }) {
    return api.post("/account/login", {
      Email,
      Password,
    });
    //   .then((response) => {
    //     if (response.data.accessToken) {
    //       TokenService.setUser(response.data);
    //     }

    //     return response.data;
    //   });
  }

  // logout() {
  //   TokenService.removeUser();
  // }

  register({ user }) {
    return api.post("/account/register", {
      FirstName: user.FirstName,
      LastName: user.LastName,
      UserName: user.UserName,
      Email: user.Email,
      Password: user.Password,
      ConfirmPassword: user.ConfirmPassword,
    });
  }

  forgot({ Email }) {
    return api.post("/account/forgotpassword", {
      Email: Email,
    });
  }

  resetpassword({ Email, Token, NewPassword, ConfirmNewPassword }) {
    return api.post("/account/resetpassword", {
      Email,
      Token,
      NewPassword,
      ConfirmNewPassword,
    });
  }

  logout({ token }) {
    return api
      .delete("/account/logout", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        if (response.data.status) {
          TokenService.removeCookie();
        }
      });
  }

  refreshToken({ token }) {
    return (api.post("account/refresh"),
    {
      RefreshToken: token,
    }).then((response) => {
      if (response.data.status) {
        const user = response.data.data;
        TokenService.removeCookie();
        TokenService.refreshCookie(user);
      }
    });
  }

  intercepors() {
    api.interceptors.request.use(
      (config) => {
        console.log("kepanggil");
        const currentDate = new Date();
        if (parseInt(TokenService.getExpireToken()) * 1000 < currentDate.getTime()) {
          const user = this.refreshToken({ token: TokenService.getRefreshToken() });
          config.headers.Authorization = `Bearer ${user.token}`;
          console.log("kepanggil lagi");
        }
        return config;
      },
      (error) => {
        console.log("error");
        return Promise.reject(error);
      }
    );
  }
}

//export default { AuthService };
//export (AuthService() as default)
