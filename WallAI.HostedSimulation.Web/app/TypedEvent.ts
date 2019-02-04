import { IDisposable } from "./Interfaces/IDisposable";
import { IListener } from "./Interfaces/IListener";

export class TypedEvent<T> {
    private listeners: IListener<T>[] = [];
    private listenersOnce: IListener<T>[] = [];
  
    on = (listener: IListener<T>): IDisposable => {
        this.listeners.push(listener);
        return {
            dispose: () => this.off(listener)
        };
    }
  
    once = (listener: IListener<T>): void => {
        this.listenersOnce.push(listener);
    }
  
    off = (listener: IListener<T>) => {
        var callbackIndex = this.listeners.indexOf(listener);
        if (callbackIndex > -1) this.listeners.splice(callbackIndex, 1);
    }
  
    emit = (event: T) => {
        this.listeners.forEach(listener => listener(event));
        this.listenersOnce.forEach(listener => listener(event));
        this.listenersOnce = [];
    }
  
    pipe = (te: TypedEvent<T>): IDisposable => {
        return this.on((e) => te.emit(e));
    }
}