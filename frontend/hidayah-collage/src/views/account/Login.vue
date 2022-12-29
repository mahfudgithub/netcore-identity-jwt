<template>
  <div class="container mt-5">
    <div class="row d-flex justify-content-center mt-5">
      <div class="col-md-5">
        <div class="card px-5 py-4 rounded bg-light border-info shadow" id="form1">
          <div class="form-data">
            <h3 class="text-center mb-4">Sign In</h3>
            <div class="forms-inputs mb-4">
              <div class="input-group">
                <input autocomplete="off" type="text" class="form-control" v-model="email" placeholder="Email or username" maxlength="50" />
              </div>
              <div class="invalid-feedback">A valid email is required!</div>
            </div>
            <div class="forms-inputs mb-4">
              <div class="input-group">
                <input :type="passwordText" class="form-control" v-model="password" aria-label="Password" placeholder="Password" maxlength="50" required />
                <span class="input-group-text">
                  <i class="bi" :class="[eyeIcon ? 'bi-eye-fill' : 'bi-eye-slash-fill']" @click="passwordType(passwordText)" id="togglePassword" style="font-size: 1.1rem; cursor: pointer" aria-hidden="true"></i>
                </span>
                <span class="mt-2 px-2">Forget?</span>
              </div>
              <div class="invalid-feedback">Password must be 8 character!</div>
            </div>
            <div class="mb-3"><button v-on:click.stop.prevent="submit" class="btn btn-success w-100">Login</button></div>
            <p class="text-center">Don't have account ? <RouterLink :to="{ name: 'register' }" style="text-decoration: none">Register</RouterLink></p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import VuePassword from "vue-password";

export default {
  name: "Login",
  components: { VuePassword },
  data() {
    return {
      email: "",
      password: "",
      passwordText: "password",
      eyeIcon: false,
    };
  },
  methods: {
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

<style></style>
