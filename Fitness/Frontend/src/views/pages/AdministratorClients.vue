<template>
  <div>
    <CForm>
      <CButton color="dark" class="px-4" v-on:click="addClient" style="margin-right: 973px;">Add Client</CButton>
    </CForm>
    <CTable caption="top" class="tbl" color="dark" striped>
      <CTableCaption style="font-weight: 600;">LIST OF CLIENTS</CTableCaption>
      <CTableHead>
        <CTableRow>
          <CTableHeaderCell class="test">{{ headers.id }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.name }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.surname }}</CTableHeaderCell>
          <CTableHeaderCell class="test">{{ headers.contactEmail }}</CTableHeaderCell>
          <CTableHeaderCell class="test action-column">Actions</CTableHeaderCell>
        </CTableRow>
      </CTableHead>
      <CTableBody>
        <CTableRow v-for="(client, index) in clients" :key="index">
          <CTableDataCell class="test">{{ client.id }}</CTableDataCell>
          <CTableDataCell class="test">{{ client.name }}</CTableDataCell>
          <CTableDataCell class="test">{{ client.surname }}</CTableDataCell>
          <CTableDataCell class="test">{{ client.email }}</CTableDataCell>
          <CTableDataCell class="test">
            <CButton color="light" class="px-3" style="margin: 0 10px;" v-on:click="updateClient(client.id)">
              <CIcon icon="cil-pencil" />
            </CButton>
            <CButton color="light" class="px-3" v-on:click="onDelete(client.id)">
              <CIcon icon="cil-trash" />
            </CButton>
          </CTableDataCell>
        </CTableRow>
      </CTableBody>
    </CTable>
    <GenericModal :modalData="modalData" />
  </div>
</template>

<script>
import dataServices from '@/services/data_services';
import GenericModal from '@/components/GenericModal.vue';

export default {
  name: "Clients",
  components: {
    GenericModal
  },
  data() {
    return {
      headers: {
        id: "Id",
        name: "Name",
        surname: "Surname",
        contactEmail: "Email",
      },
      clients: [],
      isBioExpanded: [],
      modalData: {
        isVisible: false,
        title: "Confirm delete",
        body: "Are you sure that you want to delete this client?",
        resolve: null,
        reject: null
      }
    };
  },
  methods: {
    addClient() {
      this.$router.push('/administrator/clients/0');
    },
    removeClient(id) {
      let loader = this.$loading.show();
      dataServices.methods.remove_client(id).then(() => {
        this.fetchClients();
        loader.hide();
      });
    },
    updateClient(id) {
      this.$router.push('/administrator/clients/' + id);
    },
    onDelete(id) {
      this.openModal().then((result) => {
        if(result) {
          dataServices.methods.get_client_by_id(id).then((response) => {
            var client = response.data;
            dataServices.methods.unregister_user(client.email).then((response) => {
              this.removeClient(id);
            })
          });
        }
        this.modalData.isVisible = false;
        this.modalData.resolve = null;
        this.modalData.reject = null;
      });
    },
    openModal() {
      return new Promise((resolve, reject) => {
        this.modalData.isVisible = true;
        this.modalData.resolve = resolve;
        this.modalData.reject = reject;
      });
    },
    fetchClients() {
      let loader = this.$loading.show();
      dataServices.methods.get_clients().then(response => {
        this.clients = response.data;
        loader.hide();
      }).catch(error => {
        console.error('Error fetching clients:', error);
        loader.hide();
      });
    }
  },
  mounted() {
    this.$parent.$parent.$parent.setUserData(null, "administrator");
    this.fetchClients();
  }
};
</script>

<style>
  #txt {
    font-size: 14px;
    color: red;
    text-align: center;
    font-family: Verdana, Geneva, Tahoma, sans-serif;
  }

  .tbl {
    width: 100%;
    border: 1px solid black;
  }

  .test {
    border: 1px solid black;
    text-align: center;
    cursor: pointer;
  }

  .action-column {
    width: 140px;
  }
</style>
