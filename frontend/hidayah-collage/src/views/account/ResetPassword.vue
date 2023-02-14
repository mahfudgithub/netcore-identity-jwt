<template>
  <div class="container mt-5">
    <div class="row d-flex justify-content-center mt-0">
      <div class="col-md-5">
        <div class="card px-5 py-4 rounded bg-light border-primary shadow" id="form1">
          <div class="form-data">
            <form action="#" v-on:submit.prevent="onResetPassword" class="needs-validation row" novalidate>
              <h3 class="text-center mb-4">Reset your password</h3>
              <input type="hidden" id="email" name="email" v-model="email" />
              <input type="hidden" id="token" name="token" v-model="token" />
              <div class="forms-inputs mb-2">
                <input
                  autocomplete="off"
                  type="password"
                  class="form-control"
                  :class="{ 'is-invalid': isSubmitted && form.newPassword.$error }"
                  id="newPassword"
                  name="newPassword"
                  v-model="form.newPassword.$value"
                  placeholder="new password"
                  maxlength="50"
                  required
                />
                <div class="invalid-feedback">{{ form.newPassword.$error?.message }}</div>
              </div>
              <div class="forms-inputs mb-4">
                <input
                  autocomplete="off"
                  type="password"
                  class="form-control"
                  :class="{ 'is-invalid': isSubmitted && form.confNewPassword.$error }"
                  id="confNewPassword"
                  name="confNewPassword"
                  v-model="form.confNewPassword.$value"
                  placeholder="confirm new password"
                  maxlength="50"
                  required
                />
                <div class="invalid-feedback">{{ form.confNewPassword.$error?.message }}</div>
              </div>
              <div class="mb-3">
                <div class="d-grid gap-2 d-md-flex justify-content-md-center">
                  <button type="submit" class="btn btn-success me-md-1 w-100">Submit</button>
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
import { defineForm, field, isValidForm, toObject } from "vue-yup-form";
import * as Yup from "yup";
import { AuthService as authService } from "@/services/auth.service.js";

const generateForm = () => {
  const newPassword = field("", Yup.string().required("Password is required field").min(5, "Password must be at least 5 characters"));
  const confNewPassword = field(
    "",
    Yup.string()
      .required("Confirm Password is required field")
      .label("Confirm Password")
      //.oneOf([password.$value, null], "Passwords must match")
      //.oneOf([password.$value], ({ label }) => `${label} does not match`)
      .test("passwords-match", "Password does not match", function (value) {
        return newPassword.$value === value;
      })
  );
  return defineForm({
    newPassword,
    confNewPassword,
  });
};
export default {
  name: "reset",
  data() {
    return {
      email: "",
      token: "",
      emailVal: "",
      tokenVal: "",
      isSubmitted: false,
      valid: false,
    };
  },
  setup() {
    // Get toast interface
    const toast = useToast();

    const api = new authService();
    const form = generateForm();
    // Make it available inside methods
    return { toast, form, api };
  },
  mounted() {
    this.email = this.$route.query.email;
    this.token = this.$route.query.token;
  },
  methods: {
    onResetPassword() {
      if (this.validate(this.email) && this.validate(this.token)) {
        this.valid = true;
      }

      if (!this.valid) {
        //console.log("data valid");
        this.toast.error("Invalid Url");
        this.warningToast();
        return;
      }
      this.isSubmitted = true;
      if (!isValidForm(this.form)) {
        return;
      }
      this.$isLoading(true); // show loading screen
      try {
        this.api
          .resetpassword({
            //axios
            //.post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/resetpassword`, {
            Email: this.email,
            Token: this.token,
            NewPassword: toObject(this.form).newPassword,
            ConfirmNewPassword: toObject(this.form).confNewPassword,
          })
          .then((response) => {
            if (response.data.status) {
              this.toast.success(response.data.message);
              this.$router.push({ name: "login" });
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
        //this.toast.error(error.message);
        //this.toast.error("Bad Request");
        this.$isLoading(false); // hide loading screen
      }
    },
    validate: function (input) {
      if (!/^undefined$/.test(input)) {
        return true;
      }
    },
    warningToast: function () {
      var v = this;
      setTimeout(function () {
        v.toast.warning("Please check your Url again");
      }, 2000);
    },
  },
};
</script>

<style></style>
