<script setup lang="ts">
defineProps({
  title: { type: String, required: true },
});
</script>

<template>
  <el-row justify="space-between" align="bottom">
    <h2>{{ title }}</h2>

    <aside></aside>
  </el-row>
</template>

<style scoped>
h2 {
  margin: 0;
}
</style>
