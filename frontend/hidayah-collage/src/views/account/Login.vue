<template>
  <div class="container mt-5">
    <div class="row d-flex justify-content-center mt-5">
      <div class="col-md-5">
        <div class="card px-5 py-4 rounded bg-light border-info shadow" id="form1">
          <div class="form-data">
            <form action="#" v-on:submit.prevent="onLogin" class="needs-validation" novalidate>
              <h3 class="text-center mb-4">Sign In</h3>
              <div class="forms-inputs mb-4">
                <input
                  autocomplete="off"
                  type="text"
                  class="form-control border-primary"
                  :class="{ 'is-invalid': isSubmitted && form.emailOrUsername.$error }"
                  id="emailOrUsername"
                  name="emailOrUsername"
                  v-model="form.emailOrUsername.$value"
                  placeholder="Email or username"
                  maxlength="50"
                  required
                />
                <div class="invalid-feedback">{{ form.emailOrUsername.$error?.message }}</div>
              </div>
              <div class="forms-inputs mb-4">
                <div class="input-group">
                  <input
                    :type="passwordText"
                    class="form-control border-primary"
                    :class="{ 'is-invalid': isSubmitted && form.password.$error }"
                    id="password"
                    name="password"
                    v-model="form.password.$value"
                    aria-label="Password"
                    placeholder="Password"
                    maxlength="50"
                    required
                  />

                  <span class="input-group-text rounded">
                    <i class="bi" :class="[eyeIcon ? 'bi-eye-fill' : 'bi-eye-slash-fill']" @click="passwordType(passwordText)" id="togglePassword" style="font-size: 1.1rem; cursor: pointer" aria-hidden="true"></i>
                  </span>
                  <span class="mt-2 px-2"><RouterLink :to="{ name: 'forget' }" style="text-decoration: none">Forget ?</RouterLink></span>
                  <div class="invalid-feedback">{{ form.password.$error?.message }}</div>
                </div>
              </div>
              <div class="mb-3"><button type="submit" class="btn btn-success w-100">Login</button></div>
              <p class="text-center">Don't have account ? <RouterLink :to="{ name: 'register' }" style="text-decoration: none">Register</RouterLink></p>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
//import axios from "axios";
import { useAuthStore } from "@/stores/auth.stores.js";
import { useToast } from "vue-toastification";
import { defineForm, field, isValidForm, toObject } from "vue-yup-form";
import * as Yup from "yup";
import { AuthService } from "@/services/auth.service.js";

const generateFormLogin = () => {
  const emailOrUsername = field("", Yup.string().required("Email or username is required filed").min(5, "Username must be at least 5 characters").max(50));
  const password = field("", Yup.string().required("Password is required field").min(5, "Password must be at least 5 characters"));

  return defineForm({
    emailOrUsername,
    password,
  });
};

//const token = "";

//export const token = token;

export default {
  name: "Login",
  data() {
    return {
      isSubmitted: false,
      passwordText: "password",
      eyeIcon: false,
      token: "",
    };
  },
  setup() {
    // Get toast interface
    const toast = useToast();

    const form = generateFormLogin();

    const api = new AuthService();

    const authStore = useAuthStore();

    return { form, authStore, toast, api };
  },
  // beforeMount() {
  //   this.$isLoading(true); // show loading screen
  // },
  // mounted() {
  //   this.$isLoading(false); // show loading screen
  // },
  methods: {
    onLogin() {
      this.isSubmitted = true;
      if (!isValidForm(this.form)) {
        //this.$isLoading(false);
        //console.log(JSON.stringify(toObject(this.form), null, 2));
        return;
      }

      this.$isLoading(true); // show loading screen
      try {
        // axios
        //   .post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/login`, {
        this.api
          .login({
            Email: toObject(this.form).emailOrUsername,
            Password: toObject(this.form).password,
          })
          .then((response) => {
            //console.log("token " + response.headers["set-cookie"].toString());
            if (response.data.status) {
              const user = response.data.data;
              // update pinia state
              //this.user = user;
              //test.headers['set-cookie'].toString();

              // store user details and jwt in local storage to keep user logged in between page refreshes
              //localStorage.setItem("user", JSON.stringify(user.refreshToken));
              this.$cookies.set("user", user, { httpOnly: true });
              //localStorage.setItem("is_expanded", "false");
              //this.$cookies.set("name", user.firstName, { httpOnly: true });
              this.toast.success(response.data.message);
              // redirect to previous url or default to home page
              this.$router.push({ name: "home" });
              //this.$router.push({ path: `/${user.FirstName}` });
              //<HomeViewVue v-bind:userLogin="user.FirstName" />;
              //this.$router.push("/account/login");
              //$router.push(this.returnUrl || "/");
            } else {
              this.toast.error(response.data.message);
            }
          })
          .catch((error) => {
            if (error.response) {
              this.toast.error(error.response.data.title);
            } else if (error.request) {
              this.toast.error("Error: Network Error");
            } else {
            }
          })
          .finally(() => {
            this.$isLoading(false); // hide loading screen
          });
      } catch (error) {
        this.toast.error(error.message);
        this.$isLoading(false); // hide loading screen
      }
      // try {
      //   console.log("test axios login");
      //   //this.authStore.login(toObject(this.form).emailOrUsername, toObject(this.form).password);
      // } catch (error) {}
      //this.$isLoading(false);
    },
    passwordType(passwordText) {
      if (passwordText == "password") {
        this.passwordText = "text";
        this.eyeIcon = true;
      } else {
        this.passwordText = "password";
        this.eyeIcon = false;
      }
    },
  },
};
</script>

<style setup>
.required {
  color: red;
}
</style>
