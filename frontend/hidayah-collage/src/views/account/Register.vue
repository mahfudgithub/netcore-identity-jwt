<template>
  <div class="container mt-5">
    <div class="row d-flex justify-content-center mt-0">
      <div class="col-md-5">
        <div class="card px-5 py-4 rounded bg-light border-primary shadow" id="form1">
          <div class="form-data">
            <form action="#" v-on:submit="onRegister">
              <h3 class="text-center mb-4">Register</h3>
              <div class="forms-inputs mb-2">
                <span>First Name</span>
                <input autocomplete="off" type="text" class="form-control border-danger" v-model="firstName" placeholder="first name" maxlength="50" required />
                <div class="invalid-feedback">A valid email is required!</div>
              </div>
              <div class="forms-inputs mb-2">
                <span>Last Name</span>
                <input autocomplete="off" type="text" class="form-control" v-model="lastName" placeholder="last name" maxlength="50" />
                <div class="invalid-feedback">Password must be 8 character!</div>
              </div>
              <div class="forms-inputs mb-2">
                <span>Username</span>
                <input autocomplete="off" type="text" class="form-control border-danger" v-model="username" placeholder="username" required maxlength="30" />
                <div class="invalid-feedback">Password must be 8 character!</div>
              </div>
              <div class="forms-inputs mb-2">
                <span>Email</span>
                <input autocomplete="off" type="email" class="form-control border-danger" v-model="email" placeholder="email" maxlength="50" required />
                <div class="invalid-feedback">Password must be 8 character!</div>
              </div>
              <div class="forms-inputs mb-2">
                <span>Password</span>
                <input autocomplete="off" type="password" class="form-control border-danger" v-model="password" placeholder="password" maxlength="50" required />
                <div class="invalid-feedback">Password must be 8 character!</div>
              </div>
              <div class="forms-inputs mb-4">
                <span>Confirm Password</span>
                <input autocomplete="off" type="password" class="form-control border-danger" v-model="confPassword" placeholder="confirm password" maxlength="50" required />
                <div class="invalid-feedback">Password must be 8 character!</div>
              </div>
              <div class="mb-3">
                <div class="d-grid gap-2 d-md-flex justify-content-md-center">
                  <button type="submit" class="btn btn-success me-md-1 w-100" :disabled="isDisabled">Submit</button>
                  <RouterLink :to="{ name: 'login' }" type="button" class="btn btn-outline-danger">Cancel</RouterLink>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { useToast } from "vue-toastification";

export default {
  name: "Register",
  data() {
    return {
      firstName: "",
      lastName: "",
      username: "",
      email: "",
      password: "",
      confPassword: "",
      isDisabled: false,
    };
  },
  setup() {
    // Get toast interface
    const toast = useToast();

    // Make it available inside methods
    return { toast };
  },
  // beforeMount() {
  //   console.log(`${import.meta.env.VITE_APP_BASE_API_URL}/account/register`);
  // },
  methods: {
    onRegister(e) {
      this.$isLoading(true); // show loading screen
      this.isDisabled = true;
      e.preventDefault();
      try {
        axios
          .post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/register`, {
            FirstName: this.firstName,
            LastName: this.lastName,
            UserName: this.username,
            Email: this.email,
            Password: this.password,
            ConfirmPassword: this.confPassword,
          })
          .then((response) => {
            this.$isLoading(false);
            if (response.data.status) {
              this.toast.success(response.data.message);
              this.$router.push("/account/login");
            } else {
              this.toast.error(response.data.message);
            }
          })
          .catch((error) => {
            this.$isLoading(false);
            if (error.response) {
              console.log(error.response.data.errors);
              this.toast.error(error.response.data.errors.Password[0]);
            } else if (error.request) {
              this.toast.error("Error: Network Error");
            } else {
            }
          });
      } catch (error) {
        this.$isLoading(false);
        this.toast.error(error.message);
      } finally {
        this.isDisabled = false;
      }
    },
  },
};
</script>

<style></style>
