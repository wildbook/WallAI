export class Renderer {
	private canvas: HTMLCanvasElement;
    private ctx2d: CanvasRenderingContext2D;

    public get Context2D() : CanvasRenderingContext2D { return this.ctx2d; }
    
	constructor(canvas: HTMLCanvasElement) {
        this.canvas = canvas;
        this.ctx2d = canvas.getContext("2d");
    }
    
    public SetSize(width: number, height: number){
		this.canvas.width = width;
        this.canvas.height = height;
    }

    public Render() {
        var ctx = this.Context2D;
        ctx.rect(0, 0, this.canvas.width, this.canvas.height);
        ctx.fillStyle = "black";
        ctx.fill();
    }
}
