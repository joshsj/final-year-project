<script setup lang="ts">
import { store } from "@/store";
import { useAuth0 } from "@auth0/auth0-vue";
import { watch } from "vue";

const {
  user,
  isAuthenticated,
  isLoading,
  loginWithRedirect,
  logout: _logout,
} = useAuth0();

const logout = () => _logout({ returnTo: location.origin });

watch(isLoading, (x) => (store.page.loading = x));
</script>

<template>
  <el-row v-loading="store.page.loading">
    <el-col :sm="{ span: 14, offset: 5 }" :md="{ span: 10, offset: 7 }">
      <header>
        <el-row justify="center"><h1>RendezVous</h1></el-row>

        <el-row justify="center">
          <template v-if="isAuthenticated">
            <el-dropdown trigger="click">
              <el-button type="primary">
                {{ user?.given_name ?? "Account" }}
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
        </el-row>
      </header>

      <main v-if="isAuthenticated">
        <router-view />
      </main>
    </el-col>
  </el-row>
</template>

<style scoped lang="scss"></style>
