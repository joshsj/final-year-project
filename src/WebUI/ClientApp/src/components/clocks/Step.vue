<script setup lang="ts">
import {PropType} from "vue";

defineProps({
  title: {type: String, required: true},
  state: Boolean,
  errorMessages: Array as PropType<string[]>,
});

const emit = defineEmits(["retry"]);

</script>

<template>
  <el-row justify="space-between" align="middle">
    <h2>{{ title }}</h2>

    <span>{{ state ? "✔️" : "❌" }}</span>
  </el-row>

  <div>
    <slot/>
  </div>  

  <el-alert v-if="errorMessages?.length" type="error" :closable="false">
    <p v-for="msg in errorMessages" :key="msg">{{ msg }}</p>

    <el-button
        round
        size="small"
        type="primary"
        @click="emit('retry')">
      Retry
    </el-button>
  </el-alert>
</template>


<style scoped>
.el-alert {
  margin-bottom: 1rem;
}
</style>
