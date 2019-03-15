import { Subscription } from "./subscription";

export class Configuration {
    public eventId: number;
    public eventName: string;
    public applicationId: number;
    public applicationName: string;
    public topicId: number;
    public topicName: string;
    public publisherCallbackUrl: string;
    public subscribers: Subscription[] = [];
}