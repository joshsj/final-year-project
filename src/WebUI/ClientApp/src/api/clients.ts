//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.10.0 (NJsonSchema v10.6.10.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

import {store} from "@/store";
export class BaseClient {
    private readonly IsoDateRegex = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)$/ 
    
    private dateReviver : Parameters<JSON["parse"]>[1] = ({}, value) =>
        value && typeof value === "string" && this.IsoDateRegex.test(value)
            // ensure JS parses time relative to UTC
            // uses local timezone without this
            ? new Date(value + "+00:00") 
            : value;
    
    protected async transformResult(
        _url: string,
        response: Response,
        defaultCallback: (res: Response) => Promise<any>) {
        return await (response.status < 400
            ? response.text().then(text => text ? JSON.parse(text, this.dateReviver) : undefined)
            : defaultCallback(response));
    }

    protected async transformOptions(options: RequestInit): Promise<RequestInit> {
        const {accessToken} = store;

        if (!accessToken) {
            return options;
        }

        (
            options.headers as Record<string, string>
        ).authorization = `Bearer ${accessToken}`;

        return options;
    }
}

export class ClockClient extends BaseClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super();
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    submitUnconfirmed(request: SubmitUnconfirmedClockCommand): Promise<void> {
        let url_ = this.baseUrl + "/api/Clock/submission/unconfirmed";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(request);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processSubmitUnconfirmed(_response));
        });
    }

    protected processSubmitUnconfirmed(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    submitConfirmed(request: SubmitConfirmedClockCommand): Promise<void> {
        let url_ = this.baseUrl + "/api/Clock/submission/confirmed";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(request);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processSubmitConfirmed(_response));
        });
    }

    protected processSubmitConfirmed(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    getConfirmationCode(confirmeeAssignmentId: string | undefined): Promise<ConfirmationCodeDto> {
        let url_ = this.baseUrl + "/api/Clock/confirmation-code?";
        if (confirmeeAssignmentId === null)
            throw new Error("The parameter 'confirmeeAssignmentId' cannot be null.");
        else if (confirmeeAssignmentId !== undefined)
            url_ += "ConfirmeeAssignmentId=" + encodeURIComponent("" + confirmeeAssignmentId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processGetConfirmationCode(_response));
        });
    }

    protected processGetConfirmationCode(response: Response): Promise<ConfirmationCodeDto> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as ConfirmationCodeDto;
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ConfirmationCodeDto>(null as any);
    }
}

export class JobClient extends BaseClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super();
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    get(): Promise<BriefJobDto[]> {
        let url_ = this.baseUrl + "/api/Job";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processGet(_response));
        });
    }

    protected processGet(response: Response): Promise<BriefJobDto[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as BriefJobDto[];
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<BriefJobDto[]>(null as any);
    }

    getAssignments(jobId: string | undefined): Promise<AssignmentDto[]> {
        let url_ = this.baseUrl + "/api/Job/assignment?";
        if (jobId === null)
            throw new Error("The parameter 'jobId' cannot be null.");
        else if (jobId !== undefined)
            url_ += "JobId=" + encodeURIComponent("" + jobId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.transformResult(url_, _response, (_response: Response) => this.processGetAssignments(_response));
        });
    }

    protected processGetAssignments(response: Response): Promise<AssignmentDto[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as AssignmentDto[];
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<AssignmentDto[]>(null as any);
    }
}

export interface BaseSubmitClockCommand {
    assignmentId: string;
    clockType: ClockType;
    coordinates: Coordinates;
}

export interface SubmitUnconfirmedClockCommand extends BaseSubmitClockCommand {
}

export enum ClockType {
    In = 0,
    Out = 1,
}

export interface Coordinates {
    latitude: number;
    longitude: number;
}

export interface SubmitConfirmedClockCommand extends BaseSubmitClockCommand {
    confirmationTokenValue: string;
}

export interface ConfirmationCodeDto {
    svgSource: string;
    timeRemaining: number;
}

export interface EntityDto {
    id: string;
}

export interface BriefJobDto extends EntityDto {
    title: string;
    description: string;
    start: Date;
    end: Date;
    locationTitle: string;
    assignmentCount: number;
}

export interface AssignmentDto extends EntityDto {
    employeeProviderId: string;
    employeeName: string;
    clockIn: ClockDto | undefined;
    clockOut: ClockDto | undefined;
}

export interface ClockDto extends EntityDto {
    at: Date;
    parentId: string | undefined;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    throw new ApiException(message, status, response, headers, result);
}