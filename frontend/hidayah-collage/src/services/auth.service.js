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

  refreshToken({ RefreshToken }) {
    return api.post("/account/refresh", {
      RefreshToken: RefreshToken,
    });
    // .then((response) => {
    //   if (response.data.status) {
    //     const user = response.data.data;
    //     TokenService.removeCookie();
    //     TokenService.refreshCookie(user);
    //   }
    // });
  }
}

//export default { AuthService };
//export (AuthService() as default)
