import { store } from "@/store";

class BaseClient {
  protected async transformOptions(options: RequestInit): Promise<RequestInit> {
    const { accessToken } = store;

    if (!accessToken) {
      return options;
    }

    (
      options.headers as Record<string, string>
    ).authorization = `Bearer ${accessToken}`;

    return options;
  }
}
