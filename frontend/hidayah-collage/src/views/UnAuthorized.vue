<template>
  <Sidebar />
  <main id="unauthorized-page">
    <h1>You are not Authorized</h1>
  </main>
</template>

<script>
import Sidebar from "../components/Sidebar.vue";
export default {
  name: "Unauthorized",
  components: {
    Sidebar,
  },
};
</script>

<style></style>
