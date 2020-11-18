export interface Serializer {
  fromJson(json: any): any;
  toJson(resource: any): any;
}
