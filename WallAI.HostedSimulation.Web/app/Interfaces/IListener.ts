export interface IListener<T> {
    (event: T): any;
}