<script setup lang="ts">
import RvPageTitle from "@/components/general/PageTitle.vue";

import { EmployeeClient } from "@/api/clients";

const employeeClient = new EmployeeClient();
</script>

<template>
  <rv-page-title title="Jobs" />

  <el-button @click="employeeClient.put()">Put</el-button>
</template>
