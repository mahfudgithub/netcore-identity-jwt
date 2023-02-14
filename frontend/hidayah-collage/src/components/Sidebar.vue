<template>
  <!-- Modal -->
  <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-sm">
      <div class="modal-content">
        <!-- <div class="modal-header">
          <h1 class="modal-title fs-5" id="exampleModalLabel">Confirmation</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div> -->
        <div class="modal-body">
          <kbd><samp>Are you sure to exit</samp></kbd>
        </div>
        <div class="modal-footer btn-group" role="group">
          <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal" id="close">No</button>
          <button type="button" class="btn btn-danger btn-sm" @click="onLogout">Yes</button>
        </div>
      </div>
    </div>
  </div>

  <aside :class="`${is_expanded ? 'is-expanded' : ''}`">
    <div class="logo">
      <img :src="logoURL" alt="Vue" />
    </div>

    <div class="menu-toggle-wrap">
      <button class="menu-toggle" @click="ToggleMenu">
        <span class="material-icons">keyboard_double_arrow_right</span>
      </button>
    </div>

    <h3>Menu</h3>
    <div class="menu">
      <RouterLink to="/" class="button">
        <span class="material-icons">home</span>
        <span class="text">Home</span>
      </RouterLink>
      <RouterLink :to="{ name: 'message' }" class="button">
        <span class="material-icons">email</span>
        <span class="text">Message</span>
      </RouterLink>
      <!-- <RouterLink :to="{ name: 'student' }" class="button">
        <span class="material-icons">description</span>
        <span class="text">Student</span>
      </RouterLink>
      <RouterLink to="/team" class="button">
        <span class="material-icons">group</span>
        <span class="text">Team</span>
      </RouterLink>
      <RouterLink to="/contact" class="button">
        <span class="material-icons">email</span>
        <span class="text">Contact</span>
      </RouterLink> -->
    </div>

    <div class="flex"></div>

    <div class="menu">
      <!-- <RouterLink to="/settings" class="button">
        <span class="material-icons">settings</span>
        <span class="text">Settings</span>
      </RouterLink> -->
      <!-- Button trigger modal -->
      <a href="#" class="button" data-bs-toggle="modal" data-bs-target="#exampleModal"><span class="material-icons">logout</span><span class="text">Logout</span> </a>
    </div>
  </aside>
</template>

<script setup>
import { ref } from "vue";
import logoURL from "../assets/logo.png";

const is_expanded = ref(localStorage.getItem("is_expanded") === "true");

const ToggleMenu = () => {
  is_expanded.value = !is_expanded.value;
  localStorage.setItem("is_expanded", is_expanded.value);
};
</script>
<script>
import axiosinstance from "../services/axiosinstance";

export default {
  name: "sidebar",
  data() {
    return {
      token: "",
      expire: "",
      refreshToken: "",
    };
  },
  beforeMount() {
    // this.token = this.$cookies.get("user").token;
    // this.expire = this.$cookies.get("user").expireDate;
    // this.refreshToken = this.$cookies.get("user").refreshToken;
  },
  methods: {
    onLogout() {
      this.$isLoading(true); // show loading screen
      try {
        //this.api
        //axios
        axiosinstance
          .delete(`/account/logout`, {
            withCredentials: true,
          })
          .then((response) => {
            if (response.status === 204) {
              this.closeModal();
              //localStorage.removeItem("is_expanded");
              this.$cookies.remove("user");
              this.$router.push({ name: "login" });
            }
          })
          .catch((error) => {
            if (error.response) {
              //this.toast.error(error.response.data.title);
              //console.log("not auth 1");
            } else if (error.request) {
              alert(error.message);
              //console.log("Error: Network Error");
            } else {
            }
          })
          .finally(() => {
            this.closeModal();
            this.$isLoading(false); // hide loading screen
          });
      } catch (error) {
        console.log("error " + error);
        this.$isLoading(false); // hide loading screen
      }
    },
    closeModal() {
      document.getElementById("close").click();
    },
  },
};
</script>

<style lang="scss" scoped>
aside {
  display: flex;
  flex-direction: column;

  background-color: var(--dark);
  color: var(--light);

  width: calc(2rem + 32px);
  overflow: hidden;
  min-height: 100vh;
  padding: 1rem;

  transition: 0.2s ease-in-out;

  .flex {
    flex: 1 1 0%;
  }

  .logo {
    margin-bottom: 1rem;

    img {
      width: 2rem;
    }
  }

  .menu-toggle-wrap {
    display: flex;
    justify-content: flex-end;
    margin-bottom: 1rem;

    position: relative;
    top: 0;
    transition: 0.2s ease-in-out;

    .menu-toggle {
      transition: 0.2s ease-in-out;
      .material-icons {
        font-size: 2rem;
        color: var(--light);
        transition: 0.2s ease-out;
      }

      &:hover {
        .material-icons {
          color: var(--primary);
          transform: translateX(0.5rem);
        }
      }
    }
  }

  h3,
  .button .text {
    opacity: 0;
    transition: opacity 0.3s ease-in-out;
  }

  h3 {
    color: var(--grey);
    font-size: 0.875rem;
    margin-bottom: 0.5rem;
    text-transform: uppercase;
  }

  .menu {
    margin: 0 -1rem;

    .button {
      display: flex;
      align-items: center;
      text-decoration: none;

      transition: 0.2s ease-in-out;
      padding: 0.5rem 1rem;

      .material-icons {
        font-size: 2rem;
        color: var(--light);
        transition: 0.2s ease-in-out;
      }
      .text {
        color: var(--light);
        transition: 0.2s ease-in-out;
      }

      &:hover {
        background-color: var(--dark-alt);

        .material-icons,
        .text {
          color: var(--primary);
        }
      }

      &.router-link-exact-active {
        background-color: var(--dark-alt);
        border-right: 5px solid var(--primary);

        .material-icons,
        .text {
          color: var(--primary);
        }
      }
    }
  }

  .footer {
    opacity: 0;
    transition: opacity 0.3s ease-in-out;

    p {
      font-size: 0.875rem;
      color: var(--grey);
    }
  }

  &.is-expanded {
    width: var(--sidebar-width);

    .menu-toggle-wrap {
      top: -3rem;

      .menu-toggle {
        transform: rotate(-180deg);
      }
    }

    h3,
    .button .text {
      opacity: 1;
    }

    .button {
      .material-icons {
        margin-right: 1rem;
      }
    }

    .footer {
      opacity: 0;
    }
  }

  @media (max-width: 1024px) {
    position: absolute;
    z-index: 99;
  }
}
</style>
