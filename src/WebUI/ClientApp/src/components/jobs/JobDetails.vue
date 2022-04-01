<script setup lang="ts">
import {PropType, toRefs} from "vue";
import {BriefJobDto} from "@/api/clients";
import {display} from "@/utilities/display";

const props = defineProps({
  job: {
    type: Object as PropType<BriefJobDto>,
    required: true
  }
});

const {title, locationTitle, start, end, description, assignmentCount} = toRefs(props.job)
</script>

<template>
  <el-descriptions :title="title" :column="2">
    <el-descriptions-item label="Location">{{ locationTitle }}</el-descriptions-item>
      <el-descriptions-item label="Staff">{{ assignmentCount }}</el-descriptions-item>
      <el-descriptions-item label="Start">{{ display.date(start) }}</el-descriptions-item>
      <el-descriptions-item label="End">{{ display.date(end) }}</el-descriptions-item>
    <el-descriptions-item label="Description" :span="2">{{ description }}</el-descriptions-item>
  </el-descriptions>
</template>