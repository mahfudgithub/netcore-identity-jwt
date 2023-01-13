<template>
  <Sidebar />
  <!-- <main id="msg-page">
    <h1>Message</h1>
    <div class="line-br"></div>
  </main> -->
  <main id="msg-page">
    <!-- <div class="container"> -->
    <div class="row">
      <div class="col">
        <h1>Message</h1>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <hr class="border border-danger border-1 opacity-10" />
      </div>
    </div>
    <!-- <div class="container"> -->
    <div class="row g-3">
      <div class="col-md-3">
        <input type="text" class="form-control" placeholder="First name" aria-label="First name" />
      </div>
      <div class="col-md-3">
        <button type="button" class="btn btn-primary">Primary</button>
      </div>
    </div>
    <div class="row">
      <div class="col"><hr /></div>
    </div>
    <div class="row">
      <div class="table-responsive">
        <table class="table table-striped table-hover">
          <caption>
            List of message
          </caption>
          <thead class="table-info">
            <tr>
              <th scope="col">#</th>
              <th scope="col">First</th>
              <th scope="col">Last</th>
              <th scope="col">Handle</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <th scope="row">1</th>
              <td>Mark</td>
              <td>Otto</td>
              <td>@mdo</td>
            </tr>
            <tr>
              <th scope="row">2</th>
              <td>Jacob</td>
              <td>Thornton</td>
              <td>@fat</td>
            </tr>
            <tr>
              <th scope="row">3</th>
              <td colspan="2">Larry the Bird</td>
              <td>@twitter</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <!-- </div> -->
    <!-- </div> -->
  </main>
</template>

<script>
import Sidebar from "../components/Sidebar.vue";
export default {
  name: "message",
  components: {
    Sidebar,
  },
};
</script>

<style></style>
