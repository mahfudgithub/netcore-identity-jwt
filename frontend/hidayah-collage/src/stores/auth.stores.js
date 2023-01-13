import { defineStore } from "pinia";
import axios from "axios";
import { useToast } from "vue-toastification";
//import { loading } from "../main";
//import { router } from "@/router";
// import { createApp } from "vue";
// import App from "@/App.vue";
// import Loading from "vue3-loading-screen";
// import { applyStyles } from "@popperjs/core";

const toast = useToast();

// const isLoading = function () {
//   loading;
// };

// const app = createApp(App);
// const loadingOps = {
//   bg: "#41b883ad",
//   slot: `<div class="spinner-border text-danger" style="width: 4rem; height: 4rem;" role="status">
//   <span class="visually-hidden">Loading...</span>
// </div>
// `,
// };
// app.use(Loading, loadingOps);

export const useAuthStore = defineStore({
  id: "Auth",
  state: () => ({
    // initialize state from local storage to enable user to stay logged in
    user: JSON.parse(localStorage.getItem("user")),
    returnUrl: "/",
  }),
  actions: {
    async login(username, password) {
      try {
        //console.log("login");
        //main.isLoading(true);
        //isLoading.
        this.$isLoading(true); // show loading screen
        axios
          .post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/login`, {
            Email: username,
            Password: password,
          })
          .then((response) => {
            if (response.data.status) {
              const user = response.data.data;

              // update pinia state
              this.user = user;

              // store user details and jwt in local storage to keep user logged in between page refreshes
              localStorage.setItem("user", JSON.stringify(user));

              // redirect to previous url or default to home page
              this.$router.push(this.returnUrl || "/");
              //$router.push(this.returnUrl || "/");
            } else {
              toast.error(response.data.message);
            }
            // main.isLoading(false);
            //this.$isLoading(false);
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response.data.errors);
              //toast.error(error.response.data.errors.Password[0]);
            } else if (error.request) {
              toast.error("Error: Network Error");
            } else {
            }
            //main.isLoading(false);
          });
      } catch (error) {
        toast.error(error.message);
        //main.isLoading(false);
      }
    },
  },
});
