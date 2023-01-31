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
  setup() {
    const axiosInterceptor = axios.create();

    //const jwtDecode = VueJwtDecode();

    axiosInterceptor.interceptors.request.use(
      async (config) => {
        const currentDate = new Date();
        // if (parseInt(expire) * 1000 < currentDate.getTime()){
        //     const response = await axios.get('http://localhost:5000/token');
        //     config.headers.Authorization = `Bearer ${response.data.accessToken}`;
        //     setToken(response.data.accessToken);
        //     // const decode = jwt_decode(response.data.accessToken);
        //     // setName(decode.name);
        //     // setExpire(decode.exp);
        // }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );
    //return { jwtDecode };
  },
  beforeMount() {
    //console.log("token " + this.$cookies.get("refreshToken"));
    //const decode = VueJwtDecode.decode(this.$cookies.get("refreshToken"));
    // console.log(decode);
    this.name = this.$cookies.get("user").firstName;
    //const loginUser = this.$route.params.data;
    const token = sessionStorage.getItem("refreshToken");
    console.log("ini token " + token);
    //this.refreshToken();
  },
  // mounted() {
  //   console.log(this.decode);
  // },
  methods: {
    refreshToken() {
      this.token = this.$cookies.get("token");
      //console.log("refresh token " + this.token);
      try {
        axios
          .post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/refresh`, {
            RefreshToken: this.token,
          })
          .then((response) => {
            if (response.data.status) {
              const user = response.data.data;

              //this.$cookies.set("token", JSON.stringify(user.token), { httpOnly: true });
              this.toast.success(response.data.message);
            } else {
              this.toast.error(response.data.message);
            }
          })
          .catch((error) => {
            if (error.response) {
              this.toast.error(error.response);
            } else if (error.request) {
              this.toast.error("Error: Network Error");
            } else {
            }
            //main.isLoading(false);
          })
          .finally(() => {
            this.$isLoading(false); // hide loading screen
          });
      } catch (error) {}
    },
  },
};
</script>
