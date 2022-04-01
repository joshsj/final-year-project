<script setup lang="ts">
import PageTitle from "@/components/general/PageTitle.vue";
import JobDetails from "@/components/jobs/JobDetails.vue";
import {computed, readonly} from "vue";
import {store} from "@/store";
import {display} from "@/utilities/display";
import {useAuth0} from "@auth0/auth0-vue";
import {AssignmentDto} from "@/api/clients";

const props = defineProps({jobId: {type: String, required: true}});

const {user} = useAuth0();

const isUserAssigment = ({employeeProviderId: id}: AssignmentDto): boolean => id === user.value.sub;

const job = readonly(store.jobs.items.find(x => x.id === props.jobId)!);

const userAssignment = computed(() => job.assignments?.find(isUserAssigment));
</script>

<template>
  <page-title title="Job"/>

  <job-details :job="job"/>

  <!-- only fetch assignments once  -->
  <el-collapse @change.once="store.jobs.fetchAssignments(jobId)">
    <el-collapse-item>
      <template #title>
        <span><b>Staff</b> ({{ job.assignmentCount }})</span>
      </template>

      <el-table :data="job.assignments">
        <el-table-column
            label="Name"
            #="{row: assignment}">
          {{ isUserAssigment(assignment) ? "You" : assignment.employeeName }}
        </el-table-column>

        <el-table-column
            prop="hasClockedIn"
            label="Clocked In"
            :formatter="({}, {}, value) => display.bool(value)"/>

        <el-table-column
            prop="hasClockedOut"
            label="Clocked Out"
            :formatter="({}, {}, value) => display.bool(value)"/>

        <el-table-column label="Actions" v-if="userAssignment">
          <el-button
              type="success"
              size="small"
              round
              v-if="!userAssignment.hasClockedIn">
            Clock In
          </el-button>

          <el-button
              type="success"
              size="small"
              round
              v-if="userAssignment.hasClockedIn && !userAssignment.hasClockedOut">
            Clock Out
          </el-button>
        </el-table-column>
      </el-table>
    </el-collapse-item>
  </el-collapse>
</template>
