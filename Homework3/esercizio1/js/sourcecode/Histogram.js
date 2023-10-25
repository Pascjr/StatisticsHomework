"use strict";

class Histogram {

    constructor(calculationPoint, M, N, values, classesNumber, scalingFunction) {
        if (calculationPoint > N) return;

        this.M = M;
        this.N = N;
        this.calculationPoint = calculationPoint;
        this.numberOfClasses = classesNumber;
        this.scalingFunction = scalingFunction;
        this.bucketValues = Array(this.numberOfClasses).fill(0);
        this.effectiveMaxValue = 0;
        this.effectiveMinValue = 0;

        let tmpValues = Array(this.M).fill(0);

        for (let i = 0; i < this.M; i++) {
            tmpValues[i] = this.scalingFunction(values[i][this.calculationPoint], this.calculationPoint);
        }

        this.effectiveMaxValue = Math.max(...tmpValues);
        this.effectiveMinValue = Math.min(...tmpValues);

        let delta = (this.effectiveMaxValue - this.effectiveMinValue) / this.numberOfClasses;

        for (let i = 0; i < this.M; i++) {
            if (Math.abs(this.effectiveMaxValue - tmpValues[i]) < 1E-09) {
                this.bucketValues[this.numberOfClasses - 1]++;
            } else {
                this.bucketValues[Math.floor((tmpValues[i] - this.effectiveMinValue) / delta)]++;
            }
        }

    }
}