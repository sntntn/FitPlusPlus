<template>
  <CHeader position="sticky" class="mb-4">
    <CContainer fluid>
      <CHeaderToggler class="ps-1">
      </CHeaderToggler>
      <CHeaderBrand class="mx-auto d-lg-none" to="/">
      </CHeaderBrand>
      <CHeaderNav class="align-items-center">
        <AppHeaderDropdownNotif />
        <AppHeaderDropdownAccnt />
      </CHeaderNav>
    </CContainer>
  </CHeader>
</template>

<script>
import AppHeaderDropdownAccnt from './AppHeaderDropdownAccnt'
import AppHeaderDropdownNotif from './AppHeaderDropdownNotif'

export default {
  name: 'AppHeader',
  components: {
    AppHeaderDropdownAccnt,
    AppHeaderDropdownNotif,
  },
}
</script>
