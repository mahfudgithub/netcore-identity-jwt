import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import { useAuthStore } from "../stores/auth.stores.js";
import { useCookies } from "vue3-cookies";

const { cookies } = useCookies();

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  linkActiveClass: "active",
  routes: [
    {
      path: "/",
      name: "home",
      props: true,
      component: HomeView,
    },
    {
      path: "/account/login",
      name: "login",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/account/Login.vue"),
    },
    {
      path: "/account/register",
      name: "register",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/account/Register.vue"),
    },
    {
      path: "/account/forget",
      name: "forget",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/account/ForgotPassword.vue"),
    },
    {
      path: "/account/resetpassword",
      name: "resetpassword",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/account/ResetPassword.vue"),
    },
    {
      path: "/message",
      name: "message",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/MessageMaster.vue"),
    },
  ],
  // routes: [
  //   { path: "/", component: HomeView },
  //   { ...accountRoutes },
  //   // catch all redirect to home page
  //   { path: "/:pathMatch(.*)*", redirect: "/" },
  // ],
});

// router.beforeEach((to, from, next) => {
//   if (to.name === 'login') {
//     next() // login route is always  okay (we could use the requires auth flag below). prevent a redirect loop
//   } else if (to.meta && to.meta.requiresAuth === false) {
//     next() // requires auth is explicitly set to false
//   } else if (store.getters.isLoggedIn) {
//     next() // i'm logged in. carry on
//   } else {
//     next({ name: 'login' }) // always put your redirect as the default case
//   }
// const token = localStorage.getItem("X-Access-Token");
// // If logged in, or going to the Login page.
// if (token || to.name === "register" || to.name === "login") {
//   // Continue to page.
//   console.log("token :" + token);
//   next();
// } else {
//   // Not logged in, redirect to login.
//   next({ name: "register" });
// }
//});

// router.beforeEach(async (to) => {
//   // redirect to login page if not logged in and trying to access a restricted page
//   const publicPages = ["/account/login", "/account/register"];
//   const authRequired = !publicPages.includes(to.path);
//   const authStore = useAuthStore();

//   if (authRequired && !authStore.user) {
//     authStore.returnUrl = to.fullPath;
//     return "/account/login";
//   }
// });

router.beforeEach((to, from, next) => {
  //localStorage.getItem("user"))
  const publicPages = ["/account/login", "/account/register", "/account/forget", "/account/resetpassword"];
  const authRequired = !publicPages.includes(to.path);
  const currentUser = cookies.isKey("user");
  if (authRequired && !currentUser) next({ name: "login" });
  else next();
});

export default router;
