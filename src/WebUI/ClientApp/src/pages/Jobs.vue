<script setup lang="ts">
import RvPageTitle from "@/components/general/PageTitle.vue";
import {onMounted, readonly, ref} from "vue";
import {BriefJobDto, JobClient} from "@/api/clients";
import {store} from "@/store";

const jobClient = readonly(new JobClient());

const jobs = ref<BriefJobDto[]>([]);

onMounted(async () => (jobs.value = await jobClient.get()));
</script>

<template>
  <rv-page-title title="Jobs" />

  <article v-for="({title, location}, i) in jobs">
      <h3>{{title}}</h3>

      <p><b>Latitude: </b>{{ location.latitude }}</p>
      <p><b>Longitude: </b>{{ location.longitude }}</p>
      
      <el-divider v-if="i !== jobs.length - 1" />
  </article>
  
  <el-empty v-if="!jobs.length" description="No Jobs found."/>
</template>
c