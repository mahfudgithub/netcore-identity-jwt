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
    <!-- Modal -->
    <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered modal-sm">
        <div class="modal-content">
          <!-- <div class="modal-header">
          <h1 class="modal-title fs-5" id="exampleModalLabel">Confirmation</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div> -->
          <div class="modal-body">
            <kbd><samp>Are you sure to delete</samp></kbd>
          </div>
          <div class="modal-footer btn-group" role="group">
            <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal" id="closeDelete">No</button>
            <button type="button" class="btn btn-danger btn-sm" @click="onCheckDelete">Yes</button>
          </div>
        </div>
      </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" tabindex="-1" id="EditModal" aria-labelledby="basicModal" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Add/Edit</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div class="container ms-3">
              <div class="row mb-2">
                <div class="col-sm-4">
                  <label for="" class="col-form-label">Message Code</label>
                </div>
                <div class="col-sm-7">
                  <input type="text" class="form-control" v-model="msgCode" :disabled="isDisabled" />
                </div>
              </div>
              <div class="row">
                <div class="col-sm-4">
                  <label for="" class="col-form-label">Message Desc</label>
                </div>
                <div class="col-sm-7">
                  <textarea rows="3" v-model="msgDesc" type="text" class="form-control" />
                </div>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" id="closeModal">Close</button>
            <button type="button" class="btn btn-primary" @click="onBeforeSave">Save</button>
          </div>
        </div>
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
        <input type="text" class="form-control" placeholder="search msg code" aria-label="First name" v-model="search" @keyup="onCheckSearchById" />
      </div>
      <div class="col-md-3">
        <button type="button" class="btn btn-warning" @click="onClickClear">Clear</button>
      </div>
      <div class="col-md-6 d-grid justify-content-md-end">
        <div class="btn">
          <button type="button" class="btn btn-primary" @click="onClickModalAdd">Add</button>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col"><hr /></div>
    </div>
    <div class="row">
      <div class="table-responsive">
        <table class="table table-striped table-hover">
          <caption>
            Total Data
            {{
              this.totalItems
            }}
          </caption>
          <thead class="table-info">
            <tr>
              <th width="120px" scope="col" class="text-center">Action</th>
              <th width="80px" scope="col" class="text-center">No</th>
              <th width="165px" scope="col">Message Code</th>
              <th scope="col">Message Description</th>
            </tr>
          </thead>
          <tbody v-if="Object.keys(this.messages).length > 0">
            <tr v-for="(message, index) in messages" :key="message.msG_CD">
              <td>
                <div class="input-group justify-content-center">
                  <!-- <RouterLink :to="{ name: 'message', params: { id: message.msG_CD } }" class="me-3">
                    <span><i class="bi bi-pencil-square" style="font-size: 1.1rem" aria-hidden="true"></i></span>
                  </RouterLink> -->

                  <a id="clickModalEdit" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#EditModal" role="button" hidden>Open Edit modal</a>
                  <span class="btn btn-sm btn-outline-success pt-0 pb-0 me-2 rounded" title="Edit" @click="onClickModalEdit(message.msG_CD, message.msG_TEXT)">
                    <i class="bi bi-pencil-square" style="font-size: 1.1rem" aria-hidden="true"></i>
                  </span>
                  <a id="clickModalDelete" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#deleteModal" role="button" hidden>Open Edit modal</a>
                  <!-- <span class="me-3" data-bs-toggle="modal" data-bs-target="#EditModal" style="cursor: pointer" title="Edit"><i class="bi bi-pencil-square text-success" style="font-size: 1.1rem" aria-hidden="true"></i></span> -->
                  <span class="btn btn-sm btn-outline-danger pt-0 pb-0 me-2 rounded" title="Delete" @click="onClickModalDelete(message.msG_CD)">
                    <i class="bi bi-trash3" style="font-size: 1.1rem" aria-hidden="true"></i>
                  </span>
                  <!-- <RouterLink :to="{ name: 'message' }">
                    <span class="btn btn-outline-danger pt-0 pb-0" title="Delete"><i class="bi bi-trash3" aria-hidden="true"></i></span>
                  </RouterLink> -->
                </div>
              </td>
              <td class="text-center">{{ message.seq }}</td>
              <td>{{ message.msG_CD }}</td>
              <td>{{ message.msG_TEXT }}</td>
            </tr>
          </tbody>
          <tbody v-else>
            <tr>
              <td colspan="3">No Data</td>
            </tr>
          </tbody>
        </table>
      </div>
      <nav aria-label="Page navigation example">
        <div class="container">
          <ul class="pagination justify-content-center">
            <li class="page-item disabled">
              <a class="page-link" href="#" aria-label="Previous">
                <span aria-hidden="true">&laquo;</span>
              </a>
            </li>
            <li class="page-item" v-for="index in totalPage" :key="index">
              <a class="page-link" :class="`${active ? 'active' : ''}`" href="#" @click="searchPage(index)">{{ index }}</a>
            </li>
            <li class="page-item disabled">
              <a class="page-link" href="#" aria-label="Next">
                <span aria-hidden="true">&raquo;</span>
              </a>
            </li>
            <!-- <li class="page-item">
            <a class="page-link" href="#" aria-label="Previous">
              <span aria-hidden="true">&laquo;</span>
            </a>
          </li>
          <li class="page-item"><a class="page-link" href="#">1</a></li>
          <li class="page-item"><a class="page-link" href="#">2</a></li>
          <li class="page-item"><a class="page-link" href="#">3</a></li>
          <li class="page-item">
            <a class="page-link" href="#" aria-label="Next">
              <span aria-hidden="true">&raquo;</span>
            </a>
          </li> -->
          </ul>
        </div>
      </nav>
    </div>
    <!-- </div> -->
    <!-- </div> -->
  </main>
</template>

<!-- <script setup>
const myModal = document.getElementById("onClickModalEdit").click();

const myModal = document.getElementById("EditModal");
const onEditModal = () => {
  console.log("a");
  myModal.addEventListener("shown.bs.modal", function () {
    console.log("j");
  });
};

</script> -->
<script>
import Sidebar from "../components/Sidebar.vue";
import axios from "axios";
//import { defineForm, field, isValidForm, toObject } from "vue-yup-form";
//import * as Yup from "yup";
import { useToast } from "vue-toastification";

// const generateFormModal = () => {
//   const msgCode = field("", Yup.string().required("Message Code is required field").min(3, "Message Code must be at least 3 characters").max(10, "Message Code more than 10 characters"));
//   const msgDesc = field("", Yup.string().required("Message Desc is required field").min(3, "Message Desc must be at least 3 characters").max(200, "Message Code more than 10 characters"));
//   return defineForm({
//     msgCode,
//     msgDesc,
//   });
// };

export default {
  name: "message",
  components: {
    Sidebar,
  },
  data() {
    return {
      token: "",
      //validToken: false,
      totalItems: 1,
      expire: "",
      refreshToken: "",
      active: false,
      search: "",
      action: "",
      pageSize: 5,
      page: 1,
      currentPage: 1,
      totalPage: 1,
      messages: [],
      msgCode: "",
      msgDesc: "",
      isDisabled: false,
      screenModal: "",
    };
  },
  setup() {
    // Get toast interface
    const toast = useToast();
    //let messages = ref([]);
    //const form = generateFormModal();

    return { toast };
  },
  beforeMount() {
    this.token = this.$cookies.get("user").token;
    this.expire = this.$cookies.get("user").expireDate;
    this.refreshToken = this.$cookies.get("user").refreshToken;
    this.onCheckExpire();
  },
  mounted() {},
  methods: {
    SetMessages(data) {
      this.messages = data;
      //console.log(this.messages);
      //console.log(Object.keys(this.messages).length);
    },
    onCheckExpire() {
      const currentDate = new Date();
      const expireDateInt = new Date(this.expire);
      this.refreshToken = this.$cookies.get("user").refreshToken;
      // if (parseInt(this.expire) * 1000 < currentDate.getTime()) {
      if (expireDateInt.getTime() < currentDate.getTime()) {
        //console.log("masuk expire");
        this.onRefreshToken("onReady");
        //console.log("a " + this.validToken);
      } else {
        this.onSearch();
      }
    },
    onClickClear() {
      this.search = "";
      this.onCheckExpire();
    },
    onRefreshToken(action) {
      try {
        axios
          .post(`${import.meta.env.VITE_APP_BASE_API_URL}/account/refresh`, {
            RefreshToken: this.refreshToken,
          })
          .then((response) => {
            if (response.data.status) {
              const user = response.data.data;

              this.$cookies.remove("user");

              this.$cookies.set("user", user, { httpOnly: true });
              this.token = this.$cookies.get("user").token;
              this.expire = this.$cookies.get("user").expireDate;
              this.refreshToken = this.$cookies.get("user").refreshToken;
              //console.log("a " + action);
              if (action == "onReady") {
                this.onSearch();
              } else if (action == "onById") {
                this.onSearchById();
              } else if (action == "onByPage") {
                this.page = this.currentPage;
                this.onSearch();
              } else if (action == "onSaveRefresh") {
                this.onSave();
              } else if (action == "onDeleteData") {
                this.onModalDeleteData();
              }
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response);
            } else if (error.request) {
              console.log("Error: Network Error");
            } else {
            }
            //main.isLoading(false);
          })
          .finally(() => {
            //this.$isLoading(false); // hide loading screen
          });
      } catch (error) {}
    },
    onSearch() {
      try {
        axios
          .get(`${import.meta.env.VITE_APP_BASE_API_URL}/message?size=${this.pageSize}&page=${this.page}`, {
            headers: {
              Authorization: `Bearer ${this.token}`,
            },
          })
          .then((response) => {
            if (response.data.status) {
              const listMessages = response.data.data.list;

              this.SetMessages(listMessages);
              //console.log("total " + Math.ceil(response.data.data.total / 5));
              this.totalItems = response.data.data.total;
              this.totalPage = Math.ceil(this.totalItems / 5);
              //console.log(this.messages);
            } else {
              this.SetMessages([]);
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response.data.title);
            }
          })
          .finally(() => {});
      } catch (error) {}
    },
    onCheckSearchById() {
      const currentDate = new Date();
      const expireDateInt = new Date(this.expire);
      this.refreshToken = this.$cookies.get("user").refreshToken;
      // if (parseInt(this.expire) * 1000 < currentDate.getTime()) {
      if (this.search.length < 1) {
        this.onCheckExpire();
      } else {
        if (expireDateInt.getTime() < currentDate.getTime()) {
          //console.log("masuk expire");
          this.onRefreshToken("onById");
          //console.log("a " + this.validToken);
        } else {
          this.onSearchById();
        }
      }
    },
    onSearchById() {
      try {
        axios
          .get(`${import.meta.env.VITE_APP_BASE_API_URL}/message/${this.search}`, {
            headers: {
              Authorization: `Bearer ${this.token}`,
            },
          })
          .then((response) => {
            if (response.data.status) {
              const listMessages = response.data.data;
              //console.log("total " + [listMessages].length);
              this.SetMessages([listMessages]);
              this.totalPage = 1;
              //console.log(this.messages);
            } else {
              this.SetMessages([]);
              this.totalPage = 1;
            }
          })
          .catch((error) => {
            if (error.response) {
              console.log(error.response.data.title);
            }
          })
          .finally(() => {});
      } catch (error) {}
    },
    searchPage(pg) {
      //console.log("this page " + pg);
      this.search = "";
      this.currentPage = pg;
      //this.active = true;
      const currentDate = new Date();
      const expireDateInt = new Date(this.expire);
      this.refreshToken = this.$cookies.get("user").refreshToken;
      if (expireDateInt.getTime() < currentDate.getTime()) {
        //console.log("masuk expire");
        this.onRefreshToken("onByPage");
        //console.log("a " + this.validToken);
      } else {
        this.page = pg;
        this.onSearch();
      }
    },
    onClickModalEdit(msgCode, msgDesc) {
      //console.log("ab " + msgCode);
      this.screenModal = "Edit";
      this.msgCode = msgCode;
      this.msgDesc = msgDesc;
      this.isDisabled = true;
      document.getElementById("clickModalEdit").click();
    },
    onClickModalAdd() {
      this.screenModal = "Add";
      this.isDisabled = false;
      this.msgCode = "";
      this.msgDesc = "";
      document.getElementById("clickModalEdit").click();
    },
    onBeforeSave() {
      const currentDate = new Date();
      const expireDateInt = new Date(this.expire);
      this.refreshToken = this.$cookies.get("user").refreshToken;
      if (expireDateInt.getTime() < currentDate.getTime()) {
        //console.log("masuk expire");
        this.onRefreshToken("onSaveRefresh");
        //console.log("a " + this.validToken);
      } else {
        this.onSave();
      }
    },
    onSave() {
      if (this.screenModal == "Add") {
        //console.log("on add");
        // if (!isValidForm(this.form)) {
        //   //this.$isLoading(false);
        //   //console.log(JSON.stringify(toObject(this.form), null, 2));
        //   return;
        // }
        this.$isLoading(true); // show loading screen
        const data = {
          MsgCode: this.msgCode,
          MsgText: this.msgDesc,
        };
        try {
          axios
            .post(`${import.meta.env.VITE_APP_BASE_API_URL}/message`, data, {
              headers: {
                Authorization: `Bearer ${this.token}`,
              },
            })
            .then((response) => {
              if (response.data.status) {
                this.toast.success(response.data.message);
              } else {
                this.toast.error(response.data.message);
              }
            })
            .catch((error) => {
              if (error.response) {
                this.toast.error(error.response.data.title);
              } else if (error.request) {
                this.toast.error("Error: Network Error");
              } else {
              }
            })
            .finally(() => {
              this.$isLoading(false); // hide loading screen
              this.onCloseModal();
              this.onCheckExpire();
            });
        } catch (error) {
          this.toast.error(error.message);
          this.$isLoading(false); // hide loading screen
        }
      } else if (this.screenModal == "Edit") {
        //console.log("on edit");
        this.$isLoading(true); // show loading screen
        const data = {
          MsgText: this.msgDesc,
        };
        try {
          axios
            .put(`${import.meta.env.VITE_APP_BASE_API_URL}/message/${this.msgCode}`, data, {
              headers: {
                Authorization: `Bearer ${this.token}`,
              },
            })
            .then((response) => {
              if (response.data.status) {
                this.toast.success(response.data.message);
              } else {
                this.toast.error(response.data.message);
              }
            })
            .catch((error) => {
              if (error.response) {
                this.toast.error(error.response.data.title);
              } else if (error.request) {
                this.toast.error("Error: Network Error");
              } else {
              }
            })
            .finally(() => {
              this.$isLoading(false); // hide loading screen
              this.onCloseModal();
              this.onCheckExpire();
            });
        } catch (error) {
          this.toast.error(error.message);
          this.$isLoading(false); // hide loading screen
        }
        //this.onCloseModal();
      } else {
        alert("no have screen");
        this.onCloseModal();
      }
    },
    onCloseModal() {
      document.getElementById("closeModal").click();
    },
    onCloseModalDelete() {
      document.getElementById("closeDelete").click();
    },
    onClickModalDelete(msgCode) {
      this.msgCode = msgCode;
      this.isDisabled = true;
      document.getElementById("clickModalDelete").click();
    },
    onCheckDelete() {
      const currentDate = new Date();
      const expireDateInt = new Date(this.expire);
      this.refreshToken = this.$cookies.get("user").refreshToken;
      if (expireDateInt.getTime() < currentDate.getTime()) {
        //console.log("masuk expire");
        this.onRefreshToken("onDeleteData");
        //console.log("a " + this.validToken);
      } else {
        this.onModalDeleteData();
      }
    },
    onModalDeleteData() {
      //console.log("on delete");
      this.$isLoading(true); // show loading screen

      try {
        axios
          .delete(`${import.meta.env.VITE_APP_BASE_API_URL}/message/${this.msgCode}`, {
            headers: {
              Authorization: `Bearer ${this.token}`,
            },
          })
          .then((response) => {
            if (response.data.status) {
              this.toast.success(response.data.message);
            } else {
              this.toast.error(response.data.message);
            }
          })
          .catch((error) => {
            if (error.response) {
              this.toast.error(error.response.data.title);
            } else if (error.request) {
              this.toast.error("Error: Network Error");
            } else {
            }
          })
          .finally(() => {
            this.$isLoading(false); // hide loading screen
            this.onCloseModalDelete();
            this.onCheckExpire();
          });
      } catch (error) {
        this.toast.error(error.message);
        this.$isLoading(false); // hide loading screen
      }
    },
  },
};
</script>

<style></style>
