<template>
  <div class="container">
    <div class="row">
      <div class="col-sm-8 offset-sm-2 mt-5">
        <RouterView />
      </div>
    </div>
  </div>
</template>

<script setup>
import { router } from "@/router";

// redirect home if already logged in
//const authStore = useAuthStore();
</script>

<style></style>
