import {store} from "@/store";
import {BriefJobDto} from "@/api/clients";

class BaseClient {
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
        defaultCallback: (res: Response) => Promise<BriefJobDto[]>) {
        return await (response.status < 400
            ? response.text().then(text => JSON.parse(text, this.dateReviver))
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
