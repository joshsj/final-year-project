// clients.ts

export class JobClient extends BaseClient {
  get(): Promise<BriefJobDto[]> {
    // Omitted request setup, fetch, response parsing
  }
}
