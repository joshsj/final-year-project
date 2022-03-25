<script setup lang="ts">
import { store } from "@/store";
import { useAuth0 } from "@auth0/auth0-vue";
import { ArrowDownBold } from "@element-plus/icons-vue";

const {
  user,
  isAuthenticated,
  loginWithRedirect,
  logout: _logout,
} = useAuth0();

const logout = () => {
  // indicate loading before page refresh
  store.page.loading = true;
  _logout({ returnTo: location.origin });
};
</script>

<template>
  <header>
    <h1 style="margin: 0">RendezVous</h1>

    <nav>
      <template v-if="isAuthenticated">
        <el-button type="primary" round>Jobs</el-button>

        <el-dropdown trigger="click">
          <el-button type="primary" round>
            {{ user?.given_name ?? "Account" }}
            <el-icon class="right"><arrow-down-bold /></el-icon>
          </el-button>

          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item>View</el-dropdown-item>
              <el-dropdown-item @click="logout">Logout</el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </template>

      <el-button v-else type="primary" @click="loginWithRedirect">
        Login
      </el-button>
    </nav>
  </header>
</template>

<style scoped lang="scss">
header {
  text-align: center;
}

nav {
  margin-top: 0.5rem;

  & > *:not(:last-child) {
    margin-right: 0.5rem;
  }
}
</style>
