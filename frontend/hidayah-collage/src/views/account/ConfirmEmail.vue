<template>
  <h1>Your Email has been confirm</h1>
</template>

<script>
export default {
  name: "confirm",
  created() {
    this.onRedirect();
  },
  methods: {
    onRedirect() {
      this.redirectTo();
    },
    redirectTo: function () {
      var v = this;
      setTimeout(function () {
        //v.toast.warning("Please check your Url again");
        v.$router.push({ name: "profile" });
      }, 3000);
    },
  },
};
</script>

<style></style>
