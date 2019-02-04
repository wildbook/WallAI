export class Logger {
    private source: string;
    constructor(type: string) { this.source = type; }

    public static create<T>(type: T) { return new Logger(type.constructor.name); }
    
    public info(string: string, data?: any) { this.log(this.source, LogLevel.Info, string, data); }
    public debug(string: string, data?: any) { this.log(this.source, LogLevel.Debug, string, data); }
    public warning(string: string, data?: any) { this.log(this.source, LogLevel.Warning, string, data); }
    public error(string: string, data?: any) { this.log(this.source, LogLevel.Error, string, data); }

    private log(source: string, level: LogLevel, string: string, data?: any) {
        var date = new Date();
        var str = `${date.toLocaleDateString()} - ${date.toLocaleTimeString()} | ${source} | ${string}`;

        switch(level) {
            case LogLevel.Debug:   console.debug(str, data); break;
            case LogLevel.Info:    console.info(str, data);  break;
            case LogLevel.Warning: console.warn(str, data);  break;
            case LogLevel.Error:   console.error(str, data); break;
        }
    }    
}

enum LogLevel {
    Debug   = "Debug",
    Info    = "Info",
    Warning = "Warning",
    Error   = "Error",
}