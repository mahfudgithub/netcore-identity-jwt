import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import Toast, { POSITION } from "vue-toastification";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.min";
import "bootstrap-icons/font/bootstrap-icons.css";
import "vue-toastification/dist/index.css";
import isLoading from "vue3-loading-screen";
import { FlowerSpinner } from "epic-spinners";

const app = createApp(App);
const options = {
  // You can set your default options here
  position: POSITION.TOP_CENTER,
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
app.use(isLoading, loadingOps);
app.use(Toast, options);
app.use(router);

app.mount("#app");
