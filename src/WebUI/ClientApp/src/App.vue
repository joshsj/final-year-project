<script setup lang="ts">
import { store } from "@/store";
import { useAuth0 } from "@auth0/auth0-vue";
import {useAccessTokenBehavior, useAuth} from "@/plugins/auth";

const { isLoading, isAuthenticated } = useAuth0();

useAccessTokenBehavior();
</script>

<template>
  <el-row
    id="root"
    justify="center"
    align="middle"
    v-loading="store.page.loading || isLoading">
    <el-col :span="23" :sm="16" :md="12">
      <router-view />
    </el-col>
  </el-row>
</template>

<style scoped>
#root {
  min-height: 100vh;
}
</style>
