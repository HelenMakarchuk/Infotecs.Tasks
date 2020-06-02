/** Сущность "Пользователь" */
export class Account {
    id: number;
    login: string;
    password: string;
    salt: string;
}
