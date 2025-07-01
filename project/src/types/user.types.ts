export enum RoleType {
    User = "user",
    Admin = "admin",
    Trainer = "trainer"
  }
export type UserType = {
    id: number,
    name: string,
    role: RoleType,
    phone: string,
    email: string,
    address: string,
}
