<template>
  <div class="container mt-5">
    <div class="row d-flex justify-content-center mt-5">
      <div class="col-md-5">
        <div class="card px-5 py-4 rounded bg-light border-primary shadow" id="form1">
          <div class="form-data">
            <RouterLink :to="{ name: 'login' }" type="button" class="btn btn-sm btn-outline-danger float-end">Cancel</RouterLink>

            <legend class="text-center">Register</legend>
            <div class="forms-inputs mb-4">
              <span>Email or username</span> <input autocomplete="off" type="text" class="form-control" v-model="email" placeholder="Email or username" />
              <div class="invalid-feedback">A valid email is required!</div>
            </div>
            <div class="forms-inputs mb-4">
              <span>Password</span> <input autocomplete="off" type="password" class="form-control" v-model="password" placeholder="password" />
              <div class="invalid-feedback">Password must be 8 character!</div>
            </div>
            <div class="mb-3"><button v-on:click.stop.prevent="submit" class="btn btn-success w-100">Submit</button></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup></script>

<style></style>
