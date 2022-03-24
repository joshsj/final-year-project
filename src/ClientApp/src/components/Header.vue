<script setup lang="ts">
defineProps({
  title: { type: String, required: true },
});
</script>

<template>
  <h2>{{ title }}</h2>
</template>
