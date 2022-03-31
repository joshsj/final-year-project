<script setup lang="ts">
import PageTitle from "@/components/general/PageTitle.vue";
import JobDetails from "@/components/jobs/JobDetails.vue";
import {readonly} from "vue";
import {store} from "@/store";

const props = defineProps({jobId: {type: String, required: true}});

const job = readonly(store.jobs.items.find(x => x.id === props.jobId)!);
</script>

<template>
  <page-title title="Job"/>
  
  <job-details :job="job"/>
  
  <el-collapse>
      <el-collapse-item>
          <template #title>
              <span><b>Assignees</b> ({{ job.assignmentCount }})</span>
          </template>
          
          TODO
      </el-collapse-item>
  </el-collapse>
</template>
