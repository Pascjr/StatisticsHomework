"use strict";

const labelDateTime = document.getElementById("labelDateTime");
const myCanvas = document.getElementById("myCanvas");
const ctx = myCanvas.getContext("2d");


const inputM = document.getElementById("M");
const inputN = document.getElementById("N");
const inputp = document.getElementById("p");
const inputHistoT = document.getElementById("histoT");
const inputHistoBuckets = document.getElementById("histoBuckets");

let simulating = false;

ctx.transform(1, 0, 0, -1, 0, myCanvas.height);

window.addEventListener('resize', () => {
    ctx.transform(1, 0, 0, -1, 0, myCanvas.height);
});

ResizableRectangle.initialState.rectangle = new Rectangle(1, 1, 100, 100);
ResizableRectangle.initialState.ctx = ctx;
ResizableRectangle.initialState.outlineColor = "Red";
ResizableRectangle.initialState.lineWidth = 1;
ResizableRectangle.initialState.lineDash = [1, 1];

const ResizableRectangle1 = new ResizableRectangle(ResizableRectangle.initialState);

ResizableRectangle.initialState.rectangle = new Rectangle(60, 20, 100, 100);
ResizableRectangle.initialState.ctx = ctx;
ResizableRectangle.initialState.outlineColor = "Purple";
ResizableRectangle.initialState.lineWidth = 1;
ResizableRectangle.initialState.lineDash = [1, 1];

const ResizableRectangle2 = new ResizableRectangle(ResizableRectangle.initialState);

ResizableRectangle.initialState.rectangle = new Rectangle(100, 10, 100, 100);
ResizableRectangle.initialState.ctx = ctx;
ResizableRectangle.initialState.outlineColor = "Yellow";
ResizableRectangle.initialState.lineWidth = 1;
ResizableRectangle.initialState.lineDash = [1, 1];

const ResizableRectangle3 = new ResizableRectangle(ResizableRectangle.initialState);

ResizableRectangle.initialState.rectangle = new Rectangle(200, 50, 100, 100);
ResizableRectangle.initialState.ctx = ctx;
ResizableRectangle.initialState.outlineColor = "Blue";
ResizableRectangle.initialState.lineWidth = 1;
ResizableRectangle.initialState.lineDash = [1, 1];

const ResizableRectangle4 = new ResizableRectangle(ResizableRectangle.initialState);

let s1, s2, s3, s4;

function drawScene() {

    ctx.clearRect(0, 0, myCanvas.width, myCanvas.height);

    ResizableRectangle1.drawChart(s1);
    ResizableRectangle2.drawChart(s2);
    ResizableRectangle3.drawChart(s3);
    ResizableRectangle4.drawChart(s4);

}

function simulate() {

    //s1 = new ScenarioSimulation(inputM.value*1, 50, 0.9, -1, 1, 25, 10, (val, i) => val);
    s1 = new ScenarioSimulation(inputM.value * 1, inputN.value * 1, inputp.value * 1, -1, 1, inputHistoT.value * 1, inputHistoBuckets.value * 1, (val, i) => val);
    s2 = new ScenarioSimulation(inputM.value * 1, inputN.value * 1, inputp.value * 1, 1, 0, inputHistoT.value * 1, inputHistoBuckets.value * 1, (val, i) => val);
    s3 = new ScenarioSimulation(inputM.value * 1, inputN.value * 1, inputp.value * 1, 1, 0, inputHistoT.value * 1, inputHistoBuckets.value * 1, relative);
    s4 = new ScenarioSimulation(inputM.value * 1, inputN.value * 1, inputp.value * 1, 1, 0, inputHistoT.value * 1, inputHistoBuckets.value * 1, normalize);
    setInterval(drawScene, 5);

}

function relative(val, i) {
    if (i <= 0) {
        return val;
    }
    return val / i;
}

function normalize(val, i) {
    if (i <= 0) {
        return val;
    }
    return val / Math.sqrt(i);
}