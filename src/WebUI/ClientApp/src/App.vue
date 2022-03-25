<script setup lang="ts">
import { store } from "@/store";
</script>

<template>
  <el-row
    id="root"
    justify="center"
    align="middle"
    v-loading="store.page.loading">
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
