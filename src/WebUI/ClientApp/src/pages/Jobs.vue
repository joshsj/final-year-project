<script setup lang="ts">
import RvPageTitle from "@/components/general/PageTitle.vue";
import {onMounted, readonly, ref} from "vue";
import {BriefJobDto, JobClient} from "@/api/clients";
import {display} from "@/utilities/display";
import {store} from "@/store";

const jobClient = readonly(new JobClient());

const jobs = ref<BriefJobDto[]>([]);

onMounted(async () => (jobs.value = await store.page.load(() => jobClient.get())));
</script>

<template>
  <rv-page-title title="Jobs"/>

  <article v-for="({title, locationTitle, start, end, description, assignmentCount}, i) in jobs" :key="start">
    <el-descriptions :title="title" :column="2">
        <el-descriptions-item label="Location">{{ locationTitle }}</el-descriptions-item>
        <el-descriptions-item label="Start">{{ display.date(start) }}</el-descriptions-item>
        <el-descriptions-item label="End">{{ display.date(end) }}</el-descriptions-item>
        <el-descriptions-item label="Staff">{{ assignmentCount }}</el-descriptions-item>
        
        <el-descriptions-item label="Description" :span="2">{{ description }}</el-descriptions-item>
    </el-descriptions>
      
    <el-divider v-if="i !== jobs.length - 1"/>
  </article>

  <el-empty v-if="!jobs.length" description="No Jobs found."/>
</template>
