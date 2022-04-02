<script setup lang="ts">
import {PropType} from "vue";
import {CircleCheckFilled, CircleCloseFilled} from "@element-plus/icons-vue";

const props = defineProps({
  title: {type: String, required: true},
  state: Boolean,
  errorMessages: Array as PropType<string[]>,
});

const emit = defineEmits(["retry"]);

</script>

<template>
  <el-row justify="space-between" align="middle">
    <h2>{{ title }}</h2>

    <el-icon :color="`var(--el-color-${state ? 'success' : 'error'})`" size="1.5em">
      <component :is="state ? CircleCheckFilled : CircleCloseFilled"/>
    </el-icon>
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
