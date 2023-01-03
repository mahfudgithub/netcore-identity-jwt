import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LoginView from "../views/LoginView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  linkActiveClass: "active",
  routes: [
    {
      path: "/",
      name: "home",
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

router.beforeEach(async (to) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ["/account/login", "/account/register"];
  const authRequired = !publicPages.includes(to.path);

  if (authRequired && !JSON.parse(localStorage.getItem("user"))) {
    return "/account/login";
  }
});

export default router;
