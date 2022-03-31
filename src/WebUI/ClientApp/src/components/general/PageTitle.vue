<script setup lang="ts">
import { Back } from "@element-plus/icons-vue";
import { useRouter } from "vue-router";
import { route } from "@/router";

const { push } = useRouter();

defineProps({
  title: { type: String, required: true },
});
</script>

<template>
  <header>
    <el-row justify="space-between" align="middle">
      <el-row align="middle">
        <el-button
          id="back"
          type="primary"
          :icon="Back"
          circle
          @click="push(route({ name: 'home' }))" />

        <h2>{{ title }}</h2>
      </el-row>

      <aside><slot /></aside>
    </el-row>
  </header>
</template>

<style scoped>
header {
  margin-bottom: 1.5rem;
}

h2 {
  margin: 0;
}

#back {
  margin-right: 1rem;
}
</style>
