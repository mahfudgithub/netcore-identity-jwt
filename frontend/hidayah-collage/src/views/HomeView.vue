<template>
  <Sidebar />
  <main id="home-page">
    <h1>Home</h1>
    <p>
      Welcome <b>{{ name }}</b>
    </p>
  </main>
  <!-- <div class="container">
    <div class="row mt-3 ms-2">
      <h1>Home</h1>
      <p>
        Welcome <strong>{{ name }}</strong>
      </p>
    </div>
    <div class="row">
      <div class="col">
        <h2>row baru</h2>
      </div>
    </div>
  </div> -->
</template>

<script>
import axios from "axios";
import Sidebar from "../components/Sidebar.vue";
//import VueJwtDecode from "vue-jwt-decode";
import TokenService from "../services/token.service";

export default {
  name: "home",
  props: ["data"],
  components: {
    Sidebar,
  },
  data() {
    return {
      token: "",
      name: "",
      userLogin: "Ahmad",
    };
  },
  // created() {
  //   console.log(this.$route.fullPath);
  // },
  setup() {},
  beforeMount() {
    //console.log("token " + this.$cookies.get("refreshToken"));
    //const decode = VueJwtDecode.decode(TokenService.getTokenAccess());
    const token = TokenService.getTokenAccess();
    const name = TokenService.getNameDecode(token);
    const email = TokenService.getEmailDecode(token);
    //const exp = TokenService.getExpDecode(token);
    //const expToken = new Date(TokenService.getExpireToken()).getTime() / 1000;
    //const currentDate = new Date().getTime() / 1000;
    //console.log("decode exp " + exp + " from result " + expToken + " current " + Math.floor(currentDate));
    this.name = name;
    //const loginUser = this.$route.params.data;
    // const token = sessionStorage.getItem("refreshToken");
    // console.log("ini token " + token);
    //this.refreshToken();
  },
  // mounted() {
  //   console.log(this.decode);
  // },
  methods: {},
};
</script>
