<script lang="ts" setup>
import PageTitle from "@/components/general/PageTitle.vue";
import JobDetails from "@/components/jobs/JobDetails.vue";
import {computed, onMounted, readonly} from "vue";
import {store} from "@/store";
import {display} from "@/utilities/display";
import {useAuth0} from "@auth0/auth0-vue";
import {AssignmentDto, ClockType} from "@/api/clients";
import {useRouter} from "vue-router";
import {route} from "@/router";

const props = defineProps({jobId: {type: String, required: true}});

const {user} = useAuth0();
const {push} = useRouter();

const isUserAssigment = ({employeeProviderId: id}: AssignmentDto): boolean => id === user.value.sub;

const job = readonly(store.jobs.items.find(x => x.id === props.jobId)!);

const userAssignment = computed(() => job.assignments?.find(isUserAssigment));

const clock = (type: ClockType) => userAssignment.value && push(route({
  name: 'clock',
  assignmentId: userAssignment.value.id,
  type
}));

onMounted(() => store.jobs.fetchAssignments(props.jobId));
</script>

<template>
  <page-title title="Job"/>

  <job-details :job="job"/>

  <!-- only fetch assignments once  -->
  <el-collapse v-if="job.assignmentCount">
    <el-collapse-item title="Staff">
      <el-table :data="job.assignments">
        <el-table-column
            #="{row: assignment}"
            label="Name">
          {{ isUserAssigment(assignment) ? "You" : assignment.employeeName }}
        </el-table-column>

        <el-table-column
            :formatter="({}, {}, value) => display.bool(value)"
            label="Clock In"
            prop="hasClockedIn"/>

        <el-table-column
            :formatter="({}, {}, value) => display.bool(value)"
            label="Clock Out"
            prop="hasClockedOut"/>

        <el-table-column label="Actions">
          <el-button
              v-if="!userAssignment?.hasClockedIn"
              round
              size="small"
              type="success"
              @click="clock(ClockType.In)">
            Clock In
          </el-button>

          <el-button
              v-if="userAssignment?.hasClockedIn && !userAssignment?.hasClockedOut"
              round
              size="small"
              type="success"
              @click="clock(ClockType.Out)">
          Clock Out
          </el-button>
        </el-table-column>
      </el-table>
    </el-collapse-item>
  </el-collapse>
</template>
