<template>
  <div class="container mt-5">
    <div class="row d-flex justify-content-center mt-5">
      <div class="col-md-5">
        <div class="card border-info shadow">
          <h4 class="card-header border-info">Ganti Password</h4>
          <div class="card-body">
            <div class="form-data">
              <form action="" v-on:submit.prevent="onClickSubmit" class="needs-validation" novalidate>
                <div class="form-group mb-3">
                  <input
                    type="text"
                    class="form-control"
                    :class="{ 'is-invalid': isSubmitted && form.emailOrUsername.$error }"
                    id="emailOrUsername"
                    name="emailOrUsername"
                    v-model="form.emailOrUsername.$value"
                    placeholder="Email"
                    maxlength="50"
                    required
                  />
                  <div id="emailHelp" class="form-text">We'll send you a message to your email after submit.</div>
                  <div class="invalid-feedback">{{ form.emailOrUsername.$error?.message }}</div>
                </div>
                <div class="mb-3">
                  <div class="d-grid gap-2 d-md-flex justify-content-md-center">
                    <button type="submit" class="btn btn-success me-md-1 w-100">Submit</button>
                    <RouterLink :to="{ name: 'login' }" type="button" class="btn btn-outline-danger">Cancel</RouterLink>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { useToast } from "vue-toastification";
import { defineForm, field, isValidForm, toObject } from "vue-yup-form";
import * as Yup from "yup";
import { AuthService } from "@/services/auth.service.js";

const generateForm = () => {
  const emailOrUsername = field("", Yup.string().email().required("Email is required filed").min(5, "Username must be at least 5 characters").max(50));

  return defineForm({
    emailOrUsername,
  });
};

export default {
  name: "forgot",
  data() {
    return {
      isSubmitted: false,
    };
  },
  setup() {
    // Get toast interface
    const toast = useToast();
    const form = generateForm();
    const api = new AuthService();

    return { toast, form, api };
  },
  methods: {
    onClickSubmit() {
      this.isSubmitted = true;
      if (!isValidForm(this.form)) {
        return;
      }

      this.$isLoading(true); // show loading screen
      try {
        this.api
          .forgot({
            Email: toObject(this.form).emailOrUsername,
          })
          .then((response) => {
            if (response.data.status) {
              this.toast.success(response.data.message);
              this.$router.push({ name: "home" });
              this.sayHi();
              // setTimeout(function () {
              //   this.toast.success("Check your email");
              // }, 3000);
            } else {
              this.toast.error(response.data.message);
            }
          })
          .catch((error) => {
            if (error.response) {
              //this.toast.error(error.response.data.title);
              this.toast.error(error.response.data.message);
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
    },
    sayHi: function () {
      var v = this;
      setTimeout(function () {
        v.toast.success("Check your email");
      }, 2600);
    },
  },
};
</script>

<style></style>
