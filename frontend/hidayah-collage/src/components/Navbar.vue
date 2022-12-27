<script>
import { RouterLink, RouterView } from "vue-router";

export default {
  name: "Navbar",
};
</script>

<template>
  <nav class="navbar navbar-expand-lg navbar-dark bg-success shadow-sm">
    <div class="container">
      <a class="navbar-brand" href="#">Hidayah School</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav ms-auto">
          <li class="nav-item">
            <RouterLink class="nav-link" to="/">Home</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink class="nav-link" :to="{ name: 'student' }">Student</RouterLink>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<style></style>
