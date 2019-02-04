import { Renderer } from "./Renderer";
import { Logger } from "./Logger";
import { RemoteSimulation } from "./RemoteSimulation";
import { RemoteSimulationRenderer } from "RemoteSimulationRenderer";

class App {
	private renderer: Renderer;
	private logger: Logger = Logger.create(this);
	private remoteSimulation: RemoteSimulation;
	private remoteSimulationRenderer: RemoteSimulationRenderer;

	constructor() {
		this.logger.debug("App initialization started.");

		var canvas = document.createElement("canvas");
		this.logger.debug("Canvas created.");

		this.renderer = new Renderer(canvas);
		this.logger.debug("Renderer created.");

		document.body.innerHTML = "";
		document.body.appendChild(canvas);
		this.logger.debug("Canvas attached to HTML body.");
		
		window.onresize = () => { this.resize(); };
		this.resize();
		this.logger.debug("OnResize registered.");

		this.remoteSimulation = new RemoteSimulation("ws://localhost:8181")
		this.remoteSimulationRenderer = new RemoteSimulationRenderer(this.renderer, this.remoteSimulation);
		this.logger.info("App initialized.");
	}

	private resize() {
		var height = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
		var width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;

		this.renderer.SetSize(width, height);
		this.renderer.Render();
	}
}

export = new App;