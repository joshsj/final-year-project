<script setup lang="ts">
import PageTitle from "@/components/general/PageTitle.vue";
import JobDetails from "@/components/jobs/JobDetails.vue";
import {onMounted} from "vue";
import {store} from "@/store";
import {useRouter} from "vue-router";
import {route} from "@/router";

const {push} = useRouter();

onMounted(store.jobs.fetch);
</script>

<template>
  <page-title title="Jobs"/>

  <article
      v-for="(job, i) in store.jobs.items"
      :key="job.id">
    <job-details :job="job" :use-title="true"/>

    <el-button
        round
        type="success"
        @click="push(route({ name: 'job', jobId: job.id }))">
      View
    </el-button>

    <el-divider v-if="i !== store.jobs.items.length - 1"/>
  </article>

  <el-empty v-if="!store.jobs.items.length" description="No Jobs found."/>
</template>