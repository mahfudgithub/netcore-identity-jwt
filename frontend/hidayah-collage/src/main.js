import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { createPinia } from "pinia";
import Toast, { POSITION } from "vue-toastification";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.min";
import "bootstrap-icons/font/bootstrap-icons.css";
import "vue-toastification/dist/index.css";
import Loading from "vue3-loading-screen";
import VueCookies from "vue3-cookies";
// import VueAxios from "vue-axios";
// import axios from "./services/axiosInterceptors.js";

const app = createApp(App);
const options = {
  // You can set your default options here
  position: POSITION.TOP_RIGHT,
  timeout: 2500,
  pauseOnHover: true,
  pauseOnFocusLoss: true,
};
const loadingOps = {
  bg: "#41b883ad",
  slot: `<div class="spinner-border text-danger" style="width: 4rem; height: 4rem;" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
`,
};
app.use(VueCookies, { expire: "30d", sameSite: "Lax", secure: true });
//app.use(VueAxios, axios);
//app.provide("axios", app.config.globalProperties.axios); // provide 'axios'
app.use(Loading, loadingOps);
app.use(Toast, options);
app.use(createPinia());
app.use(router);

//export const loading = app.use(Loading, loadingOps);

// app.component("", {
//   render: function (createElemeny) {
//     return createElemeny("style", this.$isLoading);
//   },
// });

// app.component('err-text', ErrorText)
//     .component('v-style', {
//     render: function (h) {
//         return h('style', this.$slots.default())
//     }
// });

app.mount("#app");
