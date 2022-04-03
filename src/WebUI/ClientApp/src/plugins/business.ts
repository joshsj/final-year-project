import {AssignmentDto} from "@/api/clients";
import {useAuth0} from "@auth0/auth0-vue";
import {computed} from "vue";
import {Job} from "@/store";

const useAssignmentBusiness = (job: Job) => {
    const {user} = useAuth0();

    const isUserAssigment = ({employeeProviderId}: AssignmentDto): boolean =>
        employeeProviderId === user.value?.sub;
    const userAssignment = computed(() => job.assignments?.find(isUserAssigment));

    const canClockIn = ({employeeProviderId, clockIn}: AssignmentDto): boolean =>
        !clockIn && isUserAssigment({employeeProviderId} as AssignmentDto);
    
    const canClockOut = ({employeeProviderId, clockIn, clockOut}: AssignmentDto): boolean =>
        !!clockIn && !clockOut && isUserAssigment({employeeProviderId} as AssignmentDto);
    
    const canConfirm = (ass : AssignmentDto) : boolean =>
        !isUserAssigment(ass)
            // user has clocked in, other user hasn't clocked out 
            ? !!userAssignment.value?.clockIn && !ass.clockOut 
            : false;
    
    const requiresConfirmation = (assignments: AssignmentDto[]) =>
        assignments.some(x => x.employeeProviderId != user.value.sub && x.clockIn);
    return {isUserAssigment, userAssignment, canClockIn, canClockOut, canConfirm, requiresConfirmation};
}

export {useAssignmentBusiness}