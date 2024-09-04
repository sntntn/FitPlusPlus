<template>
  <CDropdown variant="nav-item">
    <CDropdownToggle placement="bottom-end" class="py-0" :caret="false">
      <CAvatar :src="avatar" size="md" />
    </CDropdownToggle>
    <CDropdownMenu class="pt-0">
      <CDropdownItem style="cursor:pointer" @click="logout"> <CIcon icon="cil-lock-locked" /> Logout </CDropdownItem>
    </CDropdownMenu>
  </CDropdown>
</template>

<script>
import avatar from '@/assets/images/avatars/9.jpg';
import dataServices from '@/services/data_services';
export default {
  name: 'AppHeaderDropdownAccnt',
  setup() {
    return {
      avatar: avatar,
      itemsCount: 32,
    }
  },
  methods: {
    logout: function() {
      const username = sessionStorage.getItem('username');
      const refreshToken = sessionStorage.getItem('refreshToken');
      var request = {
        UserName: username,
        RefreshToken: refreshToken
      };
      dataServices.methods.logout(request)
        .then((response) => {
            this.$router.push('/login');
        })
    }
  }
}
</script>
