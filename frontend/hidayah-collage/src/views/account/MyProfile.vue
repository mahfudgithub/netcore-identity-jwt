<template>
  <Sidebar />
  <main id="profile-page">
    <h1>Profile</h1>
    <hr class="border border-danger border-1 opacity-10" />
    <div class="container text-center">
      <div class="row">
        <div class="col-5">
          <p class="fw-normal fs-5 text-capitalize text-end lh-1">name</p>
          <p class="fs-5 text-capitalize text-end lh-1">username</p>
          <p class="fs-5 text-capitalize text-end lh-1">phone</p>
          <p class="fs-5 text-capitalize text-end lh-1">email</p>
          <p class="fs-5 text-capitalize text-end lh-1">email confirmed</p>
        </div>
        <div class="col-7">
          <p class="fs-5 font-monospace text-capitalize text-start lh-1">{{ name }}</p>
          <p class="fs-5 font-monospace text-lowercase text-start lh-1">{{ profiles.userName }}</p>
          <p class="fs-5 font-monospace text-lowercase text-start lh-1">{{ profiles.phoneNumber ? profiles.phoneNumber : "-" }}</p>
          <p class="fs-5 font-monospace text-lowercase text-start lh-1">{{ profiles.email }}</p>
          <!-- <p class="fs-5 font-monospace text-lowercase text-start lh-1">{{ profiles.emailConfirmed ? "<p>verified</p>":"" }}</p> -->
          <div class="row px-2" v-if="profiles.emailConfirmed">
            <p class="badge bg-primary text-lowercase text-wrap lh-1" style="width: 5rem; display: block !important">Verified</p>
            <!-- <p class="badge bg-warning text-lowercase text-wrap lh-1" style="width: 5rem; color: black !important; display: block !important">unverified</p> -->
          </div>
          <div class="row px-2" v-else>
            <!-- <p v-if="profiles.emailConfirmed" class="badge bg-primary text-lowercase text-wrap lh-1" style="width: 5rem; display: block !important">Verified</p> -->
            <p class="badge bg-warning text-lowercase text-wrap lh-1" style="width: 5rem; color: black !important; display: block !important">unverified</p>
            <p class="fs-6" style="width: 20rem; color: black !important; display: block !important">
              Do you want to verify email ?
              <a href="#" class="text-decoration-none" @click="onSendEmail">Send</a>
            </p>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>

<script>
import TokenService from "@/services/token.service";
import Sidebar from "@/components/Sidebar.vue";
import axiosinstance from "@/services/axiosinstance";
import { useToast } from "vue-toastification";
export default {
  name: "profile",
  components: {
    Sidebar,
  },
  data() {
    return {
      email: "",
      name: "",
      id: "",
      profiles: [],
    };
  },
  setup() {
    const toast = useToast();

    return { toast };
  },
  beforeMount() {
    const token = TokenService.getTokenAccess();
    const id = TokenService.getIdDecode(token);
    this.id = id;
    this.onGetProfile();
    const name = TokenService.getNameDecode(token);
    // const email = TokenService.getEmailDecode(token);
    this.name = name;
    // this.email = email;
  },
  mounted() {
    //this.name = name;
    //this.email = email;
  },
  methods: {
    SetProfiles(data) {
      this.profiles = data;
    },
    onGetProfile() {
      try {
        axiosinstance
          .get(`/account/profile/${this.id}`)
          .then((response) => {
            if (response.data.status) {
              const listProfiles = response.data.data;

              this.SetProfiles(listProfiles);
            } else {
              this.SetProfiles([]);
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response.data.title);
            }
          });
      } catch (error) {}
    },
    onSendEmail() {
      this.$isLoading(true); // show loading screen
      try {
        axiosinstance
          .post(`/account/confirmedemail/${this.id}`)
          .then((response) => {
            if (response.data.status) {
              this.toast.success(response.data.message);
              //const listProfiles = response.data.data;
              //this.SetProfiles(listProfiles);
            } else {
              //this.SetProfiles([]);
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response.data.title);
            }
          })
          .finally(() => {
            this.$isLoading(false); // hide loading screen
          });
      } catch (error) {}
    },
  },
};
</script>

<style></style>
