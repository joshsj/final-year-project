import {AssignmentDto} from "@/api/clients";
import {useAuth0} from "@auth0/auth0-vue";
import {computed} from "vue";
import {Job} from "@/store";

const useAssignmentBusiness = (job: Job) => {
    const {user} = useAuth0();

    const isUserAssigment = ({employeeProviderId}: AssignmentDto): boolean =>
        employeeProviderId === user.value?.sub;
    const userAssignment = computed(() => job.assignments?.find(isUserAssigment));

    const canClockIn = ({employeeProviderId, clockedIn}: AssignmentDto): boolean =>
        !clockedIn && isUserAssigment({employeeProviderId} as AssignmentDto);
    
    const canClockOut = ({employeeProviderId, clockedIn, clockedOut}: AssignmentDto): boolean =>
        clockedIn && !clockedOut && isUserAssigment({employeeProviderId} as AssignmentDto);
    
    const canConfirm = (ass : AssignmentDto) : boolean =>
        isUserAssigment(ass)
            ? false 
            : (ass.clockedIn || !ass.clockedOut) && (userAssignment.value?.clockedIn ?? false);

    return {isUserAssigment, userAssignment, canClockIn, canClockOut, canConfirm};
}

export {useAssignmentBusiness}