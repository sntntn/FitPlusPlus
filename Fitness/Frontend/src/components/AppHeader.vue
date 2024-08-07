<template>
  <CHeader position="sticky" class="mb-4">
    <CContainer fluid>
      <CHeaderToggler class="ps-1">
      </CHeaderToggler>
      <CHeaderBrand class="mx-auto d-lg-none" to="/">
      
      </CHeaderBrand>
      <CHeaderNav>
        <CNavItem>
          <CNavLink href="#">
            <CIcon class="mx-2" icon="cil-bell" size="lg" />
          </CNavLink>
        </CNavItem>
        <CNavItem>
          <CNavLink href="#">
            <CIcon class="mx-2" icon="cil-list" size="lg" />
          </CNavLink>
        </CNavItem>
        <CNavItem>
          <CNavLink href="#">
            <CIcon class="mx-2" icon="cil-envelope-open" size="lg" />
          </CNavLink>
        </CNavItem>
        <AppHeaderDropdownAccnt />
      </CHeaderNav>
    </CContainer>
  </CHeader>
</template>

<script>
import AppHeaderDropdownAccnt from './AppHeaderDropdownAccnt'
export default {
  name: 'AppHeader',
  components: {
    AppHeaderDropdownAccnt,
  },
  setup() {
    return {
    }
  }
}
</script>
