<template>
  <Sidebar />
  <!-- <main id="msg-page">
    <h1>Message</h1>
    <div class="line-br"></div>
  </main> -->
  <main id="msg-page">
    <!-- <div class="container"> -->
    <div class="row">
      <div class="col">
        <h1>Message</h1>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <hr class="border border-danger border-1 opacity-10" />
      </div>
    </div>
    <!-- <div class="container"> -->
    <div class="row g-3">
      <div class="col-md-3">
        <input type="text" class="form-control" placeholder="First name" aria-label="First name" v-model="search" @keyup="onCheckSearchById" />
      </div>
      <div class="col-md-3">
        <button type="button" class="btn btn-primary">Primary</button>
      </div>
    </div>
    <div class="row">
      <div class="col"><hr /></div>
    </div>
    <div class="row">
      <div class="table-responsive">
        <table class="table table-striped table-hover">
          <caption>
            List of message
          </caption>
          <thead class="table-info">
            <tr>
              <th scope="col">No</th>
              <th scope="col">Message Code</th>
              <th scope="col">Message Description</th>
            </tr>
          </thead>
          <tbody v-if="messages.length > 0">
            <tr v-for="message in messages" :key="message.msG_CD">
              <th scope="row">1</th>
              <td>{{ message.msG_CD }}</td>
              <td>{{ message.msG_TEXT }}</td>
            </tr>
          </tbody>
          <tbody v-else>
            <tr>
              <td colspan="3">No Data</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <!-- </div> -->
    <!-- </div> -->
  </main>
</template>

<script>
import Sidebar from "../components/Sidebar.vue";
import axios from "axios";
export default {
  name: "message",
  components: {
    Sidebar,
  },
  data() {
    return {
      token: "",
      //validToken: false,
      expire: "",
      refreshToken: "",
      search: "",
      messages: [{ msG_CD: "", msG_TEXT: "" }],
    };
  },
  beforeMount() {
    this.token = this.$cookies.get("user").token;
    this.expire = this.$cookies.get("user").expireDate;
    this.refreshToken = this.$cookies.get("user").refreshToken;
    this.onCheckExpire();
  },
  mounted() {},
  methods: {
    SetMessages(data) {
      this.messages = data;
    },
    onCheckExpire() {
      const currentDate = new Date();
      const expireDateInt = new Date(this.expire);
      // if (parseInt(this.expire) * 1000 < currentDate.getTime()) {
      if (expireDateInt.getTime() < currentDate.getTime()) {
        //console.log("masuk expire");
        this.onRefreshToken("onReady");
        //console.log("a " + this.validToken);
      } else {
        this.onSearch();
      }
    },
    onRefreshToken(action) {
      try {
        axios
          .post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/refresh`, {
            RefreshToken: this.refreshToken,
          })
          .then((response) => {
            if (response.data.status) {
              const user = response.data.data;

              this.$cookies.remove("user");

              this.$cookies.set("user", user, { httpOnly: true });
              this.token = this.$cookies.get("user").token;
              this.expire = this.$cookies.get("user").expireDate;
              this.refreshToken = this.$cookies.get("user").refreshToken;
              //console.log("a " + action);
              if (action == "onReady") {
                this.onSearch();
              } else if (action == "onSearchById") {
                this.onSearchById(this.search);
              }
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response);
            } else if (error.request) {
              console.log("Error: Network Error");
            } else {
            }
            //main.isLoading(false);
          })
          .finally(() => {
            //this.$isLoading(false); // hide loading screen
          });
      } catch (error) {}
    },
    onSearch() {
      try {
        axios
          .get(`${import.meta.env.VITE_APP_BASE_API_URL}/message`, {
            headers: {
              Authorization: `Bearer ${this.token}`,
            },
          })
          .then((response) => {
            if (response.data.status) {
              const listMessages = response.data.data;

              this.SetMessages(listMessages);
              //console.log(this.messages);
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response.data.title);
            }
          })
          .finally(() => {});
      } catch (error) {}
    },
    onCheckSearchById() {
      const currentDate = new Date();
      const expireDateInt = new Date(this.expire);
      // if (parseInt(this.expire) * 1000 < currentDate.getTime()) {
      if (expireDateInt.getTime() < currentDate.getTime()) {
        //console.log("masuk expire");
        this.onRefreshToken("onSearchById");
        //console.log("a " + this.validToken);
      } else {
        this.onSearchById(this.search);
      }
    },
    onSearchById(id) {
      try {
        axios
          .get(`${import.meta.env.VITE_APP_BASE_API_URL}/message/${id}`, {
            headers: {
              Authorization: `Bearer ${this.token}`,
            },
          })
          .then((response) => {
            if (response.data.status) {
              const listMessages = response.data.data;

              this.SetMessages(listMessages);
              //console.log(this.messages);
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response.data.title);
            }
          })
          .finally(() => {});
      } catch (error) {}
    },
  },
};
</script>

<style></style>
