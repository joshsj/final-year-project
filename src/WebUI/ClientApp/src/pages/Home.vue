<script setup lang="ts">
import { useAuth0 } from "@auth0/auth0-vue";
import { ArrowDownBold } from "@element-plus/icons-vue";
import { route } from "@/router";
import { useRouter } from "vue-router";
import {useAuth} from "@/plugins/auth";

const { push } = useRouter();
const { isAuthenticated } = useAuth0();
const { login, logout} = useAuth();
</script>

<template>
  <header>
    <h1 style="margin: 0">RendezVous</h1>

    <nav>
      <template v-if="isAuthenticated">
        <el-button type="primary" round @click="push(route({ name: 'jobs' }))">
          Jobs
        </el-button>

        <el-dropdown trigger="click">
          <el-button type="primary" round>
            Account
            <el-icon class="right"><arrow-down-bold /></el-icon>
          </el-button>

          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item @click="push(route({ name: 'account' }))">
                View
              </el-dropdown-item>
              <el-dropdown-item divided @click="logout">
                Logout
              </el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </template>

      <el-button v-else type="primary" round @click="login">Login</el-button>
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
    margin-right: 1rem;
  }
}
</style>
