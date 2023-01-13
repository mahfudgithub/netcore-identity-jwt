<template>
  <div class="container mt-5">
    <div class="row d-flex justify-content-center mt-0">
      <div class="col-md-5">
        <div class="card px-5 py-4 rounded bg-light border-primary shadow" id="form1">
          <div class="form-data">
            <form action="#" v-on:submit.prevent="onRegister" class="needs-validation row g-12" novalidate>
              <h3 class="text-center mb-4">Register</h3>
              <div class="forms-inputs mb-2 col-md-6">
                <label>First Name<span class="required">*</span></label>
                <input
                  autocomplete="off"
                  type="text"
                  class="form-control"
                  :class="{ 'is-invalid': isSubmitted && form.firstName.$error }"
                  id="firstName"
                  name="firstName"
                  v-model="form.firstName.$value"
                  placeholder="first name"
                  maxlength="50"
                  required
                />
                <div class="invalid-feedback">{{ form.firstName.$error?.message }}</div>
              </div>
              <div class="forms-inputs mb-2 col-md-6">
                <span>Last Name</span>
                <input autocomplete="off" type="text" class="form-control" id="lastName" name="lastName" v-model="form.lastName.$value" placeholder="last name" maxlength="50" />
                <div class="invalid-feedback">Password must be 8 character!</div>
              </div>
              <div class="forms-inputs mb-2">
                <label>Username<span class="required">*</span></label>
                <input
                  autocomplete="off"
                  type="text"
                  class="form-control"
                  :class="{ 'is-invalid': isSubmitted && form.username.$error }"
                  id="username"
                  name="username"
                  v-model="form.username.$value"
                  placeholder="username"
                  required
                  maxlength="30"
                />
                <div class="invalid-feedback">{{ form.username.$error?.message }}</div>
              </div>
              <div class="forms-inputs mb-2">
                <label>Email<span class="required">*</span></label>
                <input autocomplete="off" type="email" class="form-control" :class="{ 'is-invalid': isSubmitted && form.email.$error }" id="email" name="email" v-model="form.email.$value" placeholder="email" maxlength="50" required />
                <div class="invalid-feedback">{{ form.email.$error?.message }}</div>
              </div>
              <div class="forms-inputs mb-2">
                <label>Password<span class="required">*</span></label>
                <input
                  autocomplete="off"
                  type="password"
                  class="form-control"
                  :class="{ 'is-invalid': isSubmitted && form.password.$error }"
                  id="password"
                  name="password"
                  v-model="form.password.$value"
                  placeholder="password"
                  maxlength="50"
                  required
                />
                <div class="invalid-feedback">{{ form.password.$error?.message }}</div>
              </div>
              <div class="forms-inputs mb-4">
                <label>Confirm Password<span class="required">*</span></label>
                <input
                  autocomplete="off"
                  type="password"
                  class="form-control"
                  :class="{ 'is-invalid': isSubmitted && form.confPassword.$error }"
                  id="confPassword"
                  name="confPassword"
                  v-model="form.confPassword.$value"
                  placeholder="confirm password"
                  maxlength="50"
                  required
                />
                <div class="invalid-feedback">{{ form.confPassword.$error?.message }}</div>
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
//import axios from "axios";
import { useToast } from "vue-toastification";
import { defineForm, field, isValidForm, toObject } from "vue-yup-form";
import * as Yup from "yup";
import { AuthService as authService } from "@/services/auth.service.js";
import { User } from "@/models/register.js";

const generateForm = () => {
  const firstName = field("", Yup.string().required("First name is required field"));
  const lastName = field("", Yup.string());
  const username = field("", Yup.string().required("Username is required filed").min(5, "Username must be at least 5 characters").max(30));
  const email = field("", Yup.string().email().required("Email is required filed"));
  const password = field("", Yup.string().required("Password is required field").min(5, "Password must be at least 5 characters"));
  const confPassword = field(
    "",
    Yup.string()
      .required("Confirm Password is required field")
      .label("Confirm Password")
      //.oneOf([password.$value, null], "Passwords must match")
      //.oneOf([password.$value], ({ label }) => `${label} does not match`)
      .test("passwords-match", "Password does not match", function (value) {
        return password.$value === value;
      })
  );
  return defineForm({
    firstName,
    lastName,
    username,
    email,
    password,
    confPassword,
  });
};

export default {
  name: "Register",
  data() {
    const schema = Yup.object().shape({
      firstName: Yup.string().required("First Name is required"),
      lastName: Yup.string().required("Last Name is required"),
      username: Yup.string().required("Username is required"),
      email: Yup.string().email().required("Email is required"),
      password: Yup.string().required("Password is required").min(6, "Password must be at least 6 characters"),
      confPassword: Yup.string()
        .required("Confirm Password is required")
        .oneOf([Yup.ref("password"), null], "Passwords must match"),
    });

    return {
      schema,
      isSubmitted: false,
      isDisabled: false,
      valid: false,
    };
  },
  setup() {
    // Get toast interface
    const toast = useToast();

    const api = new authService();

    const user = new User();

    const form = generateForm();

    const checkForm = (e) => {
      e.target.classList.add("was-validated");
    };

    // Make it available inside methods
    return { toast, checkForm, form, api, user };
  },
  // beforeMount() {
  //   console.log(`${import.meta.env.VITE_APP_BASE_API_URL}/account/register`);
  // },
  methods: {
    onRegister(e) {
      // this.validate();
      // if (!this.valid) {
      //   this.checkForm(e);
      //   e.preventDefault();
      //   return;
      // }
      this.isSubmitted = true;
      if (!isValidForm(this.form)) {
        //e.preventDefault();
        //alert("required");
        return;
      } //else {
      //   alert(JSON.stringify(toObject(this.form), null, 2));
      //   console.log(toObject(this.form).firstName);
      //   return;
      // }
      this.$isLoading(true); // show loading screen
      this.isDisabled = true;
      //e.preventDefault();

      try {
        this.user = {
          FirstName: toObject(this.form).firstName,
          LastName: toObject(this.form).lastName,
          UserName: toObject(this.form).username,
          Email: toObject(this.form).email,
          Password: toObject(this.form).password,
          ConfirmPassword: toObject(this.form).confPassword,
        };
        // axios
        //   .post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/register`, {
        //     FirstName: toObject(this.form).firstName,
        //     LastName: toObject(this.form).lastName,
        //     UserName: toObject(this.form).username,
        //     Email: toObject(this.form).email,
        //     Password: toObject(this.form).password,
        //     ConfirmPassword: toObject(this.form).confPassword,
        //   })
        this.api
          .register({
            user: this.user,
          })
          .then((response) => {
            if (response.data.status) {
              this.toast.success(response.data.message);
              this.$router.push("/account/login");
            } else {
              this.toast.error(response.data.message);
            }
          })
          .catch((error) => {
            if (error.response) {
              //console.log(error.response.data.errors);
              this.toast.error(JSON.stringify(error.response.data.errors));
            } else if (error.request) {
              this.toast.error("Error: Network Error");
            } else {
            }
          })
          .finally(() => {
            this.$isLoading(false); // hide loading screen
          });
      } catch (error) {
        this.$isLoading(false);
        this.toast.error(error.message);
      } finally {
        this.isDisabled = false;
      }
    },
    // validate: function () {
    //   if (this.validFirstName(this.firstName)) {
    //     this.valid = true;
    //   }
    // },
    // validEmail: function (email) {
    //   var re = /(.+)@(.+){2,}\.(.+){2,}/;
    //   if (re.test(email.toLowerCase())) {
    //     return true;
    //   }
    // },

    // validPassword: function (password) {
    //   if (password.length > 6) {
    //     return true;
    //   }
    // },

    // validFirstName: function (name) {
    //   if (name != "" || name.length > 0) {
    //     return true;
    //   } else {
    //     return false;
    //   }
    // },

    // validUsername: function (username) {
    //   if (!/\s/.test(username)) {
    //     return true;
    //   }
    // },
  },
};
</script>

<style>
.required {
  color: red;
}
</style>
