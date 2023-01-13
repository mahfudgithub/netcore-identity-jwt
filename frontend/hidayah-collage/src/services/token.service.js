class TokenService {
  getLocalRefreshToken() {
    const user = JSON.parse(localStorage.getItem("user"));
    return user?.refreshToken;
  }

  getLocalAccessToken() {
    const user = JSON.parse(localStorage.getItem("user"));
    return user?.accessToken;
  }

  updateLocalAccessToken(token) {
    let user = JSON.parse(localStorage.getItem("user"));
    user.accessToken = token;
    localStorage.setItem("user", JSON.stringify(user));
  }

  getUser() {
    return JSON.parse(localStorage.getItem("user"));
  }

  getExpireToken() {
    return this.$cookies.get("user").expireDate;
  }

  getRefreshToken() {
    return this.$cookies.get("user").refreshToken;
  }

  setUser(user) {
    console.log(JSON.stringify(user));
    localStorage.setItem("user", JSON.stringify(user));
  }

  removeUser() {
    localStorage.removeItem("user");
  }

  removeCookie() {
    //this.$cookies.keys().forEach((cookie) => this.$cookies.remove(cookie));
    this.$cookies.remove("user");
  }

  refreshCookie(user) {
    this.$cookies.set("user", user);
  }
}

export default new TokenService();
