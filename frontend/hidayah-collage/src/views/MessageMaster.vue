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
            <button type="button" class="btn btn-danger btn-sm" @click="onModalDeleteData">Yes</button>
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
              <form ref="formSubmit" action="#" class="needs-validation" novalidate>
                <div class="row mb-2">
                  <div class="col-sm-4">
                    <label for="" class="col-form-label">Message Code</label>
                  </div>
                  <div class="col-sm-7">
                    <input
                      autocomplete="off"
                      type="text"
                      ref="msgCode"
                      id="msgCode"
                      name="msgCode"
                      style="text-transform: uppercase"
                      class="form-control"
                      :class="{ 'is-invalid': isSubmitted && !!errorForm.msgCode }"
                      v-model="formAddEdit.msgCode"
                      @blur="validate('msgCode')"
                      @keypress="validate('msgCode')"
                      :disabled="isDisabled"
                      maxlength="10"
                    />
                    <div class="invalid-feedback">{{ errorForm.msgCode }}</div>
                  </div>
                </div>
                <div class="row">
                  <div class="col-sm-4">
                    <label for="" class="col-form-label">Message Desc</label>
                  </div>
                  <div class="col-sm-7">
                    <textarea
                      rows="3"
                      ref="msgDesc"
                      id="msgDesc"
                      name="msgDesc"
                      v-model="formAddEdit.msgDesc"
                      type="text"
                      class="form-control"
                      :class="{ 'is-invalid': isSubmitted && !!errorForm.msgDesc }"
                      @blur="validate('msgDesc')"
                      @keypress="validate('msgDesc')"
                      maxlength="100"
                    />
                    <div class="invalid-feedback">{{ errorForm.msgDesc }}</div>
                  </div>
                </div>
              </form>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" id="closeModal">Close</button>
            <button type="button" class="btn btn-primary" @click="onSave">Save</button>
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
        <input type="text" ref="search" class="form-control" placeholder="search msg code" aria-label="First name" v-model="search" @keyup="onSearchById" />
      </div>
      <div class="col-md-3">
        <button type="button" class="btn btn-warning" @click="onClickClear">Clear</button>
      </div>
      <div class="col-md-6 d-grid justify-content-md-end">
        <button type="button" class="btn btn-primary" @click="onClickModalAdd">Add</button>
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
              <th scope="col">Message Code</th>
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
              <td class="text-truncate" style="max-width: 165px" data-toggle="tooltip" data-placement="left" :title="message.msG_TEXT">{{ message.msG_TEXT }}</td>
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
        <div class="row g-3">
          <div class="col-sm-3"></div>
          <div class="col-sm-6">
            <ul class="pagination justify-content-center">
              <li class="page-item" :class="`${activeFirst ? 'active disabled' : ''}`">
                <a class="page-link" href="#" aria-label="Previous" @click="searchPage('first')">
                  <span aria-hidden="true">&laquo;</span>
                </a>
              </li>
              <!-- <li class="page-item" :class="`${active ? 'active' : ''}`" v-for="index in totalPage" :key="index"> -->
              <li class="page-item" :class="isActive.includes(index) && 'active disabled'" v-for="index in totalPage" :key="index">
                <a class="page-link" href="#" @click="searchPage(index)">{{ index }}</a>
              </li>
              <li class="page-item" :class="`${activeLast ? 'active disabled' : ''}`">
                <a class="page-link" href="#" aria-label="Next" @click="searchPage('last')">
                  <span aria-hidden="true">&raquo;</span>
                </a>
              </li>
            </ul>
          </div>
          <div class="col-sm-3">
            <div class="form-group d-flex justify-content-md-end">
              <label class="col-form-label" for="">Page Size : </label>
              <select class="page-link form-select-sm ms-2" @change="onChangePageSize" style="color: dodgerblue">
                <option selected value="5">5</option>
                <option value="10">10</option>
                <option value="15">15</option>
              </select>
            </div>
          </div>
        </div>
      </nav>
    </div>
    <!-- </div> -->
    <!-- </div> -->
  </main>
</template>

<script>
import Sidebar from "../components/Sidebar.vue";
import * as Yup from "yup";
import { useToast } from "vue-toastification";
import axiosinstance from "../services/axiosinstance";

const formSchemaValidation = Yup.object().shape({
  msgCode: Yup.string().min(3, "Message Code must be at least 3 characters").max(10, "Message Code more than 10 characters").required("Message Code is required field"),
  msgDesc: Yup.string().min(3, "Message Desc must be at least 3 characters").max(200, "Message Code more than 10 characters").required("Message Desc is required field"),
});

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
      isSubmitted: false,
      expire: "",
      refreshToken: "",
      active: true,
      activeFirst: false,
      activeLast: false,
      isActive: [],
      search: "",
      action: "",
      pageSize: 5,
      page: 1,
      currentPage: 1,
      totalPage: 1,
      messages: [],
      isDisabled: false,
      screenModal: "",
      formAddEdit: {
        msgCode: "",
        msgDesc: "",
      },
      errorForm: {
        msgCode: "",
        msgDesc: "",
      },
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
    // this.token = this.$cookies.get("user").token;
    // this.expire = this.$cookies.get("user").expireDate;
    // this.refreshToken = this.$cookies.get("user").refreshToken;
    this.searchPage(this.page);
  },
  mounted() {
    this.$refs.search.focus();
  },
  methods: {
    axiosInt: function () {
      var v = this;
      setTimeout(function () {
        v.$nextTick(() => v.$refs.msgCode.focus());
      }, 500);
    },
    SetMessages(data) {
      this.messages = data;
    },
    onClickClear() {
      this.search = "";
      this.page = 1;
      this.searchPage(this.page);
    },
    onChangePageSize(event) {
      this.pageSize = event.target.value;
      this.page = 1;
      this.searchPage(this.page);
    },
    onSearch() {
      try {
        axiosinstance
          .get(`/message?size=${this.pageSize}&page=${this.page}`)
          .then((response) => {
            if (response.data.status) {
              const listMessages = response.data.data.list;

              this.SetMessages(listMessages);
              //console.log("total " + Math.ceil(response.data.data.total / 5));
              this.totalItems = response.data.data.total;
              this.totalPage = Math.ceil(this.totalItems / this.pageSize);
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
    onSearchById() {
      if (this.search.length < 1) {
        this.onSearch();
      } else {
        try {
          axiosinstance
            .get(`/message/getByCode/${this.search}`)
            .then((response) => {
              if (response.data.status) {
                const listMessages = response.data.data.list;

                this.SetMessages(listMessages);
                this.totalItems = response.data.data.total;
                this.totalPage = Math.ceil(this.totalItems / this.pageSize);
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
      }
    },
    searchPage(pg) {
      //console.log("this page " + pg);

      //this.active = true;
      this.isActive = [];
      this.activeFirst = false;
      this.activeLast = false;
      if (this.isActive.includes(pg)) {
        this.isActive = this.isActive.filter((s) => s !== pg);
      } else {
        this.isActive.push(pg);
      }

      this.search = "";
      if (pg == "first") {
        this.currentPage = 1;
        this.activeFirst = true;
      } else if (pg == "last") {
        this.currentPage = this.totalPage;
        this.activeLast = true;
      } else {
        this.currentPage = pg;
      }

      if (pg == "first") {
        this.page = 1;
      } else if (pg == "last") {
        this.page = this.totalPage;
      } else {
        this.page = pg;
      }
      this.onSearch();
      //}
    },
    onClickModalEdit(msgCode, msgDesc) {
      //console.log("ab " + msgCode);
      this.screenModal = "Edit";
      this.formAddEdit.msgCode = msgCode;
      this.formAddEdit.msgDesc = msgDesc;
      this.isDisabled = true;
      this.isSubmitted = false;
      document.getElementById("clickModalEdit").click();
      this.onFocusMsgDesc();
    },
    onClickModalAdd() {
      //this.$refs.msgCode.focus();
      this.screenModal = "Add";
      this.isDisabled = false;
      this.formAddEdit.msgCode = "";
      this.formAddEdit.msgDesc = "";
      this.isSubmitted = false;
      document.getElementById("clickModalEdit").click();
      this.onFocusModal();
    },
    onFocusModal: function () {
      var v = this;
      setTimeout(function () {
        v.$nextTick(() => v.$refs.msgCode.focus());
      }, 500);
    },
    onFocusMsgDesc: function () {
      var v = this;
      setTimeout(function () {
        v.$nextTick(() => v.$refs.msgDesc.focus());
      }, 500);
    },
    validate(field) {
      formSchemaValidation
        .validateAt(field, this.formAddEdit)
        .then(() => (this.errorForm[field] = ""))
        .catch((err) => {
          this.errorForm[err.path] = err.message;
        });
    },
    onSave(e) {
      e.preventDefault();
      this.isSubmitted = true;
      if (this.screenModal == "Add") {
        formSchemaValidation
          .validate(this.formAddEdit, { abortEarly: false })
          .then(() => {
            this.$isLoading(true); // show loading screen
            const data = {
              MsgCode: this.formAddEdit.msgCode.toUpperCase(),
              MsgText: this.formAddEdit.msgDesc,
            };
            try {
              axiosinstance
                .post(`/message`, data)
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
                  this.onSearch();
                  this.onResetForm();
                });
            } catch (error) {
              this.toast.error(error.message);
              this.$isLoading(false); // hide loading screen
            }
          })
          .catch((err) => {
            err.inner.forEach((error) => {
              this.errorForm = { ...this.errorForm, [error.path]: error.message };
              //console.log("err " + this.errorForm.msgCode);
            });
            //console.log("err " + this.errorForm.msgCode);
          });
      } else if (this.screenModal == "Edit") {
        formSchemaValidation
          .validate(this.formAddEdit, { abortEarly: false })
          .then(() => {
            this.$isLoading(true); // show loading screen
            const data = {
              MsgText: this.formAddEdit.msgDesc,
            };
            try {
              axiosinstance
                .put(`/message/${this.formAddEdit.msgCode}`, data)
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
                  this.onSearch();
                  this.onResetForm();
                });
            } catch (error) {
              this.toast.error(error.message);
              this.$isLoading(false); // hide loading screen
            }
          })
          .catch((err) => {
            err.inner.forEach((error) => {
              this.errorForm = { ...this.errorForm, [error.path]: error.message };
            });
          });
      } else {
        alert("no have screen");
        this.onCloseModal();
      }
    },
    onResetForm() {
      var self = this; //you need this because *this* will refer to Object.keys below`

      setTimeout(function () {
        self.$refs.formSubmit.reset();
      }, 500);
    },
    onCloseModal() {
      document.getElementById("closeModal").click();
    },
    onCloseModalDelete() {
      document.getElementById("closeDelete").click();
    },
    onClickModalDelete(msgCode) {
      this.formAddEdit.msgCode = msgCode;
      this.isDisabled = true;
      document.getElementById("clickModalDelete").click();
    },
    onModalDeleteData() {
      //console.log("delete " + `Bearer ${TokenService.getTokenAccess()}`);
      axiosinstance
        .delete(`/message/${this.formAddEdit.msgCode}`)
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
            console.log("catch");
          }
        })
        .finally(() => {
          this.$isLoading(false); // hide loading screen
          this.onCloseModalDelete();
          this.page = 1;
          this.searchPage(this.page);
        });
    },
  },
};
</script>

<style setup></style>
