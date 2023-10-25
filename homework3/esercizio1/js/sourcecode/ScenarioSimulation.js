"use strict";

class ScenarioSimulation {

    constructor(M, N, p, successValue, failureValue, calculationPoint, numberOfBuckets, sF) {
        this.M = M;
        this.N = N + 1;
        this.p = p;
        this.scalingFunction = sF;
        this.successValue = successValue;
        this.failureValue = failureValue;
        this.simValues = Array(this.M).fill().map(() => Array(this.N).fill(0));
        this.coordinates = [];
        this.trajectoriesColor = Array(this.M).fill();
        this.maxValue = 0;
        this.minValue = 0;
 
        this.simulate();
       
        this.endValuesHistogram = new Histogram(N-1, this.M, this.N, this.simValues, numberOfBuckets, sF);
        this.midValuesHistogram = new Histogram(calculationPoint, this.M, this.N, this.simValues, numberOfBuckets, sF);
    }

    simulate() {
        for (let k = 0; k < this.M; k++) {
            let points = Array(this.N).fill().map(() => new Point());

            for (let i = 0; i < this.N; i++) {
                points[i].x = i;

                if (i > 0) {
                    if (Math.random() < this.p) {
                        this.simValues[k][i] = this.simValues[k][i - 1] + this.successValue;
                    } else {
                        this.simValues[k][i] = this.simValues[k][i - 1] + this.failureValue;
                    }

                    let scaledValue = this.scalingFunction(this.simValues[k][i], i);
                    if (scaledValue > this.maxValue) {
                        this.maxValue = scaledValue;
                    } else if (scaledValue < this.minValue) {
                        this.minValue = scaledValue;
                    }
                }

                points[i].y = this.scalingFunction(this.simValues[k][i], i);
            }

            this.coordinates.push(points);
            let r = Math.floor(Math.random() * 256);
            let g = Math.floor(Math.random() * 256);
            let b = Math.floor(Math.random() * 256);
            this.trajectoriesColor[k] = `rgb(${r},${g},${b})`;
        }
    }
}